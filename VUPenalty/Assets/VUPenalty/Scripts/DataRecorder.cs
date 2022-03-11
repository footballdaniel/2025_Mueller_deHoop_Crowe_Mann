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
        public List<Point3D> TimeSeries => _buffer.ToList();

        void Awake()
        {
            _buffer = new Queue<Point3D>();
        }

        void FixedUpdate()
        {
            var secondsOfStoredPositions = _buffer.Count * Time.fixedDeltaTime;
            if (secondsOfStoredPositions < BufferWindow)
            {
                _buffer.Enqueue(Target.position);
            }
            else
            {
                _buffer.Dequeue();
                _buffer.Enqueue(Target.position);
            }
        }


        Queue<Point3D> _buffer;
    }
}