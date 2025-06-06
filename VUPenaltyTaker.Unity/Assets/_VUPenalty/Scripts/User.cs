using UnityEngine;

namespace VUPenalty
{
    public abstract class User : MonoBehaviour
    {
        public abstract Transform FootRootElement { get; }
        
        public abstract Transform Head { get; }
        
        public abstract void Use(Foot foot);

        public void Calibrate(Foot foot)
        {
            var offset = FootRootElement.transform.rotation;
            foot.RotateModel(offset);
        }
    }
}