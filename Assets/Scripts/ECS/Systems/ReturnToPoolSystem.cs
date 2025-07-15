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
        private EcsPool<BulletRefsComponent> _refPool;
        private EcsPool<TakeDamageEvent> _takeDamageEventPool;

        private EcsFilter _bulletFilter;
        private EcsFilter _takeDamageFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inPool = _world.GetPool<InPoolTag>();
            _retPool = _world.GetPool<ReturnToPoolTag>();
            _ltPool = _world.GetPool<LifeTimeComponent>();
            _refPool = _world.GetPool<BulletRefsComponent>();
            _takeDamageEventPool = _world.GetPool<TakeDamageEvent>();

            _bulletFilter = _world.Filter<ReturnToPoolTag>().Inc<BulletRefsComponent>().End();
            _takeDamageFilter = _world.Filter<ReturnToPoolTag>().Inc<TakeDamageEvent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            ReturnToBulletPool();
            ReturnToTakeDamageEventPool();
        }

        private void ReturnToBulletPool()
        {
            foreach(var entity in _bulletFilter)
            {
                ref var bulletRef = ref _refPool.Get(entity);
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
    }
}
