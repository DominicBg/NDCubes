using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDLineRendererController : MonoBehaviour {

    NDCube ndCube;

    [SerializeField] KeyCode keyCodeShake;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    [SerializeField] float perlinSpeed;

    private void Start()
    {
        ndCube = GetComponent<NDCube>();
    }

    private void Update()
    {
        ChangeLineRendererDimension();
    }

    private void ChangeLineRendererDimension()
    {
        int dimension = GetDimensionKey();

        if (dimension == -1)
            return;

        ToggleDimension(ndCube, dimension);
    }

    void ToggleDimension(NDCube currentCube, int dimension)
    {
        if(currentCube.dimension == dimension)
        {
            bool isActive = currentCube.ConnectionNull.activeInHierarchy;
            currentCube.ConnectionNull.SetActive(!isActive);
        }
        else
        {
            ToggleDimension(currentCube.min1DimensionCubeLeft, dimension);
            ToggleDimension(currentCube.min1DimensionCubeRight, dimension);
        }
    }

    int GetDimensionKey()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            return 3;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            return 4;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            return 5;
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            return 6;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            return 7;
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            return 8;
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            return 9;
        }
        return -1;
    }
}