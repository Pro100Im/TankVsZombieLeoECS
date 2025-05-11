using UnityEngine;
using UnityEngine.UI;

namespace Game.Tank
{
    public sealed class TurretModeIndicator : MonoBehaviour, ITurretModeObserver
    {
        [SerializeField] Toggle toggleMiniGun;
        [SerializeField] Toggle toggleBigGun;

        public void OnTurretModeChanged(BaseGun currentGun)
        {
            if(currentGun is BigGun)
                toggleBigGun.isOn = true;
            else
                toggleMiniGun.isOn = true;
        }
    }
}