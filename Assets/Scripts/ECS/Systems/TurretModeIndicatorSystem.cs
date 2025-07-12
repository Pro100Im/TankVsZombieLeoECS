using ECS.Components;
using Leopotam.EcsLite;

namespace ECS.Systems
{
    public class TurretModeIndicatorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<TurretModeIndicatorComponent> _indicatorPool;
        private EcsPool<TurretComponent> _turretPool;

        private EcsFilter _indicatorFilter;
        private EcsFilter _turretFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _indicatorPool = _world.GetPool<TurretModeIndicatorComponent>();
            _turretPool = _world.GetPool<TurretComponent>();

            _indicatorFilter = _world.Filter<TurretModeIndicatorComponent>().End();
            _turretFilter = _world.Filter<TurretComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_indicatorFilter.GetEntitiesCount() == 0 || _turretFilter.GetEntitiesCount() == 0)
                return;

            var turretEntity = _turretFilter.GetRawEntities()[0];
            var indicatorEntity = _indicatorFilter.GetRawEntities()[0];

            ref var turret = ref _turretPool.Get(turretEntity);
            ref var indicator = ref _indicatorPool.Get(indicatorEntity);

            indicator.MiniGunToggle.isOn = !turret.IsBigGun;
            indicator.BigGunToggle.isOn = turret.IsBigGun;
        }
    }
}
