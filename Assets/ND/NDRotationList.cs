using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDRotationList : MonoBehaviour {

    [SerializeField] Rotation[] listOfRotations; // = new List<List<Vector2Int>>();
    NDCube ndCube;

    int index = 0;

    private void Start()
    {
        ndCube = GetComponent<NDCube>();
    }

    public void SetNextRotation()
    {
        //List<Vector2Int> currentListInt = listOfRotations[index];
        Vector2Int[] listRotationDimension = listOfRotations[index].rotationAxis;

        ndCube.listRotationDimension = listRotationDimension;
        index = (index + 1) % listOfRotations.Length;
    }

    [System.Serializable]
    public class Rotation
    {
        public Vector2Int[] rotationAxis;
    }
}
