using ECS.Components;
using Leopotam.EcsLite;

namespace ECS.Systems
{
    public class ReturnToPoolSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<InPoolTag> _inPool;
        private EcsPool<ReturnToPoolTag> _retPool;
        private EcsPool<LifeTimeComponent> _ltPool;
        private EcsPool<BulletRefsComponent> _bulletRefPool;
        private EcsPool<EnemyRefsComponent> _enemyRefPool;
        private EcsPool<TakeDamageEvent> _takeDamageEventPool;

        private EcsFilter _bulletFilter;
        private EcsFilter _takeDamageFilter;
        private EcsFilter _zombieFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inPool = _world.GetPool<InPoolTag>();
            _retPool = _world.GetPool<ReturnToPoolTag>();
            _ltPool = _world.GetPool<LifeTimeComponent>();
            _bulletRefPool = _world.GetPool<BulletRefsComponent>();
            _enemyRefPool = _world.GetPool<EnemyRefsComponent>();
            _takeDamageEventPool = _world.GetPool<TakeDamageEvent>();

            _bulletFilter = _world.Filter<ReturnToPoolTag>().Inc<BulletRefsComponent>().End();
            _takeDamageFilter = _world.Filter<ReturnToPoolTag>().Inc<TakeDamageEvent>().End();
            _zombieFilter = _world.Filter<ReturnToPoolTag>().Inc<EnemyRefsComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            ReturnToBulletPool();
            ReturnToTakeDamageEventPool();
            ReturtToZombiePool();
        }

        private void ReturnToBulletPool()
        {
            foreach(var entity in _bulletFilter)
            {
                ref var bulletRef = ref _bulletRefPool.Get(entity);
                bulletRef.GameObject.SetActive(false);

                ref var lt = ref _ltPool.Get(entity);
                lt.CurrentTime = lt.Time;

                _inPool.Add(entity);
                _retPool.Del(entity);
            }
        }

        private void ReturnToTakeDamageEventPool()
        {
            foreach(var entity in _takeDamageFilter)
            {
                ref var tde = ref _takeDamageEventPool.Get(entity);
                tde.Damage = 0;
                tde.TargetEntity = 0;

                _inPool.Add(entity);
                _retPool.Del(entity);
            }
        }

        private void ReturtToZombiePool()
        {
            foreach(var entity in _zombieFilter)
            {
                ref var enemyRef = ref _enemyRefPool.Get(entity);
                enemyRef.GameObject.SetActive(false);

                _inPool.Add(entity);
                _retPool.Del(entity);
            }
        }
    }
}
