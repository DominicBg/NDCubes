using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HyperCube
{
    public class LightMatrix
    {
        public int rows;
        public int cols;
        public double[,] mat;

        public LightMatrix(int iRows, int iCols)       
        {
            rows = iRows;
            cols = iCols;
            mat = new double[rows, cols];
        }

        public LightMatrix(double[,] arrayMatrix)
        {
            rows = arrayMatrix.GetLength(0);
            cols = arrayMatrix.GetLength(1);
            mat = arrayMatrix;
        }

        public LightMatrix(UnityEngine.Vector3 vector3)
        {
            rows = 1;
            cols = 3;
            mat = new double[rows, cols];
            mat[0, 0] = vector3.x;
            mat[0, 1] = vector3.y;
            mat[0, 2] = vector3.z;

        }
        public LightMatrix(UnityEngine.Vector3[] vector3Array)
        {
            rows = vector3Array.Length;
            cols = 3;
            mat = new double[rows, cols];

            for (int i = 0; i < vector3Array.Length; i++)
            {
                mat[i, 0] = vector3Array[i].x;
                mat[i, 1] = vector3Array[i].y;
                mat[i, 2] = vector3Array[i].z;
            }
        }

        public double this[int iRow, int iCol]
        {
            get { return mat[iRow, iCol]; }
            set { mat[iRow, iCol] = value; }
        }

        public UnityEngine.Vector3 GetRowVector3(int row)
        {
            return new UnityEngine.Vector3((float)mat[row, 0], (float)mat[row, 1], (float)mat[row, 2]);
        }

        public float[] GetRow(int row)
        {
            float[] rowValues = new float[cols];
            for (int i = 0; i < cols; i++)
            {
                rowValues[i] = (float)mat[row, i];
            }
            return rowValues;
        }

        public void SetRow(int row, float[] values)
        {
            for (int i = 0; i < cols; i++)
            {
                mat[row, i] = values[i];
            }
        }

        public LightMatrix Duplicate()
        {
            LightMatrix matrix = new LightMatrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = mat[i, j];
            return matrix;
        }

        public static LightMatrix Multiply(LightMatrix m1, LightMatrix m2)
        {
            if (m1.cols != m2.rows) throw new System.Exception("Wrong dimensions of matrix!");

            LightMatrix result = new LightMatrix(m1.rows, m2.cols);
            for (int i = 0; i < result.rows; i++)
                for (int j = 0; j < result.cols; j++)
                    for (int k = 0; k < m1.cols; k++)
                        result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }

        public static LightMatrix operator *(LightMatrix m1, LightMatrix m2)
        {
            return LightMatrix.Multiply(m1, m2);
        }
    }
}