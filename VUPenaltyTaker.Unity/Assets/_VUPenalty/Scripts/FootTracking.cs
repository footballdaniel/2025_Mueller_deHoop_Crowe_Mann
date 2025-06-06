using System;
using UnityEngine;

public class FootTracking : MonoBehaviour
{
    PlayerInputActions _input;

    void OnEnable()
    {
        _input = new PlayerInputActions();
        _input.Foot.Enable();
    }


    void Update()
    {
        transform.position = _input.Foot.Position.ReadValue<Vector3>();
        transform.rotation = _input.Foot.Rotation.ReadValue<Quaternion>();

        Debug.Log(_input.Foot.Position.ReadValue<Vector3>());
    }

    void OnDisable()
    {
        _input.Foot.Disable();
    }
}