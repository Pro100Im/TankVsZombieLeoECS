using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class ZombieFollowSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<EnemyRefsComponent> _refPool;
        private EcsPool<TankMovementComponent> _targetPool;

        private EcsFilter _bigZombieFilter;
        private EcsFilter _smallZombieFilter;
        private EcsFilter _tankMovementFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _refPool = _world.GetPool<EnemyRefsComponent>();
            _targetPool = _world.GetPool<TankMovementComponent>();

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

            Follow(targetPos, _bigZombieFilter);
            Follow(targetPos, _smallZombieFilter);
        }

        private void Follow(Vector3 targetPos, EcsFilter filter)
        {
            foreach(var entity in filter)
            {
                var zombieRefs = _refPool.Get(entity);
                var zombieRb = zombieRefs.Rb;
                var zombiePos = zombieRb.transform.position;

                Vector2 direction = (targetPos - zombiePos).normalized;
                Vector2[] directions =
                    {
                direction,
                Quaternion.Euler(0, 0, 30) * direction,
                Quaternion.Euler(0, 0, -30) * direction,
                Quaternion.Euler(0, 0, 60) * direction,
                Quaternion.Euler(0, 0, -60) * direction
                };

                foreach(var dir in directions)
                {
                    var hit = Physics2D.Raycast(zombiePos, dir, zombieRefs.AvoidDistance, zombieRefs.ObstacleLayer);

                    if(hit.collider == null)
                    {
                        direction = dir;
                        break;
                    }
                    else
                        direction = Vector2.Perpendicular(hit.normal).normalized;
                }

                var moveSpeed = zombieRefs.MoveSpeed;
                var rotationSpeed = zombieRefs.RotationSpeed;
                var forceMultiply = zombieRefs.ForceMultiply;
                var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var angle = Mathf.LerpAngle(zombieRb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);

                zombieRb.MoveRotation(angle);
                zombieRb.AddForce(direction * moveSpeed * forceMultiply, ForceMode2D.Force);

                zombieRb.linearVelocityY = Mathf.Clamp(zombieRb.linearVelocity.y, -moveSpeed, moveSpeed);
            }
        }
    }
}
