using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class LifetimeSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsFilterInject<Inc<LifeTimeComponent>, Exc<InPoolTag, ReturnToPoolTag>> _lifeTimeComponents = default;
        private readonly EcsFilterInject<Inc<LifeTimeComponent, ReturnToPoolTag>, Exc<InPoolTag>> _noLifeTimeComponents = default;

        public void Run(IEcsSystems systems)
        {
            var lifeTimeFilter = _lifeTimeComponents.Value;
            var lifeTimePool = _lifeTimeComponents.Pools.Inc1;

            foreach(var i in lifeTimeFilter)
            {
                var entity = lifeTimeFilter.GetRawEntities()[i];

                if(!lifeTimePool.Has(entity))
                    continue;

                ref var lifetimeComponent = ref lifeTimePool.Get(entity);

                lifetimeComponent.CurrentTime -= Time.deltaTime;

                if(lifetimeComponent.CurrentTime <= 0)
                {
                    var pool = _defaultWorld.Value.GetPool<ReturnToPoolTag>();

                    if(!pool.Has(entity))
                    {
                        pool.Add(entity);
                        Debug.Log($"ReturnToPoolTag добавлен на пулю: {entity}");
                    }
                }

                Debug.LogWarning($"lifetimeComponent.CurrentTime {lifetimeComponent.CurrentTime}");
            }

            var noLifeTimeFilter = _noLifeTimeComponents.Value;
            var noLifeTimePool = _noLifeTimeComponents.Pools.Inc1;

            foreach(var i in noLifeTimeFilter)
            {
                var entity = noLifeTimeFilter.GetRawEntities()[i];

                if(!noLifeTimePool.Has(entity))
                    continue;

                ref var lifetimeComponent = ref noLifeTimePool.Get(entity);

                lifetimeComponent.CurrentTime = lifetimeComponent.Time;
            }
        }
    }
}
