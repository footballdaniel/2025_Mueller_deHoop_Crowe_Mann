using System;
using UnityEngine;
using VUPenalty;

namespace VU.Scripts
{
    public class TargetArea : MonoBehaviour
    {
        [SerializeField] bool _representsSuccess;
        
        public event Action<KickEndEvent> OnKick;
        
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
                OnKick?.Invoke(new KickEndEvent()
                {
                    EndLocation = ball.transform.position,
                    Success = _representsSuccess
                });
        }
    }
}