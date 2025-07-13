using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct TankMovementComponent
    {
        public float MaxSpeed;
        public float RotationSpeed;
        public float EnginePower;

        public Rigidbody2D Rb;
    }
}