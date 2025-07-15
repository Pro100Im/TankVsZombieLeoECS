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
        private EcsPool<ZombieHpBarComponent> _zombieHpBarPool;

        private EcsFilter _playerHpFilter;
        private EcsFilter _playerHpBarFilter;
        private EcsFilter _enemyHpFilter;
        private EcsFilter _enemyHpBarFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();
            _changeHpPool = _world.GetPool<ChangeHpTag>();
            _tankHpBarPool = _world.GetPool<TankHpBarComponent>();
            _zombieHpBarPool = _world.GetPool<ZombieHpBarComponent>();

            _playerHpFilter = _world.Filter<HpComponent>().Inc<ChangeHpTag>().Inc<TankMovementComponent>().Exc<InPoolTag>().End();
            _playerHpBarFilter = _world.Filter<TankHpBarComponent>().End();
            _enemyHpFilter = _world.Filter<HpComponent>().Inc<ChangeHpTag>().Inc<EnemyRefsComponent>().Exc<InPoolTag>().End();
            _enemyHpBarFilter = _world.Filter<ZombieHpBarComponent>().Exc<InPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            TankHpBarChange();
            ZombieHpBarChange();
        }

        private void TankHpBarChange()
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

        private void ZombieHpBarChange()
        {
            foreach(var entity in _enemyHpBarFilter)
            {
                ref var bar = ref _zombieHpBarPool.Get(entity);

                bar.BarTransform.rotation = Quaternion.LookRotation(bar.BarTransform.position - Camera.main.transform.position);
            }

            foreach(var entity in _enemyHpFilter)
            {
                var hp = _hpPool.Get(entity);
                var targetValue = (float)hp.CurrentHp / hp.MaxHp;

                ref var bar = ref _zombieHpBarPool.Get(entity);       

                var foregroundScale = bar.ForegroundBar.localScale;
                bar.ForegroundBar.localScale = new Vector3(targetValue, foregroundScale.y, foregroundScale.z);

                var shadowScale = bar.ShadowBar.localScale;

                if(shadowScale.x == targetValue)
                {
                    _changeHpPool.Del(entity);

                    return;
                }

                var newShadowX = Mathf.Lerp(shadowScale.x, targetValue, Time.deltaTime * bar.ShadowSpeed);

                bar.ShadowBar.localScale = new Vector3(newShadowX, shadowScale.y, shadowScale.z);
            }
        }
    }
}
