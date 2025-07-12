using AB_Utility.FromSceneToEntityConverter;
using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class CreatePoolSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<BigGunComponent> _bigPool;
        private EcsPool<MiniGunComponent> _miniPool;

        private EcsFilter _gunFilter;
        private EcsFilter _bigBulletFilter;
        private EcsFilter _miniBulletFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _bigPool = _world.GetPool<BigGunComponent>();
            _miniPool = _world.GetPool<MiniGunComponent>();

            _gunFilter = _world.Filter<TurretComponent>().Inc<BigGunComponent>().Inc<MiniGunComponent>().End();
            _bigBulletFilter = _world.Filter<InPoolTag>().Inc<BulletRefsComponent>().Inc<SpeedComponent>().Inc<LifeTimeComponent>()
                .Inc<ExplosiveComponent>().End();
            _miniBulletFilter = _world.Filter<InPoolTag>().Inc<BulletRefsComponent>().Inc<SpeedComponent>().Inc<LifeTimeComponent>()
                .Exc<ExplosiveComponent>().End();

            var gunEntity = _gunFilter.GetRawEntities()[0];
            var bigGun = _bigPool.Get(gunEntity);
            var miniGun = _miniPool.Get(gunEntity);

            InitPool(bigGun.BulletPrefab, bigGun.BulletPoolSize);
            InitPool(miniGun.BulletPrefab, miniGun.BulletPoolSize);
        }

        private void InitPool(GameObject prefab, int poolSize)
        {
            for(int i = 0; i < poolSize; i++)
                CreatePoolObject(prefab);
        }

        private void CreatePoolObject(GameObject prefab)
        {
            var go = EcsConverter.InstantiateAndCreateEntity(prefab, _world);
            go.SetActive(false);
        }

        public void Run(IEcsSystems systems)
        {
            var gunEntity = _gunFilter.GetRawEntities()[0];
            var bigGun = _bigPool.Get(gunEntity);
            var miniGun = _miniPool.Get(gunEntity);

            if(_miniBulletFilter.GetEntitiesCount() <= 0)
            {
                CreatePoolObject(miniGun.BulletPrefab);
            }

            if(_bigBulletFilter.GetEntitiesCount() <= 0)
            {
                CreatePoolObject(bigGun.BulletPrefab);
            }
        }
    }
}
