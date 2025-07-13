using UnityEngine;

namespace ECS.Data
{
    [CreateAssetMenu(fileName = "EnemiesSpawnerData", menuName = "Data/EnemiesSpawnerData", order = 0)]
    public class EnemiesSpawnerData : ScriptableObject
    {
        [field: SerializeField] public float PointRadius { get; private set; } = 2f;
        [field: SerializeField] public float SmallZombieProbability { get; private set; } = .75f;
        [field: SerializeField] public float MaxSpawnDelay { get; private set; } = 5f;
        [field: SerializeField] public float MinSpawnDelay { get; private set; } = 2f;

        [field: SerializeField] public int MaxSpawnRadius { get; private set; } = 10;
        [field: SerializeField] public int MaxSpawnRadius2 { get; private set; } = 15;
        [field: SerializeField] public int MinSpawnRadius { get; private set; } = -10;
        [field: SerializeField] public int MinSpawnRadius2 { get; private set; } = -15;
        [field: SerializeField] public int SmallZombiePoolSize { get; private set; } = 15;
        [field: SerializeField] public int BigZombiePoolSize { get; private set; } = 5;

        [field: SerializeField] public GameObject SmallZombiePrefab { get; private set; }
        [field: SerializeField] public GameObject BigZombiePrefab { get; private set; }
        [field: SerializeField] public LayerMask ObstacleLayer { get; private set; }
    }
}