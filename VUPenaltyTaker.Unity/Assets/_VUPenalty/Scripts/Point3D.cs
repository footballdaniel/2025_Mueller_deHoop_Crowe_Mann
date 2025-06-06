using System;
using UnityEngine;

namespace VUPenalty
{
    [Serializable]
    public class Point3D
    {
        public float X;
        public float Y;
        public float Z;

        public static implicit operator Point3D(Vector3 input) =>
            new Point3D()
            {
                X = input.x,
                Y = input.y,
                Z = input.z
            };
    }
}