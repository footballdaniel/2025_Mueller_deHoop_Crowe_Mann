using UnityEngine;

namespace VU.Scripts
{
    public class KickBox : MonoBehaviour
    {
        [SerializeField] float _sampleWindowSeconds = 0.2f;
        
        Vector3 _endPosition;
        bool _hasAlreadyKicked;
        Vector3 _startPosition;
        float _startTime;
        
        bool NeedsMoreSamples => Time.time < _startTime + _sampleWindowSeconds;
        bool IsA<T>(GameObject target) where T : MonoBehaviour => target.TryGetComponent<T>(out T component); 
        
        void OnTriggerEnter(Collider other)
        {
            if (_hasAlreadyKicked ) return;

            if (IsA<Foot>(other.gameObject))
            {
                _startTime = Time.time;
                _startPosition = other.transform.position;
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            _hasAlreadyKicked = false;
        }
        
        void OnTriggerStay(Collider other)
        {
            if (NeedsMoreSamples | _hasAlreadyKicked) return;
            _endPosition = other.transform.position;
            _hasAlreadyKicked = true;
            EvaluateKick();
        }
        
        void EvaluateKick()
        {
            var kickDirection = _endPosition - _startPosition;
            print(kickDirection);
        }
    }
}