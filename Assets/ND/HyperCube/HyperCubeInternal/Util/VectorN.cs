using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{
    [System.Serializable]
    public struct VectorN
    {
        List<float> vector;

        public VectorN(int dimension)
        {
            vector = new List<float>(dimension);
        }

        public VectorN(float[] vector)
        {
            this.vector = new List<float>(vector);
        }

        public VectorN(List<float> vector)
        {
            this.vector = vector;
        }

        public VectorN(VectorN vectorN)
        {
            vector = new List<float>();
            for (int i = 0; i < vectorN.dimensions; i++)
            {
                float value = vectorN[i];
                vector.Add(value);
            }
        }

        public Vector2 ToVector2()
        {
            return new Vector3(vector[0], vector[1], 0);
        }

        public void AddDimension(float newValue)
        {
            vector.Add(newValue);
        }

        public float[] GetValues()
        {
            return vector.ToArray();
        }

        public float dimensions { get { return vector.Count; } }

        public float this[int i]
        {
            get { return vector[i]; }
            set { vector[i] = value; }
        }
    }
}