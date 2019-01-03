using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTester : MonoBehaviour {

    [SerializeField] Vector3[] positions;
    [SerializeField] Material mat;

    // Update is called once per frame
    void Update () {
        CameraLineRenderer.Instance.RenderLines(positions, null, mat);
	}
}
