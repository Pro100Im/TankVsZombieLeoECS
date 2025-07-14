using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class ZombieAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<EnemyRefsComponent> _refPool;
        private EcsPool<DamageComponent> _attackPool;
        private EcsPool<TankMovementComponent> _targetPool;
        private EcsPool<TakeDamageEvent> _takeDamagePool;

        private EcsFilter _bigZombieFilter;
        private EcsFilter _smallZombieFilter;
        private EcsFilter _tankMovementFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _refPool = _world.GetPool<EnemyRefsComponent>();
            _targetPool = _world.GetPool<TankMovementComponent>();
            _takeDamagePool = _world.GetPool<TakeDamageEvent>();
            _attackPool = _world.GetPool<DamageComponent>();

            _bigZombieFilter = _world.Filter<EnemyRefsComponent>().Inc<BigEnemyTag>().Exc<InPoolTag>().End();
            _smallZombieFilter = _world.Filter<EnemyRefsComponent>().Exc<BigEnemyTag>().Exc<InPoolTag>().End();
            _tankMovementFilter = _world.Filter<TankMovementComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_tankMovementFilter.GetEntitiesCount() <= 0)
                return;

            var targetEntity = _tankMovementFilter.GetRawEntities()[0];
            var targetComponent = _targetPool.Get(targetEntity);
            var targetPos = targetComponent.Rb.transform.position;

            CheckForAttack(targetPos, targetEntity, _bigZombieFilter);
            CheckForAttack(targetPos, targetEntity, _smallZombieFilter);
        }

        private void CheckForAttack(Vector3 targetPos, int targetEntity, EcsFilter filter)
        {
            foreach(var entity in filter)
            {
                var currentTime = Time.time;
                ref var zombieRefs = ref _refPool.Get(entity);
                var lastAttackTime = zombieRefs.LastAttackTime;
                var timeBetweenAttack = zombieRefs.TimeBetweenAttack;

                if(currentTime - lastAttackTime < timeBetweenAttack)
                    continue;

                var zombieRb = zombieRefs.Rb;
                var zombiePos = zombieRb.transform.position;
                var attackRange = zombieRefs.AttackRange;

                var distance = Vector3.Distance(zombiePos, targetPos);

                if(distance > attackRange)
                    continue;

                zombieRefs.LastAttackTime = Time.time;

                var damage = _attackPool.Get(entity).Damage;

                if(!_takeDamagePool.Has(targetEntity))
                    _takeDamagePool.Add(targetEntity) = new TakeDamageEvent { Damage = damage };
                //else
                //    _takeDamagePool.Get(entity).Damage += damage;
            }
        }
    }
}
