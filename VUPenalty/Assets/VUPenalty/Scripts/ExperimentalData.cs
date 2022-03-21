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
        public Texture GoalKeeperColor;
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