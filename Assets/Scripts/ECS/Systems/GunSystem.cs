using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace ECS.Systems
{
	public class GunSystem : IEcsInitSystem, IEcsDestroySystem
	{
		private EcsCustomInject<TankInput> _tankInput;

		private readonly EcsFilterInject<Inc<BigGunComponent, MiniGunComponent, TurretComponent>> _gunComponents = default;

		private int _entity;

		public void Init( IEcsSystems systems )
		{
			foreach( var entity in _gunComponents.Value )
				_entity = entity;

			_tankInput.Value.ActionMap.Fire.performed += FireInput;
		}

		private void FireInput( InputAction.CallbackContext context )
		{
			var turret = _gunComponents.Pools.Inc3.Get( _entity );

			if( turret.IsBigGun )
			{
				var bigGun = _gunComponents.Pools.Inc1.Get( _entity );
			}
			else
			{
				var miniGun = _gunComponents.Pools.Inc2.Get( _entity );
			}
		}

		public void Destroy( IEcsSystems systems )
		{
			_tankInput.Value.ActionMap.Fire.performed -= FireInput;
		}
	}
}