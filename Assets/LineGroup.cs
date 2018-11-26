using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGroup : MonoBehaviour {

    [SerializeField] bool loopBack;
    public LineRenderer lineRenderer;
    public int[] indexTransform;

    public void ApplyLineToGroup(Vector3[] group)
    {
        int loopBackBonus = ((loopBack) ? 1 : 0);
        Vector3[] positions = new Vector3[indexTransform.Length + loopBackBonus];
        for (int i = 0; i < indexTransform.Length; i++)
        {
            positions[i] = group[indexTransform[i]];
        }
        //Loop back
        if(loopBack)
            positions[indexTransform.Length] = group[indexTransform[0]];

        lineRenderer.positionCount = indexTransform.Length + loopBackBonus;
        lineRenderer.SetPositions(positions);
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

