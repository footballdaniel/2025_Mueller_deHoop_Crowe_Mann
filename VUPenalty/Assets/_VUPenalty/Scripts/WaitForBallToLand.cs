using System.Collections;
using UnityEngine;

namespace VUPenalty
{
    class WaitForBallToLand : ExperimentState
    {
        public WaitForBallToLand(ExperimentController context) : base(context)
        {
        }
        
        IEnumerator Delay(float duration)
        {
            yield return new WaitForSeconds(duration);
            _context.ChangeState(new SaveState(_context));
        }

        public override void Init()
        {
            Debug.Log("Wait for ball to land");

            _context.TargetAreaMissed.OnHit += Hit;
            _context.TargetAreaSuccess.OnHit += Hit;
            _context.StartCoroutine(Delay(3f));
            
        }

        void Hit(KickEndEvent obj)
        {
            _context.DataRecorder.KickEnd = obj;
        }


        public override void Tick(float deltaTime)
        {
        }

        public override void Finish()
        {
        }
    }
}