using System;
using UnityEngine;

namespace VU.Scripts
{
    public class TargetArea : MonoBehaviour
    {
        [SerializeField] bool _representsSuccess;
        
        public event Action<KickEndEvent> OnKick;
        
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BallKick>(out var ball))
                OnKick?.Invoke(new KickEndEvent()
                {
                    EndLocation = ball.transform.position,
                    Success = _representsSuccess
                });
        }
    }
}