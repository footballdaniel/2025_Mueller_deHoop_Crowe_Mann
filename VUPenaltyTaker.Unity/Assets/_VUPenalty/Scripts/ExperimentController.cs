using System;
using UnityEngine;

namespace VUPenalty
{
    public class ExperimentController : MonoBehaviour
    {
        [Header("Prefabs")] public GameObject BallPrefab;
        public GameObject GoalkeeperPrefab;
        public GameObject DataRecorderPrefab;
        public GameObject StartAreaPrefab;

        [Header("Dependencies")] public VideoDisplay VideoDisplay;
        public TargetArea TargetAreaMissed;
        public TargetArea TargetAreaSuccess;

        [Header("Debug")] public GameObject PredictionKeeper;
        public GameObject PredictionAdvertisement;

        public TrialSetting ActiveTrial { get; set; }
        public ExperimentalData ExperimentalData { get; set; }
        public Ball Ball { get; set; }
        public User User { get; set; }
        public Goalkeeper Goalkeeper { get; set; }
        public DataRecorder DataRecorder { get; set; }
        public Foot Foot { get; set; }
        public GameObject TrialGameObject { get; set; }
        public int TrialNumber => _trialNumber;
        public event Action OnReadyForNextTrial;

        public void ChangeState(ExperimentState newState)
        {
            _currentState?.Finish();
            _currentState = newState;
            _currentState.Init();
        }

        public void ReadyForNextTrial()
        {
            OnReadyForNextTrial?.Invoke();
        }

        public void SetTrialNumber(int current)
        {
            _trialNumber = current;
        }

        void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        ExperimentState _currentState;
        int _trialNumber;
    }
}