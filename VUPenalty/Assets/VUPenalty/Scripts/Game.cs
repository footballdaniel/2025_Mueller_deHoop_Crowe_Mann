using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace VUPenalty
{
    public class Game : MonoBehaviour
    {
        [Header("User display mode")] 
        [SerializeField] public GameObject _userPrefab;

        [Header("Prefabs")]
        [SerializeField] GameObject _ballPrefab;
        [SerializeField] GameObject _goalkeeperPrefab;

        [Header("Dependencies")]
        [SerializeField] Experiment _experiment;
        [SerializeField] Foot _foot;
        [SerializeField] TargetArea _targetAreaSuccess;
        [SerializeField] TargetArea _targetAreaMissed;
        [SerializeField] private VideoDisplay _videoDisplay;

        void Awake()
        {
            var userGameObject = Instantiate(_userPrefab);
            var user = userGameObject.GetComponent<User>();
            user.Use(_foot);
        }

        void Start()
        {
            GoToNextTrial();
        }

        private void Update()
        {
            if (!_experiment.IsTrialRunning)
                GoToNextTrial();
        }

        [ContextMenu("Simulate Go to next trial")]
        private void GoToNextTrial()
        {
            Debug.Log($"Start with next trial");
            var trial = _experiment.MoveNext();
            SetupTrial(trial);
            _experiment.IsTrialRunning = true;
        }

        void SetupTrial(TrialInformation trial)
        {
            _ballGO = Instantiate(_ballPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = _ballGO.GetComponent<Ball>();

            _goalkeeperGO = Instantiate(_goalkeeperPrefab);
            _goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();
            _goalkeeper.JumpDirection = trial.JumpDirection;

            _videoDisplay.Video = trial.Video;
            _videoDisplay.SetSize(trial.VideoWidth, trial.VideoHeight);

            // Events
            _experiment.Foot = _foot;
            _ball.OnKick += _experiment.OnKicked;
            _targetAreaSuccess.OnKick += _experiment.OnKickEnded;
            _targetAreaMissed.OnKick += _experiment.OnKickEnded;
            _ball.OnTrialEnd += OnTrialEnded;
            _ball.OnKick += _goalkeeper.Dive;
            
            // Timing dependent variables
            _videoDisplay.Play();
        }

        void OnTrialEnded()
        {
            var data = _experiment.SaveTrialData();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var folderPath = Application.persistentDataPath;
            var dateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss");
            var filePath = Path.Combine(folderPath, $"Trial_{dateTime}.json");
            File.WriteAllText(filePath, json);

            Debug.Log($"Data saved to: {folderPath}");

            // Events
            _ball.OnKick -= _experiment.OnKicked;
            _targetAreaSuccess.OnKick -= _experiment.OnKickEnded;
            _targetAreaMissed.OnKick -= _experiment.OnKickEnded;
            _ball.OnTrialEnd -= OnTrialEnded;

            Destroy(_ballGO);
            Destroy(_goalkeeperGO);
            Destroy(_experimentGO);
        }
        
        GameObject _userGO;
        GameObject _ballGO;
        Ball _ball;
        GameObject _goalkeeperGO;
        Goalkeeper _goalkeeper;
        GameObject _experimentGO;
    }
}