using System;
using System.IO;
using UnityEngine;

namespace VUPenalty
{
    class SaveState : ExperimentState
    {
        public SaveState(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            SaveData(GetLastTrial());
            WipeGameObjects();
            _context.ReadyForNextTrial();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }

        void SaveData(TrialData data)
        {
            var json = JsonUtility.ToJson(data, true);
            var folderPath = Application.persistentDataPath;
            var dateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss");
            var filePath = Path.Combine(folderPath, $"Trial_{dateTime}.json");
            File.WriteAllText(filePath, json);
            Debug.Log($"Data saved to: {folderPath}");
        }

        void WipeGameObjects()
        {
            GameObject.Destroy(_context.TrialGameObject);
        }


        TrialData GetLastTrial()
        {
            var videoName = _context.ActiveTrial.Video == null ? "Null" : _context.ActiveTrial.Video.name;
            
            var trial = new TrialData()
            {
                ResearchInstitution = _context.ExperimentalData.ResearchInstitution,
                ParticipantName = _context.ExperimentalData.ParticipantName,
                TrialNumber = _context.TrialNumber,
                DateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss"),
                VideoName = videoName,
                VideoHeight = _context.ExperimentalData.VideoHeight,
                VideoWidth = _context.ExperimentalData.VideoWidth,
                GoalkeeperStartBeforeKick = _context.ActiveTrial.GoalkeeperStartBeforeKick,
                AdvertisementStartBeforeKick = _context.ActiveTrial.AdvertisementStartBeforeKick,
                BallElasticity = _context.ExperimentalData.BallElasticity,
                JumpDirection = Enum.GetName(typeof(Direction), _context.ActiveTrial.JumpDirection),
                Events = new EventData(_context.DataRecorder.KickEnd, _context.DataRecorder.KickStart, _context.DataRecorder.GetDiveData()),
                Tracking = new TrackingData(_context.DataRecorder.TimeSeries)
            };
            return trial;
        }
    }
}