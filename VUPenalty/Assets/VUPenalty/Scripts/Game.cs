using UnityEngine;

namespace VUPenalty
{
    public class Game : MonoBehaviour
    {
        [Header("Prefabs")] public GameObject UserPrefab;


        [Header("Dependencies")] public GameObject Experiment;
        public Foot Foot;
        public TargetArea TargetAreaSuccess;
        public TargetArea TargetAreaMissed;
        public VideoDisplay VideoDisplay;

        [HideInInspector] public TrialSetting ActiveTrialSetting;
        [HideInInspector] public User ActiveUser;

        void Start()
        {
            ChangeState(new SetupUserGameState(this));
        }

        void Update()
        {
            _currentState.Tick(Time.deltaTime);
        }

        public void ChangeState(GameState newTrialState)
        {
            _currentState?.Finish();
            _currentState = newTrialState;
        }

        GameState _currentState;
    }
}