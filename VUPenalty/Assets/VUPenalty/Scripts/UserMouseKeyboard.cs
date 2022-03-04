using UnityEngine;
using VUPenalty;

namespace VU.Scripts
{
    public class UserMouseKeyboard : User
    {
        [SerializeField] RuntimeAnimatorController _fakeKick;
        [SerializeField] UserInput _userInput;
        
        
        public override GazeInformation Gaze { get; }
        public override HeadInformation Head { get; }
        public override Transform FootRootElement => transform;

        public override void Use(Foot foot)
        {
            _foot = foot;
            // Add some random rotation around X axis
            _foot.transform.Rotate(90, 0f, 0f);
            _animator = foot.gameObject.AddComponent<Animator>();
            _animator.runtimeAnimatorController = _fakeKick;
        }

        void Update()
        {
            if (_userInput.HasKicked())
                _animator.SetTrigger("Kick");
        }

        GameObject _footanimationGO;
        Foot _foot;
        Animator _animator;
    }
}