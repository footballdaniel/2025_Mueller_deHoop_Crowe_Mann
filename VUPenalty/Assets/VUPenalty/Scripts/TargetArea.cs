using System;
using UnityEngine;

namespace VUPenalty
{
    public class TargetArea : MonoBehaviour
    {
        [SerializeField] bool _representsSuccess;
        
        public event Action<KickEndEvent> OnHit;
        
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<VUPenalty.Ball>(out var ball))
                OnHit?.Invoke(new KickEndEvent()
                {
                    EndLocation = ball.transform.position,
                    Success = _representsSuccess
                });
        }
    }
}