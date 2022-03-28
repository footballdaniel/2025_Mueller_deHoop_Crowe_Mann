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
            var trial = new TrialData()
            {
                ParticipantName = _context.ExperimentalData.ParticipantName,
                ResearchInstitution = _context.ExperimentalData.ResearchInstitution,
                DateTime = DateTime.Now.ToString("yyyy_M_dd_HH_mm_ss"),
                Events = new EventData()
                {
                    End = _context.DataRecorder.KickEnd,
                    Start = _context.DataRecorder.KickStart
                },
                Tracking = new TrackingData()
                {
                    Foot = _context.DataRecorder.TimeSeries
                },
                JumpDirection = _context.ActiveTrial.JumpDirection
            };

            return trial;
        }
    }
}