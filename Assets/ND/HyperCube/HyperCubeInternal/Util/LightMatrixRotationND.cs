using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{ 
    public class LightMatrixRotationND
    {
        public static LightMatrix Rotation(float angle, int dimension, int rotateDim1, int rotateDim2)
        {
            if (rotateDim1 >= dimension || rotateDim2 >= dimension)
                throw new System.Exception("You tried to rotate in a dimension higher than the cube");

            double cos = Mathf.Cos(angle);
            double sin = Mathf.Sin(angle);

            double[,] matrix = new double[dimension, dimension];
            //Diagonal de 1
            for (int i = 0; i < dimension; i++)
            {
                matrix[i, i] = 1;
            }
            matrix[rotateDim1, rotateDim1] = cos;
            matrix[rotateDim1, rotateDim2] = -sin;
            matrix[rotateDim2, rotateDim2] = cos;
            matrix[rotateDim2, rotateDim1] = sin;

            return new LightMatrix(matrix);
        }
    }
}