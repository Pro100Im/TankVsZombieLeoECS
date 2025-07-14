using ECS.Components;
using Leopotam.EcsLite;
using System;

namespace ECS.Systems
{
    public class HpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<HpComponent> _hpPool;
        private EcsPool<TakeDamageEvent> _takeDamagePool;
        private EcsPool<ChangeHpTag> _changeHpPool;

        private EcsFilter _hpFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();
            _takeDamagePool = _world.GetPool<TakeDamageEvent>();
            _changeHpPool = _world.GetPool<ChangeHpTag>();

            _hpFilter = _world.Filter<HpComponent>().Inc<TakeDamageEvent>().Exc<InPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_hpFilter.GetEntitiesCount() <= 0)
                return;

            foreach(var entity in _hpFilter)
            {
                var damage = _takeDamagePool.Get(entity);
                ref var health = ref _hpPool.Get(entity);
                var currentHp = health.CurrentHp;
                var maxHp = health.MaxHp;

                currentHp -= damage.Damage;
                health.CurrentHp = Math.Clamp(currentHp, 0, maxHp);

                _takeDamagePool.Del(entity);

                if(!_changeHpPool.Has(entity))
                    _changeHpPool.Add(entity);
            }
        }
    }
}
