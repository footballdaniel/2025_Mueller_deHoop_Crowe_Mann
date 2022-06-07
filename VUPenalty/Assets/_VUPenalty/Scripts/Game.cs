using UnityEngine;

namespace VUPenalty
{
    public class Game : MonoBehaviour
    {
        [Header("Prefabs")] public GameObject UserPrefab;

        [Header("Dependencies")] 
        public GameObject Experiment;
        public Ui UI;
        public Foot Foot;
        
        public User ActiveUser { get; set; }

        void Start()
        {
            ChangeState(new SetupUserGameState(this));
        }

        void Update()
        {
            _currentState.Tick(Time.deltaTime);
        }

        public void ChangeState(GameState newState)
        {
            _currentState?.Finish();
            _currentState = newState;
            _currentState.Init();
        }

        GameState _currentState;
    }
}