using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCube
{ 
    public class RotationRandomiser
    {
        RotationData rotatorData;
        HyperCubeData data;
        List<Vector2Int> permutationList;

        public RotationRandomiser(RotationData rotatorData, HyperCubeData data)
        {
            this.rotatorData = rotatorData;
            this.data = data;

            permutationList = new List<Vector2Int>();
            GeneratePermutation(data.dimension, permutationList);
            SubscribeToRandomiseEvent();
        }

        void GeneratePermutation(int currentDimension, List<Vector2Int> permutations)
        {
            if (currentDimension < 0)
                return;

            for (int i = currentDimension + 1; i < data.dimension; i++)
            {
                Vector2Int newRotation = new Vector2Int(currentDimension, i);
                permutations.Add(newRotation);
            }
            GeneratePermutation(currentDimension - 1, permutations);
        }

        void SubscribeToRandomiseEvent()
        {
            rotatorData.onFullRotation.AddListener(TryRandomise);
        }

        void TryRandomise()
        {
            if (data.randomiseAfterRotation)
                RandomiseRotation(Random.Range(1, data.randomiseHowManyAxis));
        }

        public void RandomiseRotation(int howManyRotations)
        {
            permutationList = Randomize(permutationList);
            Vector2Int[] listRotationDimension = new Vector2Int[howManyRotations];

            for (int i = 0; i < howManyRotations; i++)
            {
                listRotationDimension[i] = permutationList[i];
            }
            rotatorData.listRotationDimension = listRotationDimension;
        }

        public List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            System.Random rnd = new System.Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count);
                randomizedList.Add(list[index]);
                list.RemoveAt(index);
            }
            return randomizedList;
        }
    }
}
