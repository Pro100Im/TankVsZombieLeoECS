using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TankMovementComponent>> _tankMovementComponents = default;
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;

        public void Run(IEcsSystems systems)
        {
            if(_tankMovementComponents.Value.GetEntitiesCount() == 0 || _playerInputComponents.Value.GetEntitiesCount() == 0)
                return;

            var entity = _tankMovementComponents.Value.GetRawEntities()[0];
            var playerInputComponents = _playerInputComponents.Pools.Inc1.Get(entity);
            ref var tankMovementComponent = ref _tankMovementComponents.Pools.Inc1.Get(entity);

            var rb = tankMovementComponent.rb;
            var enginePower = tankMovementComponent.EnginePower;
            var maxSpeed = tankMovementComponent.MaxSpeed;
            var rotationSpeed = tankMovementComponent.RotationSpeed;

            var currentSpeed = playerInputComponents.DirectionInput.y;
            var currentRotation = playerInputComponents.DirectionInput.x;

            if(currentRotation != 0)
                rb.rotation -= currentRotation * rotationSpeed * Time.fixedDeltaTime;

            rb.AddRelativeForceY(currentSpeed * enginePower);
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxSpeed, maxSpeed);
        }
    }
}