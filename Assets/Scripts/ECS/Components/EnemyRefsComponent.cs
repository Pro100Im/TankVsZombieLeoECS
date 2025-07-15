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
        public LayerMask ObstacleLayer;

        public string HitAnimTrigger;

        public float MoveSpeed;
        public float RotationSpeed;
        public float AvoidDistance;
        public float ForceMultiply;
        public float AttackRange;
        public float TimeBetweenAttack;
        [HideInInspector]
        public float LastAttackTime;
    }
}
