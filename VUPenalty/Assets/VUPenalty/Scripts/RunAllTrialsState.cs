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

            _currentTrialSetting = _experimentController.ActiveTrial;
            OnReadyForNextExperiment();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }

        void OnReadyForNextExperiment()
        {
            if (_experiment.MoveNext())
            {
                _experimentController.ActiveTrial = _currentTrialSetting;
                _experimentController.OnTrialEnd += OnReadyForNextExperiment;
                _experimentController.ChangeState(new SetupExperimentState(_experimentController));
            }
            else
            {
                _context.ChangeState(new EndState(_context));
            }
        }

        TrialSetting _currentTrialSetting;
        ExperimentController _experimentController;
        Experiment _experiment;
    }
}