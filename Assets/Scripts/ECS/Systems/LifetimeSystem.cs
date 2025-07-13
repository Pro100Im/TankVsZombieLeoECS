using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class LifetimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter _filter;

        private EcsPool<LifeTimeComponent> _ltPool;
        private EcsPool<ReturnToPoolTag> _retPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _ltPool = _world.GetPool<LifeTimeComponent>();
            _retPool = _world.GetPool<ReturnToPoolTag>();

            _filter = _world.Filter<LifeTimeComponent>().Exc<InPoolTag>().Exc<ReturnToPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter)
            {
                ref var lt = ref _ltPool.Get(entity);
                lt.CurrentTime -= Time.deltaTime;

                if(lt.CurrentTime <= 0f)
                    _retPool.Add(entity);
            }
        }
    }
}
