using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGroup : MonoBehaviour {

    [SerializeField] bool loopBack;
    public LineRenderer lineRenderer;
    public int[] indexTransform;

    public void ApplyLineToGroup(Vector3[] allVertices)
    {
        int loopBackBonus = ((loopBack) ? 1 : 0);

        Vector3[] positions = GetPositions(allVertices);
        lineRenderer.positionCount = indexTransform.Length + loopBackBonus;
        lineRenderer.SetPositions(positions);
    }

    public Vector3[] GetPositions(Vector3[] allVertices)
    {
        int loopBackBonus = ((loopBack) ? 1 : 0);
        Vector3[] positions = new Vector3[indexTransform.Length + loopBackBonus];
        for (int i = 0; i < indexTransform.Length; i++)
        {
            positions[i] = allVertices[indexTransform[i]];
        }
        //Loop back
        if (loopBack)
            positions[indexTransform.Length] = allVertices[indexTransform[0]];

        return positions;
    }

    public void ApplyColor(Color startColor, Color endColor)
    {
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }

    public void ApplyOffsetToIndices(int offset)
    {
        for (int i = 0; i < indexTransform.Length; i++)
        {
            indexTransform[i] += offset;
        }
    }
}

