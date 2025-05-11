using Game.Score;
using System.Collections;
using UnityEngine;

namespace Game.Zombie
{
    public sealed class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private float pointRadius = 2f; 
        [SerializeField] private float smallZombieProbability = .75f;
        [SerializeField] private float maxSpawnDelay = 5f;
        [SerializeField] private float minSpawnDelay = 2f;
        [Space]
        [SerializeField] private int maxSpawnRadius = 10;
        [SerializeField] private int maxSpawnRadius2 = 15;
        [SerializeField] private int minSpawnRadius = -10;
        [SerializeField] private int minSpawnRadius2 = -15;
        [SerializeField] private int spawnAttempts = 10;
        [Space]
        [SerializeField] private ZombieController smallZombiePrefab;
        [SerializeField] private ZombieController bigZombiePrefab;
        [SerializeField] private LayerMask obstacleLayer;

        private IKillCounter _killCounter;
        private Transform _target;

        public void Init(Transform target, IKillCounter killCounter)
        {
            _target = target;
            _killCounter = killCounter;

            StartCoroutine(SpawnZombiesPeriodically());
        }

        private IEnumerator SpawnZombiesPeriodically()
        {
            while(true)
            {
                float delay = Random.Range(minSpawnDelay, maxSpawnDelay);

                yield return new WaitForSeconds(delay);

                SpawnZombie();
            }
        }

        private void SpawnZombie()
        {
            if(_target == null) 
                return;

            for(int i = 0; i < spawnAttempts; i++)
            {
                int[] possibleDistancesX = { minSpawnRadius, minSpawnRadius2, maxSpawnRadius, maxSpawnRadius2 };
                int[] possibleDistancesY = { minSpawnRadius, minSpawnRadius2, maxSpawnRadius, maxSpawnRadius2 };

                int randomIndexX = Random.Range(0, possibleDistancesX.Length);
                int randomIndexY = Random.Range(0, possibleDistancesY.Length);

                int randomDistanceX = possibleDistancesX[randomIndexX];
                int randomDistanceY = possibleDistancesY[randomIndexY];

                var randomPoint = _target.position + new Vector3(randomDistanceX, randomDistanceY, 0);

                if(!Physics2D.OverlapCircle(randomPoint, pointRadius, obstacleLayer))
                {
                    var zombiePrefab = Random.value < smallZombieProbability ? smallZombiePrefab : bigZombiePrefab;
                    var zombie = Instantiate(zombiePrefab, randomPoint, Quaternion.identity);
                    zombie.OnDie += _killCounter.KillCountIncrement;
                    zombie.SetTarget(_target);

                    return;
                }
            }
        }
    }
}