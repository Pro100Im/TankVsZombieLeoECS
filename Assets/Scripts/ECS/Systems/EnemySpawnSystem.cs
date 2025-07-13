using ECS.Components;
using ECS.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<InPoolTag> _inPool;
        private EcsPool<EnemyRefsComponent> _refPool;

        private EcsFilter _bigZombieFilter;
        private EcsFilter _smallZombieFilter;

        private EnemiesSpawnerData _enemiesSpawnerData;

        private float _spawnTime;

        public EnemySpawnSystem(EnemiesSpawnerData enemiesSpawnerData)
        {
            _enemiesSpawnerData = enemiesSpawnerData;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inPool = _world.GetPool<InPoolTag>();
            _refPool = _world.GetPool<EnemyRefsComponent>();

            _bigZombieFilter = _world.Filter<InPoolTag>().Inc<EnemyRefsComponent>().Inc<BigEnemyTag>().End();
            _smallZombieFilter = _world.Filter<InPoolTag>().Inc<EnemyRefsComponent>().Exc<BigEnemyTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            SpawnZombiesPeriodically();
        }

        private void SpawnZombiesPeriodically()
        {
            if(Time.time < _spawnTime)
                return;

            SpawnZombie();
        }

        private void SpawnZombie()
        {
            var minSpawnRadius = _enemiesSpawnerData.MinSpawnRadius;
            var minSpawnRadius2 = _enemiesSpawnerData.MinSpawnRadius2;
            var maxSpawnRadius = _enemiesSpawnerData.MaxSpawnRadius;
            var maxSpawnRadius2 = _enemiesSpawnerData.MaxSpawnRadius2;
            var pointRadius = _enemiesSpawnerData.PointRadius;
            var obstacleLayer = _enemiesSpawnerData.ObstacleLayer;
            var smallZombieProbability = _enemiesSpawnerData.SmallZombieProbability;

            var possibleDistancesX = new int[] { minSpawnRadius, minSpawnRadius2, maxSpawnRadius, maxSpawnRadius2 };
            var possibleDistancesY = new int[] { minSpawnRadius, minSpawnRadius2, maxSpawnRadius, maxSpawnRadius2 };

            var randomIndexX = Random.Range(0, possibleDistancesX.Length);
            var randomIndexY = Random.Range(0, possibleDistancesY.Length);

            var randomDistanceX = possibleDistancesX[randomIndexX];
            var randomDistanceY = possibleDistancesY[randomIndexY];

            var randomPoint = /*_target.position +*/ new Vector3(randomDistanceX, randomDistanceY, 0);

            if(!Physics2D.OverlapCircle(randomPoint, pointRadius, obstacleLayer))
            {
                var minSpawnDelay = _enemiesSpawnerData.MinSpawnDelay;
                var maxSpawnDelay = _enemiesSpawnerData.MaxSpawnDelay;
                var filter = Random.value < smallZombieProbability ? _smallZombieFilter : _bigZombieFilter;

                _spawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);

                if(filter.GetEntitiesCount() > 0)
                {
                    var zombieEntity = filter.GetRawEntities()[0];

                    ref var zombieRef = ref _refPool.Get(zombieEntity);

                    if(!_inPool.Has(zombieEntity))
                        return;

                    _inPool.Del(zombieEntity);

                    zombieRef.GameObject.transform.position = randomPoint;
                    zombieRef.GameObject.SetActive(true);
                }
            }
        }
    }
}
