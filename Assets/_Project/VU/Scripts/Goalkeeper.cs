using UnityEngine;

namespace VU.Scripts
{
    public class Goalkeeper : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        public void OnKicked(KickStartEvent obj)
        {
            if (obj.VelocityVector.X > 0)
                _animator.SetTrigger("DiveLeft");
            else
                _animator.SetTrigger("DiveRight"); 
        }
    }
}