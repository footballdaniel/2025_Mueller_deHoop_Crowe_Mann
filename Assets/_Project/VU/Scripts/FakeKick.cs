using UnityEngine;

namespace VU.Scripts
{
    public class FakeKick : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Transform _foot;
        [SerializeField] UserInput _input;

        int _kick = Animator.StringToHash("Kick");

        void Update()
        {
            if (_input.HasKicked())
                _animator.SetTrigger(_kick);
        }
    }
}