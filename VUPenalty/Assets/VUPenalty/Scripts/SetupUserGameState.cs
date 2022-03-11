using System.Collections;
using UnityEngine;

namespace VUPenalty
{
    class SetupUserGameState : GameState
    {
        public SetupUserGameState(Game game) : base(game)
        {
        }

        public override void Init()
        {
            Debug.Log("Setup, wait for foot calibration");
            var userGameObject = Object.Instantiate(_context.UserPrefab);
            _context.ActiveUser = userGameObject.GetComponent<User>();
            _context.ActiveUser.Use(_context.Foot);
            _context.ActiveUser.Calibrate(_context.Foot);
            _context.StartCoroutine(WaitForCalibration(3f));
        }

        public override void Tick(float deltaTime)
        {
        }

        IEnumerator WaitForCalibration(float timeSeconds)
        {
            yield return new WaitForSeconds(timeSeconds);
            _context.ChangeState(new RunAllTrialsState(_context));
        }

        public override void Finish()
        {
        }
    }
}