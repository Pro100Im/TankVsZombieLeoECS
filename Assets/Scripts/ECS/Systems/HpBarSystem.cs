using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class HpBarSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<HpComponent> _hpPool;
        private EcsPool<ChangeHpTag> _changeHpPool;
        private EcsPool<TankHpBarComponent> _tankHpBarPool;

        private EcsFilter _playerHpFilter;
        private EcsFilter _playerHpBarFilter;
        private EcsFilter _enemyHpFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();
            _changeHpPool = _world.GetPool<ChangeHpTag>();
            _tankHpBarPool = _world.GetPool<TankHpBarComponent>();

            _playerHpFilter = _world.Filter<HpComponent>().Inc<ChangeHpTag>().Inc<TankMovementComponent>().Exc<InPoolTag>().End();
            _playerHpBarFilter = _world.Filter<TankHpBarComponent>().End();
            _enemyHpFilter = _world.Filter<HpComponent>().Inc<ChangeHpTag>().Inc<EnemyRefsComponent>().Exc<InPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_playerHpFilter.GetEntitiesCount() <= 0 || _playerHpBarFilter.GetEntitiesCount() <= 0)
                return;

            var playerEntity = _playerHpFilter.GetRawEntities()[0];
            var playerHpBarEntity = _playerHpBarFilter.GetRawEntities()[0];
            var hp = _hpPool.Get(playerEntity);

            ref var bar = ref _tankHpBarPool.Get(playerHpBarEntity);

            var targetValue = (float)hp.CurrentHp / hp.MaxHp;

            bar.ForegroundBar.value = targetValue;

            if(bar.ShadowBar.value == targetValue)
            {
                _changeHpPool.Del(playerEntity);

                return;
            }

            var currentShadowValue = bar.ShadowBar.value;
            var lerped = Mathf.Lerp(currentShadowValue, targetValue, Time.deltaTime * bar.ShadowSpeed);

            bar.ShadowBar.value = lerped;
        }
    }
}
