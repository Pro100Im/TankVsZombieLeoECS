using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class BulletCollisionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<InPoolTag> _inPoolTagPool;
        private EcsPool<BulletRefsComponent> _bulletRefsPool;
        private EcsPool<DamageComponent> _damagePool;
        private EcsPool<TakeDamageEvent> _takeDamagePool;
        private EcsPool<ReturnToPoolTag> _returnToPoolTagPool;
        private EcsPool<CircleColliderComponent> _circleColliderPool;

        private EcsFilter _colliderFilter;
        private EcsFilter _takeDamageFilter;

        private const float _offset = 0.3f;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _inPoolTagPool = _world.GetPool<InPoolTag>();
            _bulletRefsPool = _world.GetPool<BulletRefsComponent>();
            _damagePool = _world.GetPool<DamageComponent>();
            _takeDamagePool = _world.GetPool<TakeDamageEvent>();
            _returnToPoolTagPool = _world.GetPool<ReturnToPoolTag>();
            _circleColliderPool = _world.GetPool<CircleColliderComponent>();

            _colliderFilter = _world.Filter<CircleColliderComponent>().Exc<InPoolTag>().Exc<ReturnToPoolTag>().End();
            _takeDamageFilter = _world.Filter<TakeDamageEvent>().Inc<InPoolTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var entityA in _colliderFilter)
            {
                if(!_bulletRefsPool.Has(entityA))
                    continue;

                ref var colliderAComp = ref _circleColliderPool.Get(entityA);
                var colliderA = colliderAComp.Collider;

                if(colliderA == null)
                    continue;

                var centerA = colliderA.bounds.center;
                var radiusA = colliderA.radius * colliderA.transform.lossyScale.x;

                foreach(var entityB in _colliderFilter)
                {
                    if(_bulletRefsPool.Has(entityB))
                        continue;

                    ref var colliderBComp = ref _circleColliderPool.Get(entityB);
                    var colliderB = colliderBComp.Collider;

                    if(colliderB == null)
                        continue;

                    var centerB = colliderB.bounds.center;
                    var radiusB = colliderB.radius * colliderB.transform.lossyScale.x;

                    var distance = Vector2.Distance(centerA, centerB);

                    if(distance <= radiusA + radiusB + _offset)
                    {
                        if(!_returnToPoolTagPool.Has(entityA))
                            _returnToPoolTagPool.Add(entityA);

                        if(_takeDamageFilter.GetEntitiesCount() > 0)
                        {
                            var damageComponent = _damagePool.Get(entityA);
                            var takeDamageEventEntity = _takeDamageFilter.GetRawEntities()[0];
                            ref var takeDamageEvent = ref _takeDamagePool.Get(takeDamageEventEntity);

                            takeDamageEvent.TargetEntity = entityB;
                            takeDamageEvent.Damage = damageComponent.Damage;

                            _inPoolTagPool.Del(takeDamageEventEntity);
                        }
                    }
                }
            }
        }
    }
}
