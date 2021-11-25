using UnityEngine;

namespace VU.Scripts
{
    public abstract class User : MonoBehaviour
    {
        public abstract GazeInformation Gaze { get; }
        public abstract HeadInformation Head { get; }

        public abstract Transform FootRootElement { get; }
        public abstract void Visit(Foot foot);
    }
}