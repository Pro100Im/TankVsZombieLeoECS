using AB_Utility.FromSceneToEntityConverter;
using ECS.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;
using Unity.Cinemachine;
using UnityEngine;

namespace ECS.Mono
{
    public class GameEcsStartup : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;

        private TankInput _input;

        private EcsSystems _initSystems;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Start()
        {
            var cinamachine = FindFirstObjectByType(typeof(CinemachineCamera)) as CinemachineCamera;

            var world = new EcsWorld();

            _input = new TankInput();

            _initSystems = new EcsSystems(world);
            _updateSystems = new EcsSystems(world);
            _fixedUpdateSystems = new EcsSystems(world);

            AddDebugSystems(_initSystems);
            AddDebugSystems(_updateSystems);
            AddDebugSystems(_fixedUpdateSystems);

            _initSystems
                .Add(new PlayerSpawnSystem())
                .ConvertScene()
                .Inject(_playerPrefab, cinamachine);

            _updateSystems
                .Add(new PlayerAudioSystem())
                .Add(new TurretSystem())
                .Add(new TurretModeIndicatorSystem())
                .Inject(_input);

            _fixedUpdateSystems
                .Add(new PlayerMovementSystem())
                .Inject(_input);

            _initSystems.Init();
            _updateSystems.Init();
            _fixedUpdateSystems.Init();

            _input.Enable();

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
        }
    }
}