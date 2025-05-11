using UnityEngine;

namespace Game.Tank
{
    public interface ITurretModeObservable 
    {
        public void AddTurretModeObserver(ITurretModeObserver observer);

        public void RemoveTurretModeObserver(ITurretModeObserver observer);
    }
}