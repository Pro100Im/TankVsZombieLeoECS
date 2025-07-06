using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace ECS.Systems
{
    public class GunSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;
        private readonly EcsFilterInject<Inc<BigGunComponent, MiniGunComponent, TurretComponent>> _gunComponents = default;

        public void Run(IEcsSystems systems)
        {
            var entity = _gunComponents.Value.GetRawEntities()[0];
            var playerInputComponents = _playerInputComponents.Pools.Inc1.Get(entity);
            //ref var turretComponent = ref _turretComponents.Pools.Inc1.Get(entity);
        }

        private void FireInput(InputAction.CallbackContext context)
        {
            //var turret = _gunComponents.Pools.Inc3.Get(_entity);

            //if(turret.IsBigGun)
            //{
            //    var bigGun = _gunComponents.Pools.Inc1.Get(_entity);
            //}
            //else
            //{
            //    var miniGun = _gunComponents.Pools.Inc2.Get(_entity);
            //}
        }
    }
}