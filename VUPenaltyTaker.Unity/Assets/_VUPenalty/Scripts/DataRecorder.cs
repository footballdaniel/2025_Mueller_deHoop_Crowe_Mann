using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VUPenalty
{
    public class DataRecorder : MonoBehaviour
    {
        public KickEndEvent KickEnd;
        public KickStartEvent KickStart;
        public float BufferWindow = 2f;
        public Transform Target;
        public List<Point3D> TimeSeries => _bufferPoints.ToList();

        public KeeperDiveData GetDiveData()
        {
            if (KickStart == null)
                Debug.LogError("Could not export dive data, kick has not occured");

            var keeperDivedBeforeBallKickInSeconds = Time.timeSinceLevelLoad - _currentTimeAtDive;
            return new KeeperDiveData(keeperDivedBeforeBallKickInSeconds);
        }

        public void OnKeeperDived(KeeperDiveEvent obj)
        {
            _currentTimeAtDive = Time.timeSinceLevelLoad;
        }

        void Awake()
        {
            _bufferPoints = new Queue<Point3D>();
            _bufferTime = new Queue<float>();
        }

        void FixedUpdate()
        {
            var secondsOfStoredPositions = _bufferPoints.Count * Time.fixedDeltaTime;

            if (secondsOfStoredPositions > BufferWindow)
            {
                _bufferPoints.Dequeue();
                _bufferTime.Dequeue();
            }

            _bufferPoints.Enqueue(Target.position);
            _bufferTime.Enqueue(Time.timeSinceLevelLoad);
        }


        Queue<Point3D> _bufferPoints;
        Queue<float> _bufferTime;
        float _currentTimeAtDive;
    }
}