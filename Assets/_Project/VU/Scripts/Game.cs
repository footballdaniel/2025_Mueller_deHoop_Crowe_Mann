using System;
using UnityEngine;

namespace VU.Scripts
{
    public class Game : MonoBehaviour
    {
        [Header("Prefabs")] 
        public GameObject ViewPrefab;
        public GameObject BallPrefab;

        [Header("Dependencies")] public Foot Foot;
        public Experiment _experiment;
        public TargetArea _goal;
        public TargetArea _missedTarget;

        void Start()
        {
            OnNextNextTrialStarted();
        }

        void OnNextNextTrialStarted()
        {
            _ballGO = Instantiate(BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            var ball = _ballGO.GetComponent<BallKick>();
            
            // Event
            _experiment.Foot = Foot;
            ball.OnKick += _experiment.OnOnKick;
            _goal.OnKick += _experiment.OnKickEnded;
            _missedTarget.OnKick += _experiment.OnKickEnded;
            ball.OnNextTrialStart += OnNextNextTrialStarted;
        }
            
        void Awake()
        {
            _viewGO = Instantiate(ViewPrefab, transform);
            var view = _viewGO.GetComponent<User>();
            view.Visit(Foot);
        }

        GameObject _viewGO;
        GameObject _ballGO;
    }
}