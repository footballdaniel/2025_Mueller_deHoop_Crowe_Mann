using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    public class Experiment : MonoBehaviour
    {
        [SerializeField] string _participantName = "FirstName_LastName";
        [SerializeField] string _researchInstitution = "VU";
        [SerializeField] float _durationOfDataToSave = 2f;

        public List<TrialInformation> Trials;
        [HideInInspector] public Foot Foot;

        [HideInInspector] public bool IsTrialRunning;

        public void OnKicked(KickStartEvent kickStart)
        {
            _currentKickStart = kickStart;
            _footTrajectoryAtKick = new Queue<Vector3>(_footMovementBuffer);
        }

        public void OnKickEnded(KickEndEvent kickEnd)
        {
            _currentKickEnd = kickEnd;
            SaveTrialData();
            IsTrialRunning = false;
        }

        public TrialData SaveTrialData()
        {
            var trial = new TrialData()
            {
                ParticipantName = _participantName,
                ResearchInstitution = _researchInstitution,
                DateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss"),
                Events = new EventData()
                {
                    End = _currentKickEnd,
                    Start = _currentKickStart
                },
                Tracking = new TrackingData()
                {
                    Foot = _footTrajectoryAtKick.Select<Vector3, Point3D>(x => x).ToList()
                },
                JumpDirection = Trials[_currentTrial].JumpDirection
            };

            return trial;
        }

        void FixedUpdate()
        {
            var secondsOfStoredPositions = _footMovementBuffer.Count * Time.fixedDeltaTime;
            if (secondsOfStoredPositions < _durationOfDataToSave)
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

        public TrialInformation MoveNext()
        {
            _currentTrial++;
            return _currentTrial < Trials.Count ? Trials[_currentTrial] : null;
        }
        
        int _currentTrial = -1;
    }

    [Serializable]
    public enum JumpDirection
    {
        Left,
        Right
    }
    
    [Serializable]
    public class TrialInformation
    {
        public VideoClip Video;
        [Range(1f, 10f)] public float VideoHeight = 1f;
        [Range(1f, 20f)] public float VideoWidth = 10f;
        [Range(0f, 1f)] public float AnticipationTime;
        public JumpDirection JumpDirection;
    }

    [Serializable]
    public class TrialData
    {
        public string ResearchInstitution;
        public string ParticipantName;
        public string DateTime;
        public EventData Events;
        public TrackingData Tracking;
        public JumpDirection JumpDirection;
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