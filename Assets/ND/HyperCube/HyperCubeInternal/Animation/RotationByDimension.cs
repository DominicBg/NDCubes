using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{
    [System.Serializable]
    public class RotationByDimension
    {
        public int dimension;
        public Vector2Int rotationAroundDimension;
        public float angle;
        public float speed;

        public void Update()
        {
            angle += Time.deltaTime * speed;
        }

        public LightMatrix RotatePartialMatrix(LightMatrix matrix)
        {
            //Concept 
            /* On rotate une matrice par l'angle et les dimensions
             * Si on rotate par la 2ieme dimension, il y a 4 vertex
             * Donc on alterne en 4 vertex de la  matrice orinal et de la rotated
             * 
             * Si on rotate par la 3ième, il y a 8 vertex.. même chose
             * Si on rotate par la Nième dimension, il y a (2 ^ n) vertex
             */

            LightMatrix partialRotatedMatrix = new LightMatrix(matrix.rows, matrix.cols);
            LightMatrix rotationMatrix = LightMatrixRotationND.Rotation(angle * Mathf.Deg2Rad, matrix.cols, rotationAroundDimension.x, rotationAroundDimension.y);

            LightMatrix fullyRotatedMatrix = LightMatrix.Multiply(matrix, rotationMatrix);

            int twoPowerOfDimension = (int)Mathf.Pow(2, dimension);
            bool takeFromOriginal = true;
            int internalCounter = 0;
            for (int i = 0; i < matrix.rows; i++)
            {
                if (takeFromOriginal)
                {
                    partialRotatedMatrix.SetRow(i, matrix.GetRow(i));
                }
                else
                {
                    partialRotatedMatrix.SetRow(i, fullyRotatedMatrix.GetRow(i));
                }

                internalCounter++;
                if (internalCounter == twoPowerOfDimension)
                {
                    internalCounter = 0;
                    takeFromOriginal = !takeFromOriginal;
                }
            }
            return partialRotatedMatrix;
        }
    }
}