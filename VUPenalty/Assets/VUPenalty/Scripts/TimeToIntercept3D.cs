using System.Collections.Generic;
using UnityEngine;

namespace VUPenalty
{
    public class TimeToIntercept1D
    {
        float _averageZ;
        float _lastZ;
        Transform _target;
        Transform _trackedObject;

        public bool IsApproaching => (_target.position.z - _trackedObject.position.z) < 0;


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


        public float Estimate()
        {
            var target = _target.position.z;
            var trackedObject = _trackedObject.position.z;
            var motionForward = _averageZ;
            var distance_z = target - trackedObject;
            var timeLeft = distance_z / motionForward;
            return timeLeft;
        }
    }


    public class TimeToIntercept3D
    {
        private int _movingAverageWindowSize = 10;
        private int _numberOfSamples = 0;

        private Queue<Observation> _previousObservations = new Queue<Observation>();
        private Transform _target;

        private Transform _trackedObject;

        public Vector3 Average { get; private set; }


        public void To(Transform target)
        {
            _target = target;
        }

        public void From(Transform trackedObject)
        {
            _trackedObject = trackedObject;
        }


        public void Tick(float timeSinceLevelLoad)
        {
            // DebugGraph.Log(Average);

            var currentObservation = new Observation
            {
                Position = _trackedObject.position,
                Time = timeSinceLevelLoad,
            };

            _numberOfSamples++;
            _previousObservations.Enqueue(currentObservation);

            if (_numberOfSamples > _movingAverageWindowSize)
            {
                var earliestObservation = _previousObservations.Dequeue();
                var valueDelta = currentObservation.Position - earliestObservation.Position;
                var timeDelta = currentObservation.Time - earliestObservation.Time;

                Average = valueDelta / timeDelta;

                if (float.IsNaN(Average.magnitude))
                    Debug.Log("Nan...");
            }
        }


        public float Estimate()
        {
            var target2D = Vector3.ProjectOnPlane(_target.position, Vector3.up);
            var trackedObject2D = Vector3.ProjectOnPlane(_trackedObject.position, Vector3.up);

            var targetVector = (target2D - trackedObject2D).normalized;
            var motionTowardsTarget = Vector3.Project(Average, targetVector).magnitude;

            var distance2D = Vector3.Distance(target2D, trackedObject2D);

            var timeLeft = distance2D / motionTowardsTarget;
            timeLeft = motionTowardsTarget == 0 ? 100000 : timeLeft;

            // DebugGraph.Log(timeLeft);

            if (timeLeft < 0.5)
                Debug.Log("Ready");

            return timeLeft;
        }


        struct Observation
        {
            public Vector3 Position;
            public float Time;
        }
    }
}