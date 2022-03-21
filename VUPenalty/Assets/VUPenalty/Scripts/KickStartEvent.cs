using System;

namespace VUPenalty
{
    [Serializable]
    public class KickStartEvent
    {
        public Point3D Origin;
        public Point3D VelocityVector;
    }
}