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
            _timeToIntercept.Tick(deltaTime);
            var timeToKick = _timeToIntercept.Estimate();
            
            Debug.Log($"{_timeToIntercept.Prediction(1f)} and {_timeToIntercept.Estimate()}");

            if (_context.InterceptSphere.gameObject != null)
            {
                var prediction = _timeToIntercept.Prediction(_context.ActiveTrial.GoalkeeperStartBeforeKick);
                _context.InterceptSphere.transform.position =
                    new Vector3(_context.User.Head.transform.position.x, 0, _context.User.Head.transform.position.z + prediction);
            }

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