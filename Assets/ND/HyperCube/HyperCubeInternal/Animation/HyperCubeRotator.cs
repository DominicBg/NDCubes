using UnityEngine;
using UnityEngine.Events;
using static HyperCube.HyperCubeBase;

namespace HyperCube
{
    public class HyperCubeRotator
    {
        protected RotationData rotatorData;
        protected HyperCubeData hyperCubeData;

        float angle = 0;
        float TWOPI = Mathf.PI * 2;
        int minDimension = 2;

        LightMatrix verticesMatrix;
        LightMatrix modifiedVerticesMatrix;

        public HyperCubeRotator(RotationData rotatorData, HyperCubeData hyperCubeData)
        {
            this.rotatorData = rotatorData;
            this.hyperCubeData = hyperCubeData;
        }

        public void Start(Node root)
        {
            CalculateVerticesMatrix(root);
        }

        void CalculateVerticesMatrix(Node root)
        {
            verticesMatrix = new LightMatrix(root.vertices.Count, root.dimension);
            for (int i = 0; i < root.vertices.Count; i++)
            {
                verticesMatrix.SetRow(i, root.vertices[i].GetValues());
            }
            modifiedVerticesMatrix = verticesMatrix.Duplicate();
        }

        public virtual Vector3[] Rotate(Node root)
        {
            if (verticesMatrix == null)
                return null;

            UpdateRotation();

            PartialRotation();

            return RotateMatrix(root, angle);
        }

        //Increase angle until full circle
        private void UpdateRotation()
        {
            angle += Time.deltaTime * rotatorData.speed;
            if (angle > TWOPI)
            {
                angle -= TWOPI;
                rotatorData.onFullRotation.Invoke();
            }
        }

        private void PartialRotation()
        {
            if (verticesMatrix == null)
                return;

            for (int i = 0; i < rotatorData.rotationByDimension.Length; i++)
            {
                LightMatrix previousMatrix = (i == 0) ? verticesMatrix : modifiedVerticesMatrix;
                rotatorData.rotationByDimension[i].Update();
                modifiedVerticesMatrix = rotatorData.rotationByDimension[i].RotatePartialMatrix(previousMatrix);
            }
        }

        protected Vector3[] RotateMatrix(Node root, float angle)
        {
            //Rotate the verticies matrix
            LightMatrix rotatedMatrix = modifiedVerticesMatrix.Duplicate();
            for (int i = 0; i < rotatorData.listRotationDimension.Length; i++)
            {
                LightMatrix rotationMatrix = LightMatrixRotationND.Rotation(
                    angle,
                    root.dimension,
                    rotatorData.listRotationDimension[i].x,
                    rotatorData.listRotationDimension[i].y);

                rotatedMatrix = LightMatrix.Multiply(rotatedMatrix, rotationMatrix);
            }

            Vector3[] rotatedVertices = new Vector3[rotatedMatrix.rows];

            //Convert matrix to 3D coordinate
            for (int i = 0; i < rotatedMatrix.rows; i++)
            {
                Vector3 position = rotatorData.dimensionReader.NDtoVector3(rotatedMatrix.GetRow(i));
                rotatedVertices[i] = position * rotatorData.size;
            }
            return rotatedVertices;
        }
    }
}