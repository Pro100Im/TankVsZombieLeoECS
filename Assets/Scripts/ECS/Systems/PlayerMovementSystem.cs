using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Systems
{
	public class PlayerMovementSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
	{
		private EcsCustomInject<TankInput> _tankInput;

		private readonly EcsFilterInject<Inc<TankMovementComponent>> _tankMovementComponents = default;

		private float _currentSpeed;
		private float _currentRotation;

		public void Init( IEcsSystems systems )
		{
			_currentSpeed = 0;

			_tankInput.Value.ActionMap.Move.performed += SetDirection;
			_tankInput.Value.ActionMap.Move.canceled  += SetDirection;
		}

		public void Run( IEcsSystems systems )
		{
			foreach( var entity in _tankMovementComponents.Value )
			{
				var tankMovementComponent = _tankMovementComponents.Pools.Inc1.Get( entity );
				var rb              = tankMovementComponent.rb;
				var enginePower           = tankMovementComponent.EnginePower;
				var maxSpeed              = tankMovementComponent.MaxSpeed;
				var rotationSpeed         = tankMovementComponent.RotationSpeed;
				
				if( _currentRotation != 0 )
					rb.rotation -= _currentRotation * rotationSpeed * Time.fixedDeltaTime;

				rb.AddRelativeForceY( _currentSpeed * enginePower );
				rb.linearVelocityY = Mathf.Clamp( rb.linearVelocityY, -maxSpeed, maxSpeed );
			}
		}

		private void SetDirection( InputAction.CallbackContext context )
		{
			var input = context.ReadValue<Vector2>( ).normalized;

			_currentSpeed    = input.y;
			_currentRotation = input.x;
		}

		public void Destroy( IEcsSystems systems )
		{
			_tankInput.Value.ActionMap.Move.performed -= SetDirection;
			_tankInput.Value.ActionMap.Move.canceled  -= SetDirection;
		}
	}
}