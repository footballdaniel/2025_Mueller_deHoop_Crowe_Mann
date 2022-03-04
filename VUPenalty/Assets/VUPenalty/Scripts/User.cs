using UnityEngine;
using VU.Scripts;

namespace VUPenalty
{
    public abstract class User : MonoBehaviour
    {
        public abstract Transform FootRootElement { get; }
        
        public abstract Transform Head { get; }
        
        public abstract void Use(Foot foot);

        public void Calibrate(Foot foot)
        {
            var offset = transform.rotation;
            foot.transform.rotation *= Quaternion.Inverse(offset);
        }
    }
}