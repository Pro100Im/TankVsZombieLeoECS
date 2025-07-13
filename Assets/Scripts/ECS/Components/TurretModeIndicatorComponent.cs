using System;
using UnityEngine.UI;

namespace ECS.Components
{
    [Serializable]
    public struct TurretModeIndicatorComponent
    {
        public Toggle MiniGunToggle;
        public Toggle BigGunToggle;
    }
}
