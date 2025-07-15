using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct CircleColliderComponent
    {
        public CircleCollider2D Collider;
    }
}
