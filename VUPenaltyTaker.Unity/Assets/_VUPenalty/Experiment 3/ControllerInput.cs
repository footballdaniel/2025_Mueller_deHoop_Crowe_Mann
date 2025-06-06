using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInput : MonoBehaviour
{
    [SerializeField] InputActionReference _selectAction;
    [SerializeField] InputActionReference _touchpadAction;
    [SerializeField] float _offset;

    public float Offset => _offset;
    public event Action TriggerDown;

    void OnEnable()
    {
        _selectAction.action.Enable();
        _touchpadAction.action.Enable();
    }

    void OnDisable()
    {
        _selectAction.action.Disable();
        _touchpadAction.action.Disable();
    }

    void Update()
    {
        if (_selectAction.action.triggered) TriggerDown?.Invoke();

        var inputValue = _touchpadAction.action.ReadValue<Vector2>().x * Time.deltaTime;
        _offset = Mathf.MoveTowards(_offset, inputValue, 0.1f);
    }
}