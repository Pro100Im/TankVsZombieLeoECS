using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            
        }
    }
}
