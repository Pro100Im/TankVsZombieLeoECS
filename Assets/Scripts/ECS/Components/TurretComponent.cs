using System;
using UnityEngine;

namespace ECS.Components
{
	[Serializable]
	public struct TurretComponent
	{
		public Transform Transform;

		public float RotationSpeed;
		public float BigGunLaserDistance;
		public float MiniGunLaserDistance;

		public bool IsBigGun;
		
		public LayerMask    LaserMask;
		public LineRenderer LineRenderer;
		
		public GameObject MiniGun;
		public GameObject BigGun;

		[NonSerialized] public Vector3 Target;
		[NonSerialized] public Camera Camera;
	}
}