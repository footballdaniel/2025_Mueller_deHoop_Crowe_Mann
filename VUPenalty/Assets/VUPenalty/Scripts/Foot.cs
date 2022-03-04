using UnityEngine;

namespace VUPenalty
{
    public class Foot : MonoBehaviour
    {
        public void AttachTo(Transform rootObject)
        {
            transform.SetParent(rootObject, false);
        }


    }
}
