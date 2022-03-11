using System;
using UnityEngine;

namespace VUPenalty
{
    public class ExperimentController : MonoBehaviour
    {
        [Header("Prefabs")] 
        public GameObject BallPrefab;
        public GameObject GoalkeeperPrefab;
        ExperimentState _currentState;

        [Header("Dependencies")] public VideoDisplay VideoDisplay;
        
        [HideInInspector] public TrialSetting ActiveTrial { get; set; }
        public Experiment Experiment { get; set; }

        public event Action OnTrialEnd;

        public void ChangeState(SetupExperimentState newState)
        {
            _currentState?.Finish();
            _currentState = newState;
            _currentState.Init();
        }
    }
}