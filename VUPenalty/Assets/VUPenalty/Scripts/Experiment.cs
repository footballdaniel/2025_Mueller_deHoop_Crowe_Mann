using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    public class Experiment : MonoBehaviour, IEnumerable<TrialSetting>
    {
        [SerializeField] string _participantName = "FirstName_LastName";
        [SerializeField] string _researchInstitution = "VU";
        [Range(1f, 10f)] public float VideoHeight = 1f;
        [Range(1f, 20f)] public float VideoWidth = 10f;
        [SerializeField] float _durationOfDataToSave = 2f;

        public List<TrialSetting> TrialSettings;
        [HideInInspector] public Foot Foot;

        [HideInInspector] public bool IsTrialRunning;
        [HideInInspector] public TrialSetting Current;

        readonly Queue<Vector3> _footMovementBuffer = new();
        KickEndEvent _currentKickEnd;
        KickStartEvent _currentKickStart;

        int _currentTrial = -1;
        Queue<Vector3> _footTrajectoryAtKick;

        void FixedUpdate()
        {
            if (IsTrialRunning)
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
        }

        public event Action OnTrialEnd;

        public void OnKicked(KickStartEvent kickStart)
        {
            _currentKickStart = kickStart;
            _footTrajectoryAtKick = new Queue<Vector3>(_footMovementBuffer);
            StartCoroutine(Delay(3f, OnTrialEnd));
        }

        IEnumerator Delay(float duration, Action callback)
        {
            yield return new WaitForSeconds(duration);
            callback();
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
                JumpDirection = TrialSettings[_currentTrial].JumpDirection
            };

            return trial;
        }

        public bool MoveNext()
        {
            _currentTrial++;

            var canProceed = _currentTrial < TrialSettings.Count;

            if (canProceed)
                Current = TrialSettings[_currentTrial];
            
            return canProceed;
        }

        public IEnumerator<TrialSetting> GetEnumerator()
        {
            return TrialSettings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public enum JumpDirection
    {
        Left,
        Right
    }
    
    [Serializable]
    public class TrialSetting
    {
        public VideoClip Video;
        [Range(0f, 1f)] public float GoalkeeperStartBeforeKick;
        [Range(0f, 10f)] public float AdvertisementStartBeforeKick;
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