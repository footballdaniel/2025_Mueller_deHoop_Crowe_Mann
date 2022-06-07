using UnityEngine;

namespace VUPenalty
{
    public class WaitForGetInPosition : ExperimentState
    {
        StartArea _startArea;
        GameObject _startAreaGO;

        public WaitForGetInPosition(ExperimentController controller) : base(controller)
        {
        }

        public override void Init()
        {
            _startAreaGO = GameObject.Instantiate(_context.StartAreaPrefab);
            _startArea = _startAreaGO.GetComponent<StartArea>();
        }

        public override void Tick(float deltaTime)
        {
            if (_startArea.IsObserverInStartArea)
                _context.ChangeState(new WaitForKick(_context));
        }

        public override void Finish()
        {
            GameObject.Destroy(_startAreaGO);
        }
    }
}