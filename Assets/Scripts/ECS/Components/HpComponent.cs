using System;

namespace ECS.Components
{
    [Serializable]
    public struct HpComponent
    {
        public int MaxHp;
        public int CurrentHp;
    }
}
