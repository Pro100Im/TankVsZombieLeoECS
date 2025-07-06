using System;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerInputComponent
    {
        public bool SwapGunRequested;

        public Vector2 DirectionInput;
        public Vector2 AimingInput;
    }
}
