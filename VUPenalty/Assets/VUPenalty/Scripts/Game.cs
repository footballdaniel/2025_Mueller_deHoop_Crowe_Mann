using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace VU.Scripts
{
    public class Game : MonoBehaviour
    {
        [Header("User display mode")] 
        public GameObject UserPrefab;

        [Header("Prefabs")]
        public GameObject BallPrefab;
        public GameObject GoalkeeperPrefab;

        [Header("Dependencies")]
        [SerializeField] Experiment _experiment;
        public Foot Foot;
        public TargetArea _goal;
        public TargetArea _missedTarget;

        void Awake()
        {
            _userGO = Instantiate(UserPrefab);
            var user = _userGO.GetComponent<User>();
            user.Visit(Foot);
        }

        void Start()
        {
            SetupTrial();
        }

        void SetupTrial()
        {
            _ballGO = Instantiate(BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = _ballGO.GetComponent<BallKick>();

            _goalkeeperGO = Instantiate(GoalkeeperPrefab);
            _goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();

            // Events
            _experiment.Foot = Foot;
            _ball.OnKick += _experiment.OnKicked;
            _goal.OnKick += _experiment.OnKickEnded;
            _missedTarget.OnKick += _experiment.OnKickEnded;
            _ball.OnTrialEnd += OnTrialEnded;
            _ball.OnKick += _goalkeeper.OnKicked;
        }

        void OnTrialEnded()
        {
            var data = _experiment.SaveTrialData();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            // var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folderPath = Application.persistentDataPath;
            var dateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss");
            var filePath = Path.Combine(folderPath, $"Trial_{dateTime}.json");
            File.WriteAllText(filePath, json);

            Debug.Log($"Data saved to: {folderPath}");

            // Events
            _ball.OnKick -= _experiment.OnKicked;
            _goal.OnKick -= _experiment.OnKickEnded;
            _missedTarget.OnKick -= _experiment.OnKickEnded;
            _ball.OnTrialEnd -= OnTrialEnded;
            _ball.OnKick -= _goalkeeper.OnKicked;

            Destroy(_ballGO);
            Destroy(_goalkeeperGO);
            Destroy(_experimentGO);

            SetupTrial();
        }
        
        GameObject _userGO;
        GameObject _ballGO;
        BallKick _ball;
        GameObject _goalkeeperGO;
        Goalkeeper _goalkeeper;
        GameObject _experimentGO;
    }
}