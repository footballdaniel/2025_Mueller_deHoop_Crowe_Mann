using System;
using System.Collections;
using UnityEngine;

namespace VUPenalty
{
    public class Ball : MonoBehaviour
    {
        public event Action<KickStartEvent> OnKick;


        void OnTriggerEnter(Collider other)
        {
            if (_isCoolDown) return;

            if (IsA<FootModel>(other.gameObject))
            {
                _footIsKickingBall = true;
                _startTime = Time.time;
                print("Foot kick detected");
                _startPosition = other.transform.position;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (_footIsKickingBall & !_isCoolDown)
            {
                _endPosition = other.transform.position;
                EvaluateKick();
                _isCoolDown = true;
                StartCoroutine(ExitCooldown(1f));
            }

            _footIsKickingBall = false;
        }

        IEnumerator ExitCooldown(float f)
        {
            yield return new WaitForSeconds(f);
            _isCoolDown = false;
        }

        bool IsA<T>(GameObject target) where T : MonoBehaviour
        {
            var isTrue = target.TryGetComponent(out T component);
            return isTrue;
        }

        void EvaluateKick()
        {
            var kickDirection = _endPosition - _startPosition;

            var rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;

            var kickTime = Time.time - _startTime;
            var velocityMeterPerSecond = kickDirection / kickTime;
            var velocity = velocityMeterPerSecond;
            rb.velocity = velocity * 4f;


            var kickData = new KickStartEvent()
            {
                Origin = _startPosition,
                VelocityVector = kickDirection
            };
            
            OnKick?.Invoke(kickData);
        }
        
        bool _isCoolDown;
        Vector3 _endPosition;
        bool _isKickInProgress;
        Vector3 _startPosition;
        float _startTime;
        bool _footIsKickingBall;
    }

    [Serializable]
    public class KickStartEvent
    {
        public Point3D Origin;
        public Point3D VelocityVector;
    }

    [Serializable]
    public class Point3D
    {
        public static implicit operator Point3D(Vector3 input) =>
            new()
            {
                X = input.x,
                Y = input.y,
                Z = input.z
            };

        public float X;
        public float Y;
        public float Z;
    }
}