using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TankMovementComponent>> _tankMovementComponents = default;
        private readonly EcsCustomInject<TankInput> _tankInput;

        public void Run(IEcsSystems systems)
        {
            if(_tankMovementComponents.Value.GetEntitiesCount() > 0)
            {
                var entity = _tankMovementComponents.Value.GetRawEntities()[0];
                ref var tankMovementComponent = ref _tankMovementComponents.Pools.Inc1.Get(entity);

                var rb = tankMovementComponent.rb;
                var enginePower = tankMovementComponent.EnginePower;
                var maxSpeed = tankMovementComponent.MaxSpeed;
                var rotationSpeed = tankMovementComponent.RotationSpeed;

                var input = _tankInput.Value.ActionMap.Move.ReadValue<Vector2>().normalized;
                var currentSpeed = input.y;
                var currentRotation = input.x;

                if(currentRotation != 0)
                    rb.rotation -= currentRotation * rotationSpeed * Time.fixedDeltaTime;

                rb.AddRelativeForceY(currentSpeed * enginePower);
                rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxSpeed, maxSpeed);
            }
        }
    }
}