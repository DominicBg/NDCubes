using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{
    [System.Serializable]
    public class HyperCubeData
    {
        public int dimension = 4;
        public Gradient gradient;
        public Material material;
        public float lineSize = .1f;

        [Header("Advanced")]
        public bool preRender;
        public bool randomiseAfterRotation;
        public int randomiseHowManyAxis = 2;
    }
}
