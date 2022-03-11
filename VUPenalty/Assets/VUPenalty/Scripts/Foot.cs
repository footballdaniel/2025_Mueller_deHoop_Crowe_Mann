using UnityEngine;

namespace VUPenalty
{
    public class Foot : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        
        public void AttachTo(Transform rootObject)
        {
            transform.SetParent(rootObject, false);
        }

        public void RotateModel(Quaternion rotation)
        {
            _model.transform.rotation *= Quaternion.Inverse(rotation);
        }
        
        
    }
}
