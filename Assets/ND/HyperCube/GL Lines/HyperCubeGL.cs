using HyperCube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCubeGL : HyperCubeBase
{

    private void Update()
    {
        base.Update();
        SendToLineRenderer();
    }

    void SendToLineRenderer()
    {
        foreach (IndicesGroup iGroup in root.indicesGroups)
        {
            Vector3[] linePositions = new Vector3[iGroup.indices.Length];
            for (int i = 0; i < iGroup.indices.Length; i++)
            {
                int index = iGroup.indices[i];
                linePositions[i] = vertices[index] + transform.position;
            }
            CameraLineRenderer.Instance.RenderLines(linePositions, data.gradient, data.material);
        }
    }

}
