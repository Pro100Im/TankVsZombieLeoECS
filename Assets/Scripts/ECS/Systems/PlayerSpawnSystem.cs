using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerSpawnSystem : IEcsInitSystem
    {
        private GameObject _playerPrefab;
        private CinemachineCamera _cinemachine;

        public PlayerSpawnSystem(GameObject playerPrefab, CinemachineCamera cinemachine)
        {
            _playerPrefab = playerPrefab;
            _cinemachine = cinemachine;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerGo = EcsConverter.InstantiateAndCreateEntity(_playerPrefab, world);

            _cinemachine.Follow = playerGo.transform;
        }
    }
}