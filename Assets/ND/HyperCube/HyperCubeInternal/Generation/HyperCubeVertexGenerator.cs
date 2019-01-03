using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HyperCube.HyperCubeBase;

namespace HyperCube
{
    public class HyperCubeVertexGenerator
    {
        //Generate the tree structure of hypercubes
        //if dim = 2
        //  generate a square
        //else
        //  duplicate the vertices lower dimension
        //  add a new dimensions to every vertices

        public static Node GenerateVertices(int dimension)
        {
            Node root = new Node(dimension);

            if (root.dimension == 2)
            {
                GenerateSquare(root.vertices);
            }
            else
            {
                GenerateLowerDimensions(root, dimension);
            }
            return root;
        }

        static void GenerateLowerDimensions(Node root, int dimension)
        {
            root.left = GenerateVertices(dimension - 1);
            root.right = GenerateVertices(dimension - 1);

            AddLowerDimensionVertices(root, root.left, -0.5f);
            AddLowerDimensionVertices(root, root.right, 0.5f);
        }

        static void AddLowerDimensionVertices(Node root, Node child, float newValue)
        {
            //Copy vertices from lower dimension, and add the newValue at the end of the vertex
            //if lower dimension = 2,  copy x y, and add z as newValue;

            for (int i = 0; i < child.vertices.Count; i++)
            {
                VectorN newDimensionVertex = new VectorN(child.vertices[i]);
                newDimensionVertex.AddDimension(newValue);

                root.vertices.Add(newDimensionVertex);
            }
        }

        static void GenerateSquare(List<VectorN> vertices)
        {
            float[] v1 = { -0.5f, -0.5f };
            float[] v2 = { -0.5f, 0.5f };
            float[] v3 = { 0.5f, 0.5f };
            float[] v4 = { 0.5f, -0.5f };

            vertices.Add(new VectorN(v1));
            vertices.Add(new VectorN(v2));
            vertices.Add(new VectorN(v3));
            vertices.Add(new VectorN(v4));
        }
    }
}