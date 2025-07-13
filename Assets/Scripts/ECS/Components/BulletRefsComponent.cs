using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct BulletRefsComponent
    {
        public GameObject GameObject;
        public Rigidbody2D Rb;
    }
}
