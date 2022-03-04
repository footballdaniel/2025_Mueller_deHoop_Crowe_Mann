using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VU.Scripts;

namespace VUPenalty
{
    public class Experiment : MonoBehaviour
    {
        public string ParticipantName = "DefaultName";
        public float DurationOfDataToSave = 2f;
        [HideInInspector] public Foot Foot;

        public void OnKicked(KickStartEvent kickStart)
        {
            _currentKickStart = kickStart;
            _footTrajectoryAtKick = new Queue<Vector3>(_footMovementBuffer);
        }

        public void OnKickEnded(KickEndEvent kickEnd)
        {
            _currentKickEnd = kickEnd;
            SaveTrialData();
            print("Kick ended");
        }

        public Trial SaveTrialData()
        {
            var trial = new Trial()
            {
                ParticipantName = ParticipantName,
                DateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss"),
                Events = new EventData()
                {
                    End = _currentKickEnd,
                    Start = _currentKickStart
                },
                Tracking = new TrackingData()
                {
                    Foot = _footTrajectoryAtKick.Select<Vector3, Point3D>(x => x).ToList()
                }
            };

            return trial;
        }

        void FixedUpdate()
        {
            var secondsOfStoredPositions = _footMovementBuffer.Count * Time.fixedDeltaTime;
            if (secondsOfStoredPositions < DurationOfDataToSave)
            {
                _footMovementBuffer.Enqueue(Foot.transform.position);
            }
            else
            {
                _footMovementBuffer.Dequeue();
                _footMovementBuffer.Enqueue(Foot.transform.position);
            }
        }

        readonly Queue<Vector3> _footMovementBuffer = new();
        Queue<Vector3> _footTrajectoryAtKick;
        KickStartEvent _currentKickStart;
        KickEndEvent _currentKickEnd;
    }

    [Serializable]
    public class Trial
    {
        public string ParticipantName;
        public string DateTime;
        public EventData Events;
        public TrackingData Tracking;
    }

    [Serializable]
    public class TrackingData
    {
        public List<Point3D> Foot;
    }

    [Serializable]
    public class EventData
    {
        public KickStartEvent Start;
        public KickEndEvent End;
    }

    [Serializable]
    public class KickEndEvent
    {
        public Point3D EndLocation;
        public bool Success;
    }
}