using System;
using UnityEditor;
using UnityEngine;

namespace VUPenalty
{
    class SaveData : ExperimentState
    {
        public SaveData(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            WipeGameObjects();
            _context.ReadyForNextTrial();
        }

        void WipeGameObjects()
        {
            GameObject.Destroy(_context.TrialGameObject);
        }


        TrialData GetTrialData()
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

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }
    }
}