using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLineRenderer : MonoBehaviour {

    public static CameraLineRenderer Instance
    {
        get
        {
            if (Instance == null)
            {
                Debug.Log("Added CameraLineRenderer to Main Camera");
                Instance = Camera.main.gameObject.AddComponent<CameraLineRenderer>();
            }
            
            return Instance;
        }
        private set
        {
        }
    }

    List<LineGroup> lineGroups = new List<LineGroup>();
    [SerializeField] Material defaultMaterial;

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnPostRender()
    {
        //DrawDebug();
        RenderLineGroups();
    }
    private void OnDrawGizmos()
    {
        //DrawDebug();
        RenderLineGroups();
    }

    void DrawDebug()
    {
        Vector3[] linePositions = new Vector3[5];
        linePositions[0] = new Vector3(-0.5f, -0.5f, 0);
        linePositions[1] = new Vector3(-0.5f, 0.5f, 0);
        linePositions[2] = new Vector3(0.5f, 0.5f, 0);
        linePositions[3] = new Vector3(0.5f, -0.5f, 0);
        linePositions[4] = new Vector3(-0.5f, -0.5f, 0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.white);
        for (int i = 0; i < linePositions.Length; i++)
        {
            GL.Vertex(linePositions[i] * 2);
            
        }
        GL.End();
        GL.Flush();
    }


    void RenderLineGroups()
    {
        foreach (LineGroup lineGroup in lineGroups)
        {
            RenderLineGroup(lineGroup);
        }
        lineGroups.Clear();
    }

    void RenderLineGroup(LineGroup lineGroup)
    {
        GL.Begin(GL.LINE_STRIP);

        if (lineGroup.material != null)
            lineGroup.material.SetPass(0);
        else
            defaultMaterial.SetPass(0);

        for (int i = 0; i < lineGroup.vertices.Length; i++)
        {
            if (lineGroup.gradient == null)
            {
                GL.Color(Color.cyan);
            }
            else
            {
                float t = (float)i / (lineGroup.vertices.Length-1);
                GL.Color(lineGroup.gradient.Evaluate(t));
            }
            GL.Vertex(lineGroup.vertices[i]);
        }
        GL.End();
        GL.Flush();
    }

    public void RenderLines(Vector3[] vertices, Gradient gradient, Material material)
    {
        lineGroups.Add(new LineGroup(vertices, gradient, material));
    }

    public class LineGroup
    {
        public Vector3[] vertices;
        public Gradient gradient;
        public Material material;

        public LineGroup(Vector3[] vertices, Gradient gradient, Material material)
        {
            this.vertices = vertices;
            this.gradient = gradient;
            this.material = material;
        }
    }
}
