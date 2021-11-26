using UnityEngine;

namespace VU.Scripts
{
    public class Foot : MonoBehaviour
    {
        public void AttachTo(Transform rootObject)
        {
            transform.SetParent(rootObject);
        }
    }
}
