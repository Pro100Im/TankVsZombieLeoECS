using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;
        private readonly EcsCustomInject<TankInput> _tankInput;

        public void Init(IEcsSystems systems)
        {
            _tankInput.Value.Enable();
        }

        public void Run(IEcsSystems systems)
        {
            if(_playerInputComponents.Value.GetEntitiesCount() == 0)
                return;

            var entity = _playerInputComponents.Value.GetRawEntities()[0];

            ref var playerInputComponent = ref _playerInputComponents.Pools.Inc1.Get(entity);

            playerInputComponent.SwapGunRequested = _tankInput.Value.ActionMap.SwapGun.WasPressedThisFrame();
            playerInputComponent.AimingInput = _tankInput.Value.ActionMap.Point.ReadValue<Vector2>();
            playerInputComponent.DirectionInput = _tankInput.Value.ActionMap.Move.ReadValue<Vector2>().normalized;
        }

        public void Destroy(IEcsSystems systems)
        {
            _tankInput.Value.Disable();
        }
    }
}
