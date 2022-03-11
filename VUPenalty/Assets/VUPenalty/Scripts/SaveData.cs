using System;

namespace VUPenalty
{
    class SaveData : ExperimentState
    {
        public SaveData(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            // var trialData = GetTrialData();
            
            
            _context.ReadyForNextTrial();
            
        }


        TrialData GetTrialData()
        {
            var trial = new TrialData()
            {
                ParticipantName = _context.Experiment.ParticipantName,
                ResearchInstitution = _context.Experiment.ResearchInstitution,
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