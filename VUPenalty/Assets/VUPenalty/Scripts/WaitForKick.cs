using UnityEngine;

namespace VUPenalty
{
    public class WaitForKick : ExperimentState
    {
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

        public override void Tick(float deltaTime)
        {
            _timeToIntercept.Tick(deltaTime);
            var timeToKick = _timeToIntercept.Estimate();
            var predictionGoalkeeper = _timeToIntercept.Prediction(_context.ActiveTrial.GoalkeeperStartBeforeKick);
            var predictionAdvertisement =
                _timeToIntercept.Prediction(_context.ActiveTrial.AdvertisementStartBeforeKick);

            var goalkeeperPrediction = new Vector3(_context.User.Head.transform.position.x, 0,
                _context.User.Head.transform.position.z + predictionGoalkeeper);
            var advertisementPrediction = new Vector3(_context.User.Head.transform.position.x, 0,
                _context.User.Head.transform.position.z + predictionAdvertisement);


            if (_context.PredictionKeeper.gameObject != null)
                _context.PredictionKeeper.transform.position = goalkeeperPrediction;

            if (_context.PredictionAdvertisement.gameObject != null)
                _context.PredictionAdvertisement.transform.position = advertisementPrediction;

            if ((goalkeeperPrediction.z >= 0) & !_hasGoalkeeperAlreadyDived) _context.Goalkeeper.Dive();

            if ((advertisementPrediction.z >= 0) & !_hasVideoAlreadyStarted) 
                _context.VideoDisplay.Play();

            // Should video already play? Override the prediction if its long
            if (_context.ActiveTrial.AdvertisementStartBeforeKick >= 3f && ! _hasVideoAlreadyStarted)
            {
                _context.VideoDisplay.Play();
            }
        }

        public override void Finish()
        {
            _context.Ball.OnKick -= OnKicked;
        }
    
        void OnKicked(KickStartEvent obj)
        {
            Debug.Log("Has Kicked");
            _context.DataRecorder.KickStart = obj;
            _context.ChangeState(new WaitForBallToLand(_context));
        }

        TimeToIntercept1D _timeToIntercept;

        bool _hasGoalkeeperAlreadyDived;
        bool _hasVideoAlreadyStarted;
    }
}