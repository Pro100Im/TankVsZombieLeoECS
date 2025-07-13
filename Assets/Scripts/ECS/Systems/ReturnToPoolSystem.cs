using ECS.Components;
using Leopotam.EcsLite;

namespace ECS.Systems
{
    public class ReturnToPoolSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter _bulletFilter;

        private EcsPool<InPoolTag> _inPool;
        private EcsPool<ReturnToPoolTag> _retPool;
        private EcsPool<LifeTimeComponent> _ltPool;
        private EcsPool<BulletRefsComponent> _refPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inPool = _world.GetPool<InPoolTag>();
            _retPool = _world.GetPool<ReturnToPoolTag>();
            _ltPool = _world.GetPool<LifeTimeComponent>();
            _refPool = _world.GetPool<BulletRefsComponent>();

            _bulletFilter = _world.Filter<ReturnToPoolTag>().Inc<BulletRefsComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            ReturnToBulletPool();
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
    }
}
