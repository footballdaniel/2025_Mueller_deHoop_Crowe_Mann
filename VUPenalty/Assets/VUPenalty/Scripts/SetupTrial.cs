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

            var dataRecorderGO = GameObject.Instantiate(_context.DataRecorderPrefab);
            _context.DataRecorder = dataRecorderGO.GetComponent<DataRecorder>();
            _context.DataRecorder.Target = _context.Foot.transform;

            var ballGo = Object.Instantiate(_context.BallPrefab, new Vector3(0f, 0.15f, 0f), Quaternion.identity);
            _context.Ball = ballGo.GetComponent<Ball>();

            var _goalkeeperGO = Object.Instantiate(_context.GoalkeeperPrefab);
            _context.Goalkeeper = _goalkeeperGO.GetComponent<Goalkeeper>();
            _context.Goalkeeper.JumpDirection = _context.ActiveTrial.JumpDirection;

            _context.VideoDisplay.LoadVideo(_context.ActiveTrial.Video);
            _context.VideoDisplay.SetSize(_context.Experiment.VideoWidth, _context.Experiment.VideoHeight);

            _context.ChangeState(new WaitForKick(_context));
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }
    }
}