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
        private EcsPool<ReturnToPoolTag> _returnToPoolTagPool;

        private EcsFilter _takeDamageEventFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();
            _takeDamagePool = _world.GetPool<TakeDamageEvent>();
            _changeHpPool = _world.GetPool<ChangeHpTag>();
            _returnToPoolTagPool = _world.GetPool<ReturnToPoolTag>();

            _takeDamageEventFilter = _world.Filter<TakeDamageEvent>().Exc<InPoolTag>().Exc<ReturnToPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_takeDamageEventFilter.GetEntitiesCount() <= 0)
                return;

            foreach(var entity in _takeDamageEventFilter)
            {
                var takeDamageEvent = _takeDamagePool.Get(entity);
                var targetEntity = takeDamageEvent.TargetEntity;
                var damage = takeDamageEvent.Damage;

                ref var health = ref _hpPool.Get(targetEntity);
                var currentHp = health.CurrentHp;
                var maxHp = health.MaxHp;

                currentHp -= damage;
                health.CurrentHp = Math.Clamp(currentHp, 0, maxHp);

                _returnToPoolTagPool.Add(entity);

                if(!_changeHpPool.Has(targetEntity))
                    _changeHpPool.Add(targetEntity);
            }
        }
    }
}
