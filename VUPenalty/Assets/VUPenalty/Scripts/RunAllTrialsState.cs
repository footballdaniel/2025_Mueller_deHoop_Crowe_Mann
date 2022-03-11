using UnityEngine;

namespace VUPenalty
{
    class RunAllTrialsState : GameState
    {
        public RunAllTrialsState(Game context) : base(context)
        {
        }

        public override void Init()
        {
            _experiment = _context.Experiment.GetComponent<Experiment>();
            _experimentController = _context.Experiment.GetComponent<ExperimentController>();
            _experimentController.Experiment = _experiment;
            _experimentController.User = _context.ActiveUser;
            _experimentController.Foot = _context.Foot;
            
            _experimentController.OnReadyForNextTrial += ReadyForNextTrial;
            _NumberOfTrials = _experiment.TrialSettings.Count;
            _Current = -1;
            
            ReadyForNextTrial();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
            _experimentController.OnReadyForNextTrial -= ReadyForNextTrial;
        }

        void ReadyForNextTrial()
        {
            Debug.Log("Ready for next trial if trial is available");
            
            if (_Current < _NumberOfTrials)
            {
                _Current++;
                Debug.Log($"Loading Trial number {_Current}");
                _experimentController.ActiveTrial = _experiment.TrialSettings[_Current];
                _experimentController.ChangeState(new SetupTrial(_experimentController));
            }
            else
            {
                _context.ChangeState(new EndState(_context));
            }
        }
        
        ExperimentController _experimentController;
        Experiment _experiment;
        int _NumberOfTrials;
        int _Current;
    }
}