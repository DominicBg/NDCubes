using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCube
{
    [System.Serializable]
    public class RotationData
    {
        [HideInInspector] public UnityEvent onFullRotation = new UnityEvent();

        [Header("Rotate around what axis? x = 0, y = 1, z = 2.. etc")]
        public Vector2Int[] listRotationDimension = { new Vector2Int(2, 3), new Vector2Int(0,2) };

        [Header("Rotate a single dimenson (Advanced)")]
        public RotationByDimension[] rotationByDimension;

        [Header("AddComponent NDDimensionReader (Optional)")]
        public NDimensionReader dimensionReader;

        [Header("Parameters")]
        public float size = 1;
        public float speed = 1;
    }
}