using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VU.Scripts
{
    public class Experiment : MonoBehaviour
    {
        public string ParticipantName = "DefaultName";
        public Foot Foot;
        public float Memory = 2f;

        public void OnKicked(KickStartEvent kickStart)
        {
            _currentKickStart = kickStart;
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
                Events = new EventData()
                {
                    End = _currentKickEnd,
                    Start = _currentKickStart
                },
                Tracking = new TrackingData()
                {
                    Foot = _footMovement.Select<Vector3, Point3D>(x => x).ToList()
                }
            };

            return trial;
        }

        void FixedUpdate()
        {
            var secondsOfStoredPositions = _footMovement.Count / Time.fixedDeltaTime;
            if (secondsOfStoredPositions < Memory)
            {
                _footMovement.Enqueue(Foot.transform.position);
            }
            else
            {
                _footMovement.Dequeue();
                _footMovement.Enqueue(Foot.transform.position);
            }
        }

        readonly Queue<Vector3> _footMovement = new();

        KickStartEvent _currentKickStart;
        KickEndEvent _currentKickEnd;
    }

    [Serializable]
    public class Trial
    {
        public string ParticipantName;
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