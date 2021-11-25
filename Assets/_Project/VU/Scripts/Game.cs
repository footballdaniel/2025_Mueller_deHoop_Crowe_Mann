using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace VU.Scripts
{
    public class Game : MonoBehaviour
    {
        [Header("Prefabs")] public GameObject ViewPrefab;
        public GameObject BallPrefab;

        [Header("Dependencies")] public Foot Foot;
        public Experiment _experiment;
        public TargetArea _goal;
        public TargetArea _missedTarget;

        void Awake()
        {
            _viewGO = Instantiate(ViewPrefab, transform);
            var view = _viewGO.GetComponent<User>();
            view.Visit(Foot);
        }

        void Start()
        {
            _ballGO = Instantiate(BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = _ballGO.GetComponent<BallKick>();

            // Events
            _experiment.Foot = Foot;
            _ball.OnKick += _experiment.OnKicked;
            _goal.OnKick += _experiment.OnKickEnded;
            _missedTarget.OnKick += _experiment.OnKickEnded;
            _ball.OnTrialEnd += OnTrialEnded;
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
            Destroy(_ballGO);


            _ballGO = Instantiate(BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            var ball = _ballGO.GetComponent<BallKick>();

            // Events
            _experiment.Foot = Foot;
            ball.OnKick += _experiment.OnKicked;
            _goal.OnKick += _experiment.OnKickEnded;
            _missedTarget.OnKick += _experiment.OnKickEnded;
            ball.OnTrialEnd += OnTrialEnded;
        }

        GameObject _viewGO;
        GameObject _ballGO;
        BallKick _ball;
    }
}