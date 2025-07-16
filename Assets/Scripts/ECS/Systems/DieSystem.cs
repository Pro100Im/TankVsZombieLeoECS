using ECS.Components;
using Leopotam.EcsLite;

namespace ECS.Systems
{
    public class DieSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<HpComponent> _hpPool;
        private EcsPool<EnemyRefsComponent> _refPool;
        private EcsPool<EndGameComponent> _endGamePool;
        private EcsPool<TankMovementComponent> _playerPool;
        private EcsPool<ReturnToPoolTag> _returnToPoolTagPool;

        private EcsFilter _zombieFilter;
        private EcsFilter _endGameFilter;
        private EcsFilter _playerFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _hpPool = _world.GetPool<HpComponent>();
            _refPool = _world.GetPool<EnemyRefsComponent>();
            _playerPool = _world.GetPool<TankMovementComponent>();
            _endGamePool = _world.GetPool<EndGameComponent>();
            _returnToPoolTagPool = _world.GetPool<ReturnToPoolTag>();

            _zombieFilter = _world.Filter<EnemyRefsComponent>().Inc<HpComponent>().Exc<InPoolTag>().End();
            _endGameFilter = _world.Filter<EndGameComponent>().End();
            _playerFilter = _world.Filter<TankMovementComponent>().Inc<HpComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            CheckZombie();
        }

        private void CheckZombie()
        {
            foreach(var entity in _zombieFilter)
            {
                var hp = _hpPool.Get(entity);

                if(hp.CurrentHp > 0)
                    continue;

                var zombie = _refPool.Get(entity);

                zombie.DieEffect.Play();

                _returnToPoolTagPool.Add(entity);
            }

            if(_playerFilter.GetEntitiesCount() <= 0)
                return;

            var playerEntity = _playerFilter.GetRawEntities()[0];
            var playerHp = _hpPool.Get(playerEntity);
            ref var player = ref _playerPool.Get(playerEntity);

            if(playerHp.CurrentHp > 0 || player.MaxSpeed == 0)
                return;

            player.MaxSpeed = 0;

            var endGameEntity = _endGameFilter.GetRawEntities()[0];
            var endGame = _endGamePool.Get(endGameEntity);

            endGame.EndGameMenu.SetActive(true);
        }
    }
}
