using UnityEngine;

namespace VUPenalty
{
    public class TimeToIntercept1D
    {
        public void To(Transform target)
        {
            _target = target;
        }

        public void From(Transform trackedObject)
        {
            _trackedObject = trackedObject;
        }


        public void Tick(float deltaTime)
        {
            var current_z = _trackedObject.position.z;
            var delta_z = current_z - _lastZ;

            _lastZ = current_z;
            _averageZ = delta_z / deltaTime;
        }

        public float Prediction(float seconds)
        {
            var prediction = _averageZ * seconds;

            if (float.IsInfinity(prediction))
                return 1000;
            else
                return prediction;
        }


        public float Estimate()
        {
            var target = _target.position.z;
            var trackedObject = _trackedObject.position.z;
            var motionForward = _averageZ;
            var distance_z = target - trackedObject;
            var timeLeft = distance_z / motionForward;

            if (float.IsInfinity(timeLeft))
                return 1000;
            else
                return timeLeft;
        }

        float _averageZ;
        float _lastZ;
        Transform _target;
        Transform _trackedObject;
    }
}