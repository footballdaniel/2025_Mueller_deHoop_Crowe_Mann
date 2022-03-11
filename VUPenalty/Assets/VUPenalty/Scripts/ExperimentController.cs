using System;
using UnityEngine;

namespace VUPenalty
{
    public class ExperimentController : MonoBehaviour
    {
        [Header("Prefabs")] public GameObject BallPrefab;
        public GameObject GoalkeeperPrefab;
        public GameObject DataRecorderPrefab;

        [Header("Dependencies")] public VideoDisplay VideoDisplay;
        public TargetArea TargetAreaMissed;
        public TargetArea TargetAreaSuccess;

        [Header("Debug")] public GameObject InterceptSphere;
        public event Action OnReadyForNextTrial;

        public TrialSetting ActiveTrial { get; set; }
        public ExperimentalData ExperimentalData { get; set; }
        public Ball Ball { get; set; }
        public User User { get; set; }
        public Goalkeeper Goalkeeper { get; set; }
        public DataRecorder DataRecorder { get; set; }
        public Foot Foot { get; set; }

        ExperimentState _currentState;

        public void ChangeState(ExperimentState newState)
        {
            _currentState?.Finish();
            _currentState = newState;
            _currentState.Init();
        }

        void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        public void ReadyForNextTrial()
        {
            OnReadyForNextTrial?.Invoke();
        }
    }
}