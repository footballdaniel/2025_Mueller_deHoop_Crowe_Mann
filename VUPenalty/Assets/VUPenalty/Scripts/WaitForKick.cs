using UnityEngine;

namespace VUPenalty
{
    public class WaitForKick : ExperimentState
    {
        TimeToIntercept1D _timeToIntercept;

        public WaitForKick(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            Debug.Log("Wait for kick");
            _timeToIntercept = new TimeToIntercept1D();
            _timeToIntercept.From(_context.User.Head);
            _timeToIntercept.To(_context.Ball.transform);

            _context.Ball.OnKick += OnKicked;
        }

        void OnKicked(KickStartEvent obj)
        {
            Debug.Log("Has Kicked");
            _context.DataRecorder.KickStart = obj;
            _context.ChangeState(new WaitForBallToLand(_context));
        }

        public override void Tick(float deltaTime)
        {
            var timeToKick = _timeToIntercept.Estimate();

            if (timeToKick > 0)
            {
                if (timeToKick < _context.ActiveTrial.GoalkeeperStartBeforeKick & !_hasGoalkeeperAlreadyDived)
                {
                    _context.Goalkeeper.Dive();
                }

                if (timeToKick < _context.ActiveTrial.AdvertisementStartBeforeKick & !_hasVideoAlreadyStarted)
                {
                    _context.VideoDisplay.Play();
                }
            }
        }
        
        public override void Finish()
        {
            _context.Ball.OnKick -= OnKicked;
        }

        bool _hasGoalkeeperAlreadyDived;
        bool _hasVideoAlreadyStarted;
    }
}