using AB_Utility.FromSceneToEntityConverter;
using ECS.Components;
using ECS.Data;
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

        private EcsFilter _bigZombieFilter;
        private EcsFilter _smallZombieFilter;

        private EnemiesSpawnerData _enemiesSpawnerData;

        public CreatePoolSystem(EnemiesSpawnerData enemiesSpawnerData)
        {
            _enemiesSpawnerData = enemiesSpawnerData;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            CreateBulletPools();
            CreateEnemiesPool();
        }

        private void CreateBulletPools()
        {
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

        private void CreateEnemiesPool()
        {
            _bigZombieFilter = _world.Filter<InPoolTag>().Inc<EnemyRefsComponent>().Inc<BigEnemyTag>().End();
            _smallZombieFilter = _world.Filter<InPoolTag>().Inc<EnemyRefsComponent>().Exc<BigEnemyTag>().End();

            var bigZombiePrefab = _enemiesSpawnerData.BigZombiePrefab;
            var bigZombiePoolSize = _enemiesSpawnerData.BigZombiePoolSize;

            var smallZombiePrefab = _enemiesSpawnerData.SmallZombiePrefab;
            var smallZombiePoolSize = _enemiesSpawnerData.SmallZombiePoolSize;

            InitPool(bigZombiePrefab, bigZombiePoolSize);
            InitPool(smallZombiePrefab, smallZombiePoolSize);
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
            CheckBulletPools();
            CheckEnemiesPools();
        }

        private void CheckBulletPools()
        {
            var gunEntity = _gunFilter.GetRawEntities()[0];
            var bigGun = _bigPool.Get(gunEntity);
            var miniGun = _miniPool.Get(gunEntity);

            if(_miniBulletFilter.GetEntitiesCount() <= 0)
                CreatePoolObject(miniGun.BulletPrefab);

            if(_bigBulletFilter.GetEntitiesCount() <= 0)
                CreatePoolObject(bigGun.BulletPrefab);
        }

        private void CheckEnemiesPools()
        {
            if(_smallZombieFilter.GetEntitiesCount() <= 0)
                CreatePoolObject(_enemiesSpawnerData.SmallZombiePrefab);

            if(_bigZombieFilter.GetEntitiesCount() <= 0)
                CreatePoolObject(_enemiesSpawnerData.BigZombiePrefab);
        }
    }
}
