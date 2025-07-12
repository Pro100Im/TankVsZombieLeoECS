using AB_Utility.FromSceneToEntityConverter;
using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class GunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<InPoolTag> _inPool;
        private EcsPool<SpeedComponent> _speedPool;
        private EcsPool<TurretComponent> _turretPool;
        private EcsPool<BigGunComponent> _bigPool;
        private EcsPool<MiniGunComponent> _miniPool;
        private EcsPool<BulletRefsComponent> _refPool;
        private EcsPool<PlayerInputComponent> _inputPool;

        private EcsFilter _gunFilter;
        private EcsFilter _playerInputFilter;
        private EcsFilter _bigBulletFilter;
        private EcsFilter _miniBulletFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inputPool = _world.GetPool<PlayerInputComponent>();
            _turretPool = _world.GetPool<TurretComponent>();
            _bigPool = _world.GetPool<BigGunComponent>();
            _miniPool = _world.GetPool<MiniGunComponent>();
            _inPool = _world.GetPool<InPoolTag>();
            _speedPool = _world.GetPool<SpeedComponent>();
            _refPool = _world.GetPool<BulletRefsComponent>();

            _playerInputFilter = _world.Filter<PlayerInputComponent>().End();
            _gunFilter = _world.Filter<TurretComponent>().Inc<BigGunComponent>().Inc<MiniGunComponent>().End();
            _bigBulletFilter = _world.Filter<InPoolTag>().Inc<BulletRefsComponent>().Inc<SpeedComponent>().Inc<LifeTimeComponent>()
                .Inc<ExplosiveComponent>().End();
            _miniBulletFilter = _world.Filter<InPoolTag>().Inc<BulletRefsComponent>().Inc<SpeedComponent>().Inc<LifeTimeComponent>()
                .Exc<ExplosiveComponent>().End();

            var gunEntity = _gunFilter.GetRawEntities()[0];
            var bigGun = _bigPool.Get(gunEntity);
            var miniGun = _miniPool.Get(gunEntity);

            InitBulletPool(bigGun.BulletPrefab, bigGun.BulletPoolSize);
            InitBulletPool(miniGun.BulletPrefab, miniGun.BulletPoolSize);
        }

        private void InitBulletPool(GameObject prefab, int poolSize)
        {
            for(int i = 0; i < poolSize; i++)
            {
                var go = EcsConverter.InstantiateAndCreateEntity(prefab, _world);
                go.SetActive(false);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var inputEntity = _playerInputFilter.GetRawEntities()[0];
            var input = _inputPool.Get(inputEntity);

            if(!input.FireRequested)
                return;

            var gunEntity = _gunFilter.GetRawEntities()[0];
            ref var turret = ref _turretPool.Get(gunEntity);

            if(turret.IsBigGun)
                FireBullet(gunEntity, true);
            else
                FireBullet(gunEntity, false);
        }

        private void FireBullet(int gunEntity, bool isBig)
        {
            var filter = isBig ? _bigBulletFilter : _miniBulletFilter;
            var gunComp = isBig ? (object)_bigPool.Get(gunEntity) : _miniPool.Get(gunEntity);
            var bulletPrefab = isBig ? ((BigGunComponent)gunComp).BulletPrefab : ((MiniGunComponent)gunComp).BulletPrefab;
            var firePoint = isBig ? ((BigGunComponent)gunComp).FirePoint : ((MiniGunComponent)gunComp).FirePoint;
            var fireEffect = isBig ? ((BigGunComponent)gunComp).FireEffect : ((MiniGunComponent)gunComp).FireEffect;

            if(filter.GetEntitiesCount() > 0)
            {
                var bulletEntity = filter.GetRawEntities()[0];

                ref var bulletRef = ref _refPool.Get(bulletEntity);
                ref var speedComp = ref _speedPool.Get(bulletEntity);

                if(!_inPool.Has(bulletEntity))
                    return;

                _inPool.Del(bulletEntity);

                bulletRef.GameObject.transform.position = firePoint.position;
                bulletRef.GameObject.transform.rotation = firePoint.rotation;
                bulletRef.GameObject.SetActive(true);
                bulletRef.Rb.AddForce(bulletRef.GameObject.transform.up * speedComp.Speed, ForceMode2D.Impulse);

                fireEffect.Play();
            }
            else
            {
                var go = EcsConverter.InstantiateAndCreateEntity(bulletPrefab, _world);
                go.SetActive(false);
            }
        }
    }
}