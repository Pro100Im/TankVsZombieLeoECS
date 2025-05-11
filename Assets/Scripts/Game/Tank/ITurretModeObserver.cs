using UnityEngine;

namespace Game.Tank
{
    public interface ITurretModeObserver
    {
        public void OnTurretModeChanged(BaseGun currentGun);
    }
}