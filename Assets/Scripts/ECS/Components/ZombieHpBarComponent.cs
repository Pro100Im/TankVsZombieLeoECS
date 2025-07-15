using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct ZombieHpBarComponent
    {
        public Transform BarTransform;
        public Transform ForegroundBar;
        public Transform ShadowBar;

        public float ShadowSpeed;
    }
}
