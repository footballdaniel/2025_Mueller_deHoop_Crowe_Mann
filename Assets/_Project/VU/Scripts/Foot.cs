using UnityEngine;

namespace VU.Scripts
{
    public class Foot : MonoBehaviour
    {
        [SerializeField] Transform _modelRoot;

        public Transform ModelRoot => _modelRoot;

        public void AttachTo(Transform rootObject)
        {
            transform.SetParent(rootObject);
        }
    }
}
