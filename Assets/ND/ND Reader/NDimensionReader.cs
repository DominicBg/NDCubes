using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NDimensionReader : MonoBehaviour {

    public abstract Vector3 NDtoVector3(float[] values);
}
