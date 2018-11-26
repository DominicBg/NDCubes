using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDimensionReader_Basic : NDimensionReader
{
    [SerializeField] protected float distance = 2;
    [SerializeField] protected float growth = 1;

    public override Vector3 NDtoVector3(float[] values)
    {
        Vector3 vector3 = new Vector3(values[0], values[1], values[2]);
        if (values.Length > 3)
        {
            vector3 *= 1 / (growth - values[3]);
        }
        if (values.Length > 4)
        {
            vector3 += Vector3.up * values[4] * distance;
        }
        if (values.Length > 5)
        {
            vector3 += Vector3.right * values[5] * distance;
        }
        if (values.Length > 6)
        {
            vector3 += Vector3.forward * values[6] * distance;
        }
        if (values.Length > 7)
        {
            vector3 *= 1 / (growth - values[7]);
        }
        if (values.Length > 8)
        {
            vector3 *= 1 / (growth - values[8]);
        }
        if (values.Length > 9)
        {
            vector3 *= 1 / (growth - values[9]);
        }

        return vector3;
    }
}
