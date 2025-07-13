using ECS.Components;
using Leopotam.EcsLite;

namespace ECS.Systems
{
    public class HpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<HpComponent> _hpPool;
        private EcsPool<TakeDamageTag> _takeDamagePool;

        private EcsFilter _hpFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();

            _hpFilter = _world.Filter<HpComponent>().Inc<TakeDamageTag>().Exc<InPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_hpFilter.GetEntitiesCount() <= 0)
                return;

            foreach(var entity in _hpFilter)
            {

            }
        }
    }
}
