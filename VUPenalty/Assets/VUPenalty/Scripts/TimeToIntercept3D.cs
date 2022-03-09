using System.Collections.Generic;
using UnityEngine;

namespace VUPenalty
{
    public class TimeToIntercept1D
    {
        private int _movingAverageWindowSize = 10;
        private int _numberOfSamples = 0;
        
        
        struct Observation
        {
            public Vector3 Position;
            public float Time;
        }
        
        private Queue<Observation> _previousObservations = new Queue<Observation>();
        
        Transform _trackedObject;
        Transform _target;
        private float _last_z;
        private float _average_z;


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

            var delta_z = current_z - _last_z;
            
            _last_z = current_z;

            _average_z = delta_z / deltaTime;

            // var currentObservation = new Observation
            // {
            //     Position = _trackedObject.position,
            //     Time = timeSinceLevelLoad,
            // };
            //
            // _numberOfSamples++;
            // _previousObservations.Enqueue(currentObservation);
            //
            // if (_numberOfSamples > _movingAverageWindowSize)
            // {
            //     var earliestObservation = _previousObservations.Dequeue();
            //     var valueDelta = currentObservation.Position - earliestObservation.Position;
            //     var timeDelta = currentObservation.Time - earliestObservation.Time;
            //
            //     Average = valueDelta / timeDelta; 
            //     
            //     DebugGraph.Log(Average.z);
            //     
            //     if (float.IsNaN(Average.magnitude))
            //         Debug.Log("Nan value, probably null division");
            // }



        }


        public float Estimate()
        {
            var target = _target.position.z;
            var trackedObject = _trackedObject.position.z;

            var motionForward = _average_z;

            var distance_z = target - trackedObject;
            
            var timeLeft = distance_z / motionForward;

            return timeLeft;


            // var target2D = Vector3.ProjectOnPlane(_target.position, Vector3.up);
            // var trackedObject2D = Vector3.ProjectOnPlane(_trackedObject.position, Vector3.up);
            //
            // var targetVector = (target2D - trackedObject2D).normalized;
            // var motionTowardsTarget = Vector3.Project(Average, targetVector).magnitude;
            //
            // var distance2D = Vector3.Distance(target2D, trackedObject2D);
            //
            // var timeLeft = distance2D / motionTowardsTarget;
            // timeLeft = motionTowardsTarget == 0 ? 100000 : timeLeft;
            //
            // DebugGraph.Log(timeLeft);
            //
            // if (timeLeft < 0.5)
            //     Debug.Log("Ready");
            //
            // return timeLeft;

        }

        public Vector3 Average { get; private set; }
    }
    
    
    public class TimeToIntercept3D
    {
        private int _movingAverageWindowSize = 10;
        private int _numberOfSamples = 0;
        
        
        struct Observation
        {
            public Vector3 Position;
            public float Time;
        }
        
        private Queue<Observation> _previousObservations = new Queue<Observation>();
        
        private Transform _trackedObject;
        private Transform _target;
        

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

        public Vector3 Average { get; private set; }
    }
}