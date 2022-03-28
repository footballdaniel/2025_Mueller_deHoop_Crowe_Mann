
using System;
using UnityEngine;

namespace VUPenalty
{
    public class StartArea : MonoBehaviour
    {
        public bool IsObserverInStartArea;

        void OnTriggerStay(Collider other)
        {
            if (IsA<FootModel>(other.gameObject))
                IsObserverInStartArea = true;
        }
    
        void OnTriggerExit(Collider other)
        {
            if (IsA<FootModel>(other.gameObject))
                IsObserverInStartArea = false;
        }
    
        bool IsA<T>(GameObject target) where T : MonoBehaviour
        {
            var isTrue = target.TryGetComponent(out T component);
            return isTrue;
        }
    }
}
