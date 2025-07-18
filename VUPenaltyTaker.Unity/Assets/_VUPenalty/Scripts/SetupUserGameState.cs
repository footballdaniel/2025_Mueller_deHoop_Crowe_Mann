﻿using UnityEngine;

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

            _context.UI.OnCalibratePress += OnCalibratePressed;
            
            var userGameObject = Object.Instantiate(_context.UserPrefab);
            _context.ActiveUser = userGameObject.GetComponent<User>();
            _context.ActiveUser.Use(_context.Foot);

        }

        void OnCalibratePressed()
        {
            _context.ActiveUser.Calibrate(_context.Foot);
            _context.ChangeState(new RunAllTrialsState(_context));
        }

        public override void Tick(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Skip setup");
                OnCalibratePressed();
            }
        }

        public override void Finish()
        {
            _context.UI.OnCalibratePress -= OnCalibratePressed;
        }
    }
}