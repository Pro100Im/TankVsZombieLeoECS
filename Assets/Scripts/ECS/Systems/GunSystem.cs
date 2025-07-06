using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace ECS.Systems
{
    public class GunSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;
        private readonly EcsFilterInject<Inc<TurretComponent, BigGunComponent, MiniGunComponent>> _gunComponents = default;

        public void Run(IEcsSystems systems)
        {
            var entity = _gunComponents.Value.GetRawEntities()[0];
            var playerInputComponent = _playerInputComponents.Pools.Inc1.Get(entity);

            if(!playerInputComponent.FireRequested)
                return;

            var turretComponent = _gunComponents.Pools.Inc1.Get(entity);

            if(turretComponent.IsBigGun)
            {
                var bigGunComponent = _gunComponents.Pools.Inc2.Get(entity);

                bigGunComponent.FireEffect.Play();
            }
            else
            {
                var miniGunComponent = _gunComponents.Pools.Inc3.Get(entity);

                miniGunComponent.FireEffect.Play();
            }
        }
    }
}