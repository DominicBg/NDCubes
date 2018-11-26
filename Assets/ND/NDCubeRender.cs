using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDCubeRender : NDCube {

    [SerializeField] int frameRate;
    List<Vector3[]> verticesPerAngle = new List<Vector3[]>();
    Vector3[] currentVerticies;
    int index = 0;
    float timer = 0;

    float step = 0;

    // Update is called once per frame
    [ContextMenu("Calculate")]
    public void Calculate ()
    {
        StartGeneration();

        float angle = 0;
        step = 1 / (float)frameRate;

		while(angle < 2 * Mathf.PI)
        {
            angle += step;
            Vector3[] rotatexVertices = RotateMatrix(angle);
            verticesPerAngle.Add(rotatexVertices);
        }
	}

    private void Update()
    {
        if (verticesPerAngle.Count == 0)
            return;

        timer += Time.deltaTime * speed;
        while(timer > step)
        {
            timer -= step;
            index = (index + 1) % verticesPerAngle.Count;
        }

        rotatedVertices = CalculateLerpedValue(index, timer/step);
        UpdateLineGroups();
    }

    Vector3[] CalculateLerpedValue(int currentIndex, float t)
    {
        int nextIndex = (index + 1) % verticesPerAngle.Count;
        Vector3[] lerpedValues = new Vector3[verticesPerAngle.Count];

        Vector3[] currents = verticesPerAngle[currentIndex];
        Vector3[] nexts = verticesPerAngle[nextIndex];

        for(int i = 0; i < currents.Length; i++)
        {  
            lerpedValues[i] =  Vector3.Lerp(currents[i], nexts[i], t);
        }
        return lerpedValues;
    }
}
