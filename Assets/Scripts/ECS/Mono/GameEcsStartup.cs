using AB_Utility.FromSceneToEntityConverter;
using ECS.Data;
using ECS.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using Unity.Cinemachine;
using UnityEngine;

namespace ECS.Mono
{
    public class GameEcsStartup : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private EnemiesSpawnerData _enemiesSpawnerData;

        private TankInput _input;

        private EcsSystems _initSystems;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private EcsSystems _lateUpdateSystems;

        private void Start()
        {
            var cinamachine = FindFirstObjectByType(typeof(CinemachineCamera)) as CinemachineCamera;

            var world = new EcsWorld();

            _input = new TankInput();

            _initSystems = new EcsSystems(world);
            _updateSystems = new EcsSystems(world);
            _fixedUpdateSystems = new EcsSystems(world);
            _lateUpdateSystems = new EcsSystems(world);

            AddDebugSystems(_initSystems);
            AddDebugSystems(_updateSystems);
            AddDebugSystems(_fixedUpdateSystems);

            _initSystems
                .Add(new PlayerSpawnSystem(_playerPrefab, cinamachine))
                .ConvertScene()
                .Init();

            _updateSystems
                .Add(new PlayerInputSystem(_input))
                .Add(new PlayerAudioSystem())
                .Add(new TurretSystem())
                .Add(new TurretModeIndicatorSystem())
                .Add(new CreatePoolSystem(_enemiesSpawnerData))
                .Add(new EnemySpawnSystem(_enemiesSpawnerData))
                .Add(new GunSystem())
                .Add(new LifetimeSystem())
                .Add(new ZombieAttackSystem())
                .Add(new BulletCollisionSystem())
                .Add(new HpSystem())
                .Add(new ReturnToPoolSystem())
                .Init();

            _fixedUpdateSystems
                .Add(new PlayerMovementSystem())
                .Add(new ZombieFollowSystem())
                .Init();

            _lateUpdateSystems
                .Add(new HpBarSystem())
                .Init();

            SceneLoader.Instance.FadeScreen(0);
        }

        private void AddDebugSystems(EcsSystems systems)
        {
#if UNITY_EDITOR
            systems
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsSystemsDebugSystem());
#endif
        }

        private void Update()
        {
            _updateSystems.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems.Run();
        }

        private void LateUpdate()
        {
            _lateUpdateSystems.Run();
        }

        private void OnDestroy()
        {
            _initSystems?.Destroy();
            _initSystems?.GetWorld()?.Destroy();
            _initSystems = null;

            _updateSystems?.Destroy();
            _updateSystems?.GetWorld()?.Destroy();
            _updateSystems = null;

            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems?.GetWorld()?.Destroy();
            _fixedUpdateSystems = null;

            _lateUpdateSystems?.Destroy();
            _lateUpdateSystems?.GetWorld()?.Destroy();
            _lateUpdateSystems = null;
        }
    }
}