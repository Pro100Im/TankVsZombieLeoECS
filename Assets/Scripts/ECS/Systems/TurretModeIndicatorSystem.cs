using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class TurretModeIndicatorSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TurretModeIndicatorComponent>> _turretModeIndicatorComponents = default;
        private readonly EcsFilterInject<Inc<TurretComponent>> _turretComponents = default;

        public void Run(IEcsSystems systems)
        {
            if(_turretModeIndicatorComponents.Value.GetEntitiesCount() > 0 && _turretComponents.Value.GetEntitiesCount() > 0)
            {
                var turretEntity = _turretComponents.Value.GetRawEntities()[0];
                ref var turretComponent = ref _turretComponents.Pools.Inc1.Get(turretEntity);

                var indicatorEntity = _turretModeIndicatorComponents.Value.GetRawEntities()[0];
                ref var indicatorComponent = ref _turretModeIndicatorComponents.Pools.Inc1.Get(indicatorEntity);

                indicatorComponent.MiniGunToggle.isOn = !turretComponent.IsBigGun;
                indicatorComponent.BigGunToggle.isOn = turretComponent.IsBigGun;
            }
        }
    }
}
