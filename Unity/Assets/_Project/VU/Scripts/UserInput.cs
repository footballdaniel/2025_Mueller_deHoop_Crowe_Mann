using UnityEngine;

namespace VU.Scripts
{
    public class UserInput : MonoBehaviour
    {
        InputActions _inputActions;

        void Awake()
        {
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
        }

        public bool HasKicked() => _inputActions.Player.Kick.WasPerformedThisFrame();
    }
}