using UnityEngine;

namespace VUPenalty
{
    public class SetupTrial : ExperimentState
    {
        public SetupTrial(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            Debug.Log("Setup trial");

            _context.TrialGameObject = new GameObject("Trial Root Object");

            var _goalkeeperGO = Object.Instantiate(_context.GoalkeeperPrefab,_context.TrialGameObject.transform);
            _goalkeeperGO.transform.position = new Vector3(_context.ActiveTrial.GoalKeeperDisplacement, 0, 0);
            _context.Goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();
            _context.Goalkeeper._direction = _context.ActiveTrial.JumpDirection;

            var dataRecorderGO =
                GameObject.Instantiate(_context.DataRecorderPrefab, _context.TrialGameObject.transform);
            _context.DataRecorder = dataRecorderGO.GetComponent<DataRecorder>();
            _context.DataRecorder.Target = _context.Foot.transform;

            var ballGo = Object.Instantiate(_context.BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity,
                _context.TrialGameObject.transform);
            _context.Ball = ballGo.GetComponent<Ball>();
            _context.Ball.ElasticityMultiplier = _context.ExperimentalData.BallElasticity;


            _context.VideoDisplay.LoadVideo(_context.ActiveTrial.Video);
            _context.VideoDisplay.SetSize(_context.ExperimentalData.VideoWidth, _context.ExperimentalData.VideoHeight);

            _context.Goalkeeper.SetGoalkeeperColor(_context.ActiveTrial.GoalKeeperColor);
            
            _context.ChangeState(new WaitForGetInPosition(_context));
        }

        public override void Finish()
        {
        }

        public override void Tick(float deltaTime)
        {
        }
    }
}