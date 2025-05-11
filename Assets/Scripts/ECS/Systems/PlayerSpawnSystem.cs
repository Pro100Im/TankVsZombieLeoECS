using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Cinemachine;
using UnityEngine;

namespace ECS.Systems
{
	public class PlayerSpawnSystem : IEcsInitSystem
	{
		private readonly EcsWorldInject _defaultWorld = default;

		private EcsCustomInject<GameObject>        _playerPrefab;
		private EcsCustomInject<CinemachineCamera> _cinamachine;

		public void Init( IEcsSystems systems )
		{
			var playerGo = EcsConverter.InstantiateAndCreateEntity( _playerPrefab.Value, _defaultWorld.Value );

			_cinamachine.Value.Follow = playerGo.transform;
		}
	}
}