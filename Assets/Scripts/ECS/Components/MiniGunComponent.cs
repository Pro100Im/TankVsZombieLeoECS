using System;
using UnityEngine;

namespace ECS.Components
{
    [Serializable]
    public struct MiniGunComponent
	{
		public Transform FirePoint;
        public ParticleSystem FireEffect;
        public GameObject BulletPrefab;
        public int BulletPoolSize;
	}
}
