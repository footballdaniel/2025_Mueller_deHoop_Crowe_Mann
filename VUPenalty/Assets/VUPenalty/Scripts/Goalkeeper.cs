using UnityEngine;

namespace VUPenalty
{
    public class Goalkeeper : MonoBehaviour
    {
        public JumpDirection JumpDirection;
        [SerializeField] Animator _animator;

        public void Dive()
        {
            switch (JumpDirection)
            {
                case JumpDirection.Left:
                    _animator.SetTrigger("DiveLeft");
                    break;
                case JumpDirection.Right:
                    _animator.SetTrigger("DiveRight");
                    break;
                default:
                    Debug.LogWarning($"Jump direction {JumpDirection} not implemented");
                    break;
            }
        }
    }
}