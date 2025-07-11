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
        private readonly EcsFilterInject<Inc<InPoolTag, BulletRefsComponent, SpeedComponent, LifeTimeComponent>, Exc<ExplosiveComponent>> _miniBulletRefsInPoolComponents = default;
        private readonly EcsFilterInject<Inc<InPoolTag, BulletRefsComponent, SpeedComponent, LifeTimeComponent, ExplosiveComponent>> _bigBulletRefsInPoolComponents = default;

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
            var pool = _defaultWorld.Value.GetPool<InPoolTag>();

            if(turretComponent.IsBigGun)
            {
                var bigGunComponent = _gunComponents.Pools.Inc2.Get(entity);
                var bulletFilter = _bigBulletRefsInPoolComponents.Value;

                if(bulletFilter.GetEntitiesCount() > 0)
                {
                    var bulletEntity = bulletFilter.GetRawEntities()[0];
                    var bulletRefComponent = _bigBulletRefsInPoolComponents.Pools.Inc2.Get(bulletEntity);
                    var speedComponent = _bigBulletRefsInPoolComponents.Pools.Inc3.Get(bulletEntity);

                    if(!pool.Has(bulletEntity))
                        return;

                    var returnToPool = _defaultWorld.Value.GetPool<ReturnToPoolTag>();
                    Debug.Log($"Перед выстрелом: у пули {bulletEntity} есть ReturnToPoolTag? {returnToPool.Has(bulletEntity)}");

                    pool.Del(bulletEntity);

                    bulletRefComponent.GameObject.transform.position = bigGunComponent.FirePoint.position;
                    bulletRefComponent.GameObject.transform.rotation = bigGunComponent.FirePoint.rotation;
                    bulletRefComponent.GameObject.SetActive(true);
                    bulletRefComponent.Rb.AddForce(bulletRefComponent.GameObject.transform.up * speedComponent.Speed, ForceMode2D.Impulse);

                    bigGunComponent.FireEffect.Play();
                }

                if(bulletFilter.GetEntitiesCount() == 0)
                {
                    var go = EcsConverter.InstantiateAndCreateEntity(bigGunComponent.BulletPrefab, _defaultWorld.Value);
                    go.SetActive(false);
                }
            }
            else
            {
                var miniGunComponent = _gunComponents.Pools.Inc3.Get(entity);
                var bulletFilter = _miniBulletRefsInPoolComponents.Value;

                if(bulletFilter.GetEntitiesCount() > 0)
                {
                    var bulletEntity = bulletFilter.GetRawEntities()[0];
                    var bulletRefComponent = _miniBulletRefsInPoolComponents.Pools.Inc2.Get(bulletEntity);
                    var speedComponent = _miniBulletRefsInPoolComponents.Pools.Inc3.Get(bulletEntity);

                    if(!pool.Has(bulletEntity))
                        return;
                    
                    pool.Del(bulletEntity);

                    bulletRefComponent.GameObject.transform.position = miniGunComponent.FirePoint.position;
                    bulletRefComponent.GameObject.transform.rotation = miniGunComponent.FirePoint.rotation;
                    bulletRefComponent.GameObject.SetActive(true);
                    bulletRefComponent.Rb.AddForce(bulletRefComponent.GameObject.transform.up * speedComponent.Speed, ForceMode2D.Impulse);

                    miniGunComponent.FireEffect.Play();
                }

                if(bulletFilter.GetEntitiesCount() == 0)
                {
                    var go = EcsConverter.InstantiateAndCreateEntity(miniGunComponent.BulletPrefab, _defaultWorld.Value);
                    go.SetActive(false);
                }
            }
        }
    }
}