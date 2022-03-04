using UnityEngine;
using VU.Scripts;

namespace VUPenalty
{
    public abstract class User : MonoBehaviour
    {
        public abstract GazeInformation Gaze { get; }
        public abstract HeadInformation Head { get; }

        public abstract Transform FootRootElement { get; }
        public abstract void Use(Foot foot);
    }
}