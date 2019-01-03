using System.Collections;
using System.Collections.Generic;
using static HyperCube.HyperCubeBase;

namespace HyperCube
{
    public static class HyperCubeEdgeGenerator
    {

        //start dim n
        //recursive generate until 2
        //if n == 2
        //  generate square
        //else 
        //  collect left, right
        //  add offset of left.size to everyright
        //  connect every left to right (i + left.size);

        public static List<IndicesGroup> GenerateLineIndices(Node root)
        {
            if (root.dimension == 2)
            {
                root.indicesGroups.Add(GenerateSquare());
                return root.indicesGroups;
            }
            else
            {
                //Recursive
                root.left.indicesGroups = GenerateLineIndices(root.left);
                root.right.indicesGroups = GenerateLineIndices(root.right);
                int offset = root.right.vertices.Count;

                //Collect from lower dimension
                AddLeftIndices(root, root.left);
                AddRightIndices(root, root.right, offset);

                Link2LowerDimensions(root);
                return root.indicesGroups;
            }
        }

        static void AddLeftIndices(Node root, Node left)
        {
            root.indicesGroups.AddRange(left.indicesGroups);
        }

        static void AddRightIndices(Node root, Node right, int offset)
        {
            foreach (IndicesGroup indicesGroup in right.indicesGroups)
            {
                for (int i = 0; i < indicesGroup.indices.Length; i++)
                {
                    indicesGroup.indices[i] += offset;
                }
            }
            root.indicesGroups.AddRange(right.indicesGroups);
        }

        static IndicesGroup GenerateSquare()
        {
            IndicesGroup indicesGroup = new IndicesGroup();
            indicesGroup.indices = new int[5];
            for (int i = 0; i < 4; i++)
            {
                indicesGroup.indices[i] = i;
            }
            indicesGroup.indices[4] = 0;
            return indicesGroup;
        }

        static void Link2LowerDimensions(Node dim)
        {
            //Create connections and link both dimensions together

            int min1Length = dim.left.vertices.Count;
            for (int i = 0; i < min1Length; i++)
            {
                //color
                IndicesGroup indicesGroup = new IndicesGroup();
                indicesGroup.indices = new int[2];
                indicesGroup.indices[0] = i;
                indicesGroup.indices[1] = i + min1Length;

                dim.indicesGroups.Add(indicesGroup);
            }
        }
    }
}