using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct EnemyRefsComponent
    {
        public GameObject GameObject;
        public Rigidbody2D Rb;
        public ParticleSystem DieEffect;
        public Animator Animator;
    }
}
