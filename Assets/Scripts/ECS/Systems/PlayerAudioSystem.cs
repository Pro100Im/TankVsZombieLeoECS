using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerAudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<TankMovementComponent> _movementPool;
        private EcsPool<TankAudioComponent> _audioPool;

        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _movementPool = _world.GetPool<TankMovementComponent>();
            _audioPool = _world.GetPool<TankAudioComponent>();

            _filter = _world.Filter<TankMovementComponent>().Inc<TankAudioComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter)
            {
                if(!_audioPool.Has(entity)) continue;

                var move = _movementPool.Get(entity);
                var audio = _audioPool.Get(entity);

                var rb = move.Rb;
                var maxSpeed = move.MaxSpeed;

                var audioSource = audio.AudioSourceEngine;
                var minPitch = audio.MinSpeed;
                var maxPitch = audio.MaxSpeed;

                var velocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY);
                var value = velocity.magnitude / maxSpeed;

                var pitch = Mathf.Lerp(minPitch, maxPitch, value);
                audioSource.pitch = pitch;
            }
        }
    }
}