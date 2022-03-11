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

        [ContextMenu("Simulate Kick")]
        void SimulateKick()
        {
            _ball.SimulateKick();
        }

        void Awake()
        {
            var userGameObject = Instantiate(_userPrefab);
            _user = userGameObject.GetComponent<User>();
            _user.Use(_foot);
            _timeToIntercept = new TimeToIntercept1D();
        }

        void Start()
        {
            GoToNextTrial();
        }
        


        private void Update()
        {
            _warmupCounter++;

            if (_warmupCounter > 300)
            {
                if (_experiment.IsTrialRunning)
                {
                    _timeToIntercept.Tick(Time.deltaTime);

                    var time = _timeToIntercept.Estimate();

                    if (Camera.main.transform.position.z < 0)
                    {
                        if (time < _currentTrial.AdvertisementStartBeforeKick & ! _hasAdvertisementStarted)
                        {
                            _hasAdvertisementStarted = true;
                            _videoDisplay.Play();
                        }

                        if (time < _currentTrial.GoalkeeperStartBeforeKick & ! _hasGoalkeeperStarted)
                        {
                            _hasGoalkeeperStarted = true;
                            _goalkeeper.Dive();
                        }
                    }
                }
                else
                {
                    GoToNextTrial();
                }
            }

        }

        [ContextMenu("Calibrate")]
        private void CalibrateFoot()
        {
            _user.Calibrate(_foot);
        }


        [ContextMenu("Simulate Go to next trial")]
        private void GoToNextTrial()
        {

            if (_experiment.MoveNext())
            {
                Debug.Log($"Start with next trial");
                _currentTrial = _experiment.Current;
                SetupTrial(_currentTrial);
                _experiment.IsTrialRunning = true;
            }
            else
            {
                Debug.Log("Experiment has finished");
            }
            
        }

        void SetupTrial(TrialInformation trial)
        {
            _hasAdvertisementStarted = false;
            _hasGoalkeeperStarted = false;
            
            _ballGO = Instantiate(_ballPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = _ballGO.GetComponent<Ball>();

            _goalkeeperGO = Instantiate(_goalkeeperPrefab);
            _goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();
            _goalkeeper.JumpDirection = trial.JumpDirection;

            _videoDisplay.Video = trial.Video;
            _videoDisplay.SetSize(_experiment.VideoWidth, _experiment.VideoHeight);

            _timeToIntercept.From(_user.Head);
            _timeToIntercept.To(_ballGO.transform);
            
            _experiment.Foot = _foot;
            
            // Events
            _ball.OnKick += _experiment.OnKicked;
            _targetAreaSuccess.OnKick += _experiment.OnKickEnded;
            _targetAreaMissed.OnKick += _experiment.OnKickEnded;
            _experiment.OnTrialEnd += OnTrialEnded;
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
            _experiment.OnTrialEnd -= OnTrialEnded;

            Destroy(_ballGO);
            Destroy(_goalkeeperGO);
            Destroy(_experimentGO);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(50f, 50f, 500f, 50f), $"Participant is in run up position: {Camera.main.transform.position.z < 0}");
        }

        GameObject _userGO;
        GameObject _ballGO;
        Ball _ball;
        GameObject _goalkeeperGO;
        Goalkeeper _goalkeeper;
        GameObject _experimentGO;
        private User _user;
        private TimeToIntercept1D _timeToIntercept;
        private TrialInformation _currentTrial;
        private bool _hasAdvertisementStarted;
        private bool _hasGoalkeeperStarted;
        private int _warmupCounter;
    }
}