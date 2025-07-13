using System;
using UnityEngine.UI;

namespace ECS.Components
{
    [Serializable]
    public struct TankHpBarComponent
    {
        public Slider ForegroundBar;
        public Slider ShadowBar;

        public float ShadowSpeed;
    }
}
