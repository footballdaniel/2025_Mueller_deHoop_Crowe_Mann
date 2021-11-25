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
        
        KickStartEvent _currentKickStart;
        KickEndEvent _currentKickEnd;

        public Queue<Vector3> FootMovement = new();

        void FixedUpdate()
        {
            var secondsOfStoredPositions = FootMovement.Count / Time.fixedDeltaTime;
            if (secondsOfStoredPositions < Memory)
                FootMovement.Enqueue(Foot.transform.position);
            else
            {
                FootMovement.Dequeue();
                FootMovement.Enqueue(Foot.transform.position);
            }
        }

        public void OnOnKick(KickStartEvent kickStart)
        {
            _currentKickStart = kickStart;
        }

        public void OnKickEnded(KickEndEvent kickEnd)
        {
            _currentKickEnd = kickEnd;
            SaveTrialData();
            print("Kick ended");
        }

        public void SaveTrialData()
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
                    Foot = FootMovement.ToList()
                }
            };
        }
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
        public List<Vector3> Foot;
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