using System;
using UnityEngine;

namespace ECS.Components
{
	[Serializable]
	public struct TankAudioComponent
	{
		public float       MinSpeed;
		public float       MaxSpeed;
		public AudioClip   BigGunClip;
		public AudioClip   MiniGunClip;
		public AudioSource AudioSourceEngine;
		public AudioSource AudioSourceGun;
	}
}