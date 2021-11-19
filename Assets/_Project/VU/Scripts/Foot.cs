using UnityEngine;

namespace VU.Scripts
{
    public class Foot : MonoBehaviour
    {
        [SerializeField] Transform _footRoot;


        void Update()
        {
            transform.SetPositionAndRotation(_footRoot.position, _footRoot.rotation);
        }
    }
}
