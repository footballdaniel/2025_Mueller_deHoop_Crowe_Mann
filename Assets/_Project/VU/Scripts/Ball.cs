using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float _sampleWindowSeconds = 0.2f;
    [SerializeField] Rigidbody _rigidBody;

    Vector3 _endPosition;
    bool _hasAlreadyKicked;
    Vector3 _startPosition;
    float _startTime;

    bool NeedsMoreSamples => Time.time < _startTime + _sampleWindowSeconds;

    void OnTriggerEnter(Collider other)
    {
        if (_hasAlreadyKicked) return;
        _startTime = Time.time;
        _startPosition = other.transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        _hasAlreadyKicked = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (NeedsMoreSamples | _hasAlreadyKicked) return;
        _endPosition = other.transform.position;
        _hasAlreadyKicked = true;
        EvaluateKick();
    }

    void EvaluateKick()
    {
        var kickDirection = _endPosition - _startPosition;
        print(kickDirection);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = kickDirection / _sampleWindowSeconds;
    }
}