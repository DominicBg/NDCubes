using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{
    public abstract class HyperCubeBase : MonoBehaviour
    {
        [Header("Data")]
        public HyperCubeData data;
        public RotationData rotatorData;

        public HyperCubeRotator rotator { get; protected set; }
        public HyperCubeRotatorPreRenderer preRenderRotator { get; protected set; }

        protected RotationRandomiser rotationRandomiser;

        protected Node root;
        protected Vector3[] vertices;

        protected void Start()
        {
            AddDimensionReaderIfNull();

            root = HyperCubeVertexGenerator.GenerateVertices(data.dimension);
            HyperCubeEdgeGenerator.GenerateLineIndices(root);

            InitializeComponents();

            rotator.Start(root);
            preRenderRotator.Start(root);
            preRenderRotator.Calculate(root);
        }

        void AddDimensionReaderIfNull()
        {
            if(!gameObject.GetComponent<NDimensionReader>())
            {
                Debug.Log("No NDimensionReader was on the component, default one added");
                NDimensionReader_Basic dimensionReader = gameObject.AddComponent<NDimensionReader_Basic>();
                rotatorData.dimensionReader = dimensionReader;
            }
            else if(rotatorData.dimensionReader == null)
            {
                rotatorData.dimensionReader = gameObject.GetComponent<NDimensionReader>();
            }
        }

        void InitializeComponents()
        {
            rotator = new HyperCubeRotator(rotatorData, data);
            preRenderRotator = new HyperCubeRotatorPreRenderer(rotatorData, data);
            rotationRandomiser = new RotationRandomiser(rotatorData, data);
        }

        protected void Update()
        {
            if (data.preRender)
                vertices = preRenderRotator.Rotate(root);
            else
                vertices = rotator.Rotate(root);
        }

        [System.Serializable]
        public class Node
        {
            public Node(int dimension)
            {
                this.dimension = dimension;
            }

            public Node left;
            public Node right;
            public int dimension;
            public List<IndicesGroup> indicesGroups = new List<IndicesGroup>();
            public List<VectorN> vertices = new List<VectorN>();
        }

        public class IndicesGroup
        {
            public int[] indices;
        }
    }
}