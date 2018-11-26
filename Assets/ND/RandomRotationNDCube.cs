using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationNDCube : MonoBehaviour {

    [SerializeField] int howManyRotations = 2;
    NDCube ndCube;
    List<Vector2Int> permutationList;

    private void Start()
    {
        ndCube = GetComponent<NDCube>();

        permutationList = new List<Vector2Int>();
        GeneratePermutation(ndCube.dimension, permutationList);
    }

    void GeneratePermutation(int currentDimension, List<Vector2Int> permutations)
    {
        if (currentDimension < 0)
            return;

        for (int i = currentDimension + 1; i < ndCube.dimension; i++)
        {
            Vector2Int newRotation = new Vector2Int(currentDimension, i);
            permutations.Add(newRotation);
        }
        GeneratePermutation(currentDimension - 1, permutations);
    }

    public void RandomiseRotation()
    {
        //permutationList.Shuffle();
        Vector2Int[] listRotationDimension = new Vector2Int[howManyRotations];

        for (int i = 0; i < howManyRotations; i++)
        {
            listRotationDimension[i] = permutationList[i];
        }

        ndCube.listRotationDimension = listRotationDimension;
    }
}
