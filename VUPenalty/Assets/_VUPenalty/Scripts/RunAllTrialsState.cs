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
            _experimentalData = _context.Experiment.GetComponent<ExperimentalData>();
            _experimentController = _context.Experiment.GetComponent<ExperimentController>();
            _experimentController.ExperimentalData = _experimentalData;
            _experimentController.User = _context.ActiveUser;
            _experimentController.Foot = _context.Foot;
            
            _experimentController.OnReadyForNextTrial += ReadyForNextTrial;
            _NumberOfTrials = _experimentalData.TrialSettings.Count;
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
            _Current++;
            
            if (_Current < _NumberOfTrials)
            {
                Debug.Log($"Loading Trial number {_Current}");
                _experimentController.ActiveTrial = _experimentalData.TrialSettings[_Current];
                _experimentController.SetTrialNumber(_Current);
                _experimentController.ChangeState(new SetupTrial(_experimentController));
            }
            else
            {
                _context.ChangeState(new EndState(_context));
            }
        }
        
        ExperimentController _experimentController;
        ExperimentalData _experimentalData;
        int _NumberOfTrials;
        int _Current;
    }
}