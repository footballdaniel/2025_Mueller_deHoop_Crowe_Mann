using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VUPenalty
{
    public class ExperimentalData : MonoBehaviour
    {
        public string ParticipantName = "FirstName_LastName";
        public string ResearchInstitution = "VU";
        [Range(1f, 10f)] public float VideoHeight = 1f;
        [Range(1f, 20f)] public float VideoWidth = 10f;
        [Range(0f, 5f)] public float BallElasticity = 2f;
        public List<TrialSetting> TrialSettings;
    }

    [Serializable]
    public enum Direction
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
        public Direction JumpDirection;
        public Direction AdvertisementDirection;
        public Texture GoalKeeperColor;
        [Range(-3.66f, 3.66f)] public float GoalKeeperDisplacement;
    }

    [Serializable]
    public class TrialData
    {
        public string ResearchInstitution;
        public string ParticipantName;
        public int TrialNumber;
        public string DateTime;
        public string VideoName;
        public float VideoHeight;
        public float VideoWidth;
        public float GoalkeeperStartBeforeKick;
        public float AdvertisementStartBeforeKick;
        public float BallElasticity;
        public string JumpDirection;
        public EventData Events;
        public TrackingData Tracking;
    }

    [Serializable]
    public class TrackingData
    {
        public List<Point3D> Foot;

        public TrackingData(List<Point3D> foot)
        {
            Foot = foot;
        }
    }

    [Serializable]
    public class EventData
    {
        public EventData(KickEndEvent end, KickStartEvent start, KeeperDiveData dive)
        {
            End = end;
            Start = start;
            Dive = dive;
        }

        public KickStartEvent Start;
        public KickEndEvent End;
        public KeeperDiveData Dive;
    }

    
    public class KeeperDiveEvent
    {
        public KeeperDiveEvent(Direction direction)
        {
            Direction = direction;
        }
        
        public Direction Direction;
    }

    [Serializable]
    public class KeeperDiveData
    {
        public KeeperDiveData(float secondsBeforeKick, Direction direction)
        {
            SecondsBeforeKick = secondsBeforeKick;
            _direction = direction;
        }
        
        public Direction _direction;
        public float SecondsBeforeKick;
    }

    [Serializable]
    public class KickEndEvent
    {
        public Point3D EndLocation;
        public bool Success;
    }
}