using UnityEngine;
using VU.Scripts;

namespace VUPenalty
{
    public class UserVR : User
    {
        [SerializeField] Transform _footRoot;
        [SerializeField] GazeInformation _gazeInformation;
        [SerializeField] HeadInformation _headInformation;

        public override GazeInformation Gaze => _gazeInformation;
        public override HeadInformation Head => _headInformation;
        public override Transform FootRootElement => _footRoot;

        public override void Use(Foot foot)
        {
            foot.AttachTo(_footRoot);
        }
    }
}