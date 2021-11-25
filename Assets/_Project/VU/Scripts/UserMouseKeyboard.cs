using System;
using UnityEngine;

namespace VU.Scripts
{
    public class UserMouseKeyboard : User
    {
        [SerializeField] RuntimeAnimatorController _fakeKick;
        [SerializeField] UserInput _userInput;
        
        
        public override GazeInformation Gaze { get; }
        public override HeadInformation Head { get; }
        public override Transform FootRootElement => transform;

        public override void Visit(Foot foot)
        {
            _foot = foot;
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