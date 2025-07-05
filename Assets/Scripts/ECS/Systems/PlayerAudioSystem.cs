using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerAudioSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TankMovementComponent>> _tankMovementComponents = default;
        private readonly EcsFilterInject<Inc<TankAudioComponent>> _tankAudioComponents = default;

        public void Run(IEcsSystems systems)
        {
            foreach(var entity in _tankMovementComponents.Value)
            {
                var tankMovementComponent = _tankMovementComponents.Pools.Inc1.Get(entity);
                var tankAudioComponent = _tankAudioComponents.Pools.Inc1.Get(entity);

                var rb = tankMovementComponent.rb;
                var maxValue = tankMovementComponent.MaxSpeed;
                var audioSourceEngine = tankAudioComponent.AudioSourceEngine;
                var minSpeed = tankAudioComponent.MinSpeed;
                var maxSpeed = tankAudioComponent.MaxSpeed;
                var value = (Mathf.Abs(rb.linearVelocityY) + Mathf.Abs(rb.linearVelocityX)) / maxValue;

                var pitch = Mathf.Lerp(minSpeed, maxSpeed, value);
                audioSourceEngine.pitch = pitch;
            }
        }
    }
}