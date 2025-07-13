using System;
using UnityEngine;

namespace ECS.Components
{
	[Serializable]
	public struct BigGunComponent
	{
        public GameObject BulletPrefab;
        public int BulletPoolSize;

        public float ReloadDuration;
		public float HeatingDuration;
		public float RecoilPower;

		public SpriteRenderer Gun;
		public Color          HotColor;
		
		public Transform FirePoint;
        public ParticleSystem FireEffect;
	}
}