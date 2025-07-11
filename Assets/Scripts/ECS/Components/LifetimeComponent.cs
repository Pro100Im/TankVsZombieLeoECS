using System;

namespace ECS.Components
{
    [Serializable]
    public struct LifeTimeComponent
    {
        public float Time;
        public float CurrentTime;
    }
}
