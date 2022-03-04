using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VU.Scripts;

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
        [SerializeField] TargetArea _goal;
        [SerializeField] TargetArea _missedTarget;

        void Awake()
        {
            var userGameObject = Instantiate(_userPrefab);
            var user = userGameObject.GetComponent<User>();
            user.Use(_foot);
        }

        void Start()
        {
            SetupTrial();
        }

        void SetupTrial()
        {
            _ballGO = Instantiate(_ballPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = _ballGO.GetComponent<VU.Scripts.Ball>();

            _goalkeeperGO = Instantiate(_goalkeeperPrefab);
            _goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();

            // Events
            _experiment.Foot = _foot;
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
        VU.Scripts.Ball _ball;
        GameObject _goalkeeperGO;
        Goalkeeper _goalkeeper;
        GameObject _experimentGO;
    }
}