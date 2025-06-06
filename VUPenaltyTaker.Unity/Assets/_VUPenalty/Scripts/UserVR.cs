using UnityEngine;

namespace VUPenalty
{
    public class UserVR : User
    {
        [SerializeField] Transform _footRoot;
        [SerializeField] Camera _camera;

        public override Transform Head => _camera.transform;

        public override Transform FootRootElement => _footRoot;

        public override void Use(Foot foot)
        {
            foot.AttachTo(_footRoot);
        }
    }
}