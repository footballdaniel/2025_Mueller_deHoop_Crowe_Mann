using System;
using UnityEngine;

namespace VU.Scripts
{
    public class Foot : MonoBehaviour
    {
        public void AttachTo(Transform rootObject)
        {
            transform.SetParent(rootObject, false);
        }

        void Update()
        {
            if (transform.position.z > 0.9f)
                Debug.Log("Has transformed");
        }
    }
}
