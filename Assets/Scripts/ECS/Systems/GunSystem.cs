using AB_Utility.FromSceneToEntityConverter;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class GunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;
        private readonly EcsFilterInject<Inc<TurretComponent, BigGunComponent, MiniGunComponent>> _gunComponents = default;
        private readonly EcsFilterInject<Inc<InPoolTag, BulletRefsComponent>,Exc<ExplosiveComponent>> _miniBulletRefsInPoolComponents = default;
        private readonly EcsFilterInject<Inc<InPoolTag, BulletRefsComponent, ExplosiveComponent>> _bigBulletRefsInPoolComponents = default;

        public void Init(IEcsSystems systems)
        {
            var entity = _gunComponents.Value.GetRawEntities()[0];
            var bigGunComponent = _gunComponents.Pools.Inc2.Get(entity);
            var miniGunComponent = _gunComponents.Pools.Inc3.Get(entity);

            InitBulletPool(bigGunComponent.BulletPrefab, bigGunComponent.BulletPoolSize);
            InitBulletPool(miniGunComponent.BulletPrefab, miniGunComponent.BulletPoolSize);
        }

        private void InitBulletPool(GameObject prefab, int poolSize)
        {
            for(int i = 0; i < poolSize; i++)
            {
                var go = EcsConverter.InstantiateAndCreateEntity(prefab, _defaultWorld.Value);
                go.SetActive(false);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var entity = _gunComponents.Value.GetRawEntities()[0];
            var playerInputComponent = _playerInputComponents.Pools.Inc1.Get(entity);

            if(!playerInputComponent.FireRequested)
                return;

            var turretComponent = _gunComponents.Pools.Inc1.Get(entity);

            if(turretComponent.IsBigGun)
            {
                var bigGunComponent = _gunComponents.Pools.Inc2.Get(entity);

                var bulletFilter = _bigBulletRefsInPoolComponents.Value;

                if(bulletFilter.GetEntitiesCount() > 0)
                {
                    var bulletEntity = bulletFilter.GetRawEntities()[0];
                    var bulletRefComponen = _bigBulletRefsInPoolComponents.Pools.Inc2.Get(bulletEntity);

                    var pool = _defaultWorld.Value.GetPool<InPoolTag>();
                    if(pool.Has(bulletEntity))
                    {
                        pool.Del(bulletEntity);
                    }

                    bulletRefComponen.GameObject.SetActive(true);
                }

                if(bulletFilter.GetEntitiesCount() == 0)
                {
                    var go = EcsConverter.InstantiateAndCreateEntity(bigGunComponent.BulletPrefab, _defaultWorld.Value);
                    go.SetActive(false);
                }

                bigGunComponent.FireEffect.Play();
            }
            else
            {
                var miniGunComponent = _gunComponents.Pools.Inc3.Get(entity);

                var bulletFilter = _miniBulletRefsInPoolComponents.Value;

                if(bulletFilter.GetEntitiesCount() > 0)
                {
                    var bulletEntity = bulletFilter.GetRawEntities()[0];
                    var bulletRefComponen = _miniBulletRefsInPoolComponents.Pools.Inc2.Get(bulletEntity);

                    var pool = _defaultWorld.Value.GetPool<InPoolTag>();
                    if(pool.Has(bulletEntity))
                    {
                        pool.Del(bulletEntity);
                    }

                    bulletRefComponen.GameObject.SetActive(true);
                }
   
                if(bulletFilter.GetEntitiesCount() == 0)
                {
                    var go = EcsConverter.InstantiateAndCreateEntity(miniGunComponent.BulletPrefab, _defaultWorld.Value);
                    go.SetActive(false);
                }

                miniGunComponent.FireEffect.Play();
            }
        }
    }
}