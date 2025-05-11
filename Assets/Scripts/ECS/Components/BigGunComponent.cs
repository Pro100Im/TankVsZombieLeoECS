using UnityEngine;

namespace ECS.Components
{
	public struct BigGunComponent
	{
		public float ReloadDuration;
		public float HeatingDuration;
		public float RecoilPower;

		public SpriteRenderer Gun;
		public Color          HotColor;
		public Rigidbody2D    Rb;
		
		public Transform firePoint;
        //public BulletPool bulletPool;
        public ParticleSystem fireEffect;
        //public TankAudio tankAudio;
	}
}