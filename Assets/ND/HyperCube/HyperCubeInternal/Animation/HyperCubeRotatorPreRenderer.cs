using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HyperCube.HyperCubeBase;

namespace HyperCube
{
    public class HyperCubeRotatorPreRenderer : HyperCubeRotator
    {
        int frameRate = 30;

        List<Vector3[]> verticesPerAngle = new List<Vector3[]>();
        Vector3[] currentVerticies;

        int index = 0;
        float timer = 0;
        float step = 0;

        public HyperCubeRotatorPreRenderer(RotationData rotatorData, HyperCubeData hyperCubeData) : base(rotatorData, hyperCubeData) { }

        public void Calculate(Node root)
        {
            float angle = 0;
            step = 1 / (float)frameRate;

            while (angle < 2 * Mathf.PI)
            {
                angle += step;
                Vector3[] rotatexVertices = RotateMatrix(root, angle);
                verticesPerAngle.Add(rotatexVertices);
            }
        }

        public override Vector3[] Rotate(Node root)
        {
            if (verticesPerAngle.Count == 0)
                return null;

            timer += Time.deltaTime * rotatorData.speed;
            while (timer > step)
            {
                timer -= step;
                index = (index + 1) % verticesPerAngle.Count;
            }

            return CalculateLerpedValue(index, timer / step);
        }

        Vector3[] CalculateLerpedValue(int currentIndex, float t)
        {
            int nextIndex = (index + 1) % verticesPerAngle.Count;
            Vector3[] lerpedValues = new Vector3[verticesPerAngle.Count];

            Vector3[] currents = verticesPerAngle[currentIndex];
            Vector3[] nexts = verticesPerAngle[nextIndex];

            for (int i = 0; i < currents.Length; i++)
            {
                lerpedValues[i] = Vector3.Lerp(currents[i], nexts[i], t);
            }
            return lerpedValues;
        }
    }
}