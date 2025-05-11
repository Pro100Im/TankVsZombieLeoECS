using ECS.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tank
{
	public sealed class TurretModeIndicator : MonoBehaviour
	{
		[SerializeField] Toggle toggleMiniGun;
		[SerializeField] Toggle toggleBigGun;

		private void Awake( )
		{
			UIGameActions.OnTurretSwitched += OnTurretModeChanged;
		}

		public void OnTurretModeChanged( bool isBigGun )
		{
			if( isBigGun )
				toggleBigGun.isOn = true;
			else
				toggleMiniGun.isOn = true;
		}

		private void OnDestroy( )
		{
			UIGameActions.OnTurretSwitched -= OnTurretModeChanged;
		}
	}
}