using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly TankInput _tankInput;

        private EcsWorld _world;

        private EcsPool<PlayerInputComponent> _inputPool;

        private EcsFilter _inputFilter;

        public PlayerInputSystem(TankInput tankInput)
        {
            _tankInput = tankInput;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _inputPool = _world.GetPool<PlayerInputComponent>();
            _inputFilter = _world.Filter<PlayerInputComponent>().End();

            _tankInput.Enable();
        }

        public void Run(IEcsSystems systems)
        {
            if(_inputFilter.GetEntitiesCount() == 0)
                return;

            var map = _tankInput.ActionMap;
            var entity = _inputFilter.GetRawEntities()[0];
            ref var input = ref _inputPool.Get(entity);

            input.SwapGunRequested = map.SwapGun.WasPressedThisFrame();
            input.FireRequested = map.Fire.WasPressedThisFrame();
            input.AimingInput = map.Point.ReadValue<Vector2>();
            input.DirectionInput = map.Move.ReadValue<Vector2>().normalized;
        }

        public void Destroy(IEcsSystems systems)
        {
            _tankInput.Disable();
        }
    }
}
