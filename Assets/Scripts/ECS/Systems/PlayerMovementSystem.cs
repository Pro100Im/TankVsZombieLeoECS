using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<TankMovementComponent> _movementPool;
        private EcsPool<PlayerInputComponent> _inputPool;

        private EcsFilter _movementFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _movementPool = _world.GetPool<TankMovementComponent>();
            _inputPool = _world.GetPool<PlayerInputComponent>();

            _movementFilter = _world.Filter<TankMovementComponent>().Inc<PlayerInputComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_movementFilter.GetEntitiesCount() == 0)
                return;

            var entity = _movementFilter.GetRawEntities()[0];

            ref var move = ref _movementPool.Get(entity);
            ref var input = ref _inputPool.Get(entity);

            var rb = move.rb;
            var enginePower = move.EnginePower;
            var maxSpeed = move.MaxSpeed;
            var rotationSpeed = move.RotationSpeed;

            var currentSpeed = input.DirectionInput.y;
            var currentRotation = input.DirectionInput.x;

            if(currentRotation != 0f)
                rb.rotation -= currentRotation * rotationSpeed * Time.fixedDeltaTime;

            rb.AddRelativeForceY(currentSpeed * enginePower);
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxSpeed, maxSpeed);
        }
    }
}