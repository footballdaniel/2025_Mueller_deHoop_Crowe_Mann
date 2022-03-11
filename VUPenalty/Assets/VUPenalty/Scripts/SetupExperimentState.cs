using UnityEngine;

namespace VUPenalty
{
    public class SetupExperimentState : ExperimentState
    {
        public SetupExperimentState(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            Debug.Log("Setup trial");

            var ballGo = Object.Instantiate(_context.BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _ball = ballGo.GetComponent<Ball>();

            var _goalkeeperGO = Object.Instantiate(_context.GoalkeeperPrefab);
            Goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();
            Goalkeeper.JumpDirection = _context.ActiveTrial.JumpDirection;

            _context.VideoDisplay.LoadVideo(_context.ActiveTrial.Video);
            // _context.VideoDisplay.SetSize(_context, _context._experiment.VideoHeight);
            
            // _context.ChangeState(new WaitForKickTrialState(_context) );
        }


        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }

        bool _hasAdvertisementStarted;
        bool _hasGoalkeeperStarted;
        Ball _ball;
        TimeToIntercept1D _timeToIntercept;
    }
    
    public class WaitForKickExperimentState : ExperimentState
    {
        TimeToIntercept1D _timeToIntercept;

        public WaitForKickExperimentState(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            // _timeToIntercept = new TimeToIntercept1D();
            // _timeToIntercept.From(_context.ActiveUser.Head);
            // _timeToIntercept.To(new GameObject().transform);
            //
            //
            // // Callbacks
            // _context.Ball.OnKick += Trial.OnKicked;
            // _context.TargetAreaSuccess.OnKick += this.trials.OnKickEnded;
            // _context.TargetAreaMissed.OnKick += this.trials.OnKickEnded;
            // this.trials.OnTrialEnd += OnTrialsEnded;
        }

        public override void Tick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Finish()
        {
            throw new System.NotImplementedException();
        }
    }
}