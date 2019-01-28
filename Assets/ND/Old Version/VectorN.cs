using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //public Vector3 ToVector3()
    //{
    //    return NDtoVector3(vector.ToArray());
    //}

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

    //public static Vector3 NDtoVector3(float[] values)
    //{
    //    Vector3 vector3 = new Vector3(values[0], values[1], values[2]);
    //    if (values.Length > 3)
    //    {
    //        vector3 *= 1 / (1-values[3]);
    //    }
    //    if (values.Length > 4)
    //    {
    //        vector3 += Vector3.up * values[4] * 2;
    //    }
    //    if (values.Length > 5)
    //    {
    //        vector3 += Vector3.right * values[5] * 2;
    //    }
    //    if (values.Length > 6)
    //    {
    //        vector3 += Vector3.forward * values[6] * 2;
    //    }
    //    if (values.Length > 7)
    //    {
    //        vector3 *= 1 / (1 - values[7]);
    //    }
    //    if (values.Length > 8)
    //    {
    //        vector3 *= 1 / (1 - values[8]);
    //    }
    //    if (values.Length > 9)
    //    {
    //        vector3 *= 1 / (1 - values[9]);
    //    }

    //    if (vector3.magnitude > 1)
    //        vector3 = vector3.normalized;

    //    return vector3;
    //}

}
