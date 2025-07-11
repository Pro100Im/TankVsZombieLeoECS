using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class ReturnToPoolSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsFilterInject<Inc<BulletRefsComponent, ReturnToPoolTag>, Exc<InPoolTag>> _bulletReturnToPoolComponents = default;

        public void Run(IEcsSystems systems)
        {
            var returnToPoolFilter = _bulletReturnToPoolComponents.Value;
            var bulletRefPool = _bulletReturnToPoolComponents.Pools.Inc1;
            var returnToPool = _bulletReturnToPoolComponents.Pools.Inc2;

            foreach(var i in returnToPoolFilter)
            {
                var returnToPoolEntity = returnToPoolFilter.GetRawEntities()[i];

                if(!returnToPool.Has(returnToPoolEntity))
                    continue;

                ref var bulletRefComponent = ref bulletRefPool.Get(returnToPoolEntity);
                var returnToPoolComponent = returnToPool.Get(returnToPoolEntity);
                var pool = _defaultWorld.Value.GetPool<ReturnToPoolTag>();

                bulletRefComponent.GameObject.SetActive(false);
                bulletRefComponent.Rb.linearVelocity = Vector2.zero;

                if(pool.Has(returnToPoolEntity))
                    pool.Del(returnToPoolEntity);
            }
        }
    }
}
