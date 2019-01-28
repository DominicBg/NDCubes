using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Generate des cubes de dimensions n >= 3


public class NDCube : MonoBehaviour
{
    public List<VectorN> vertices { get; protected set; }
    public List<LineGroup> lineGroups { get; set; }
    public GameObject ConnectionNull { get; private set; }
    public NDCube min1DimensionCubeLeft { get; private set; }
    public NDCube min1DimensionCubeRight { get; private set; }

    public UnityEvent onFullRotation = new UnityEvent();
    public Vector2Int[] listRotationDimension;

    NDCube root;
    Matrix verticesMatrix;
    Matrix modifiedVerticesMatrix;

    protected Vector3[] rotatedVertices;

    [Header("Dimension settings")]
    [SerializeField] protected Gradient gradientOverDimensions;
    [SerializeField] public int dimension;

    [Header("Parameters")]
    [SerializeField] protected float size = 10;
    [SerializeField] protected float speed;
    [SerializeField] protected float lineSize = 1;

    [Header("References")]
    [SerializeField] RotationByDimension[] rotationByDimension;
    [SerializeField] NDimensionReader dimensionReader;
    [SerializeField] LineGroup lineGroupPrefab;

    float angle = 0;
    float TWOPI = Mathf.PI * 2;
    int minDimension = 2;

    [ContextMenu("Generate N Cube")]
    public void StartGeneration()
    {
        Debug.Log("Start Generation");
        root = this;
        GenerateNCube(root);
        CalculateVerticesMatrix();
    }

    private void OnValidate()
    {
        PartialRotation();
    }

    void CalculateVerticesMatrix()
    {
        verticesMatrix = new Matrix(vertices.Count, dimension);
        for (int i = 0; i < vertices.Count; i++)
        {
            verticesMatrix.SetRow(i, vertices[i].GetValues());
        }
        modifiedVerticesMatrix = verticesMatrix.Duplicate();
    }

    private void Update()
    {
        if (verticesMatrix == null)
            return;

        UpdateRotation();

        PartialRotation();

        rotatedVertices = RotateMatrix(angle);

        UpdateLineGroups();
    }

    //Increase angle until full circle
    private void UpdateRotation()
    {
        angle += Time.deltaTime * speed;
        if (angle > TWOPI)
        {
            angle -= TWOPI;
            onFullRotation.Invoke();
        }
    }

    private void PartialRotation()
    {
        if (verticesMatrix == null)
            return;

        for (int i = 0; i < rotationByDimension.Length; i++)
        {
            Matrix previousMatrix = (i == 0) ? verticesMatrix : modifiedVerticesMatrix;
            rotationByDimension[i].Update();
            modifiedVerticesMatrix = rotationByDimension[i].RotatePartialMatrix(previousMatrix);
        }
    }

    protected Vector3[] RotateMatrix(float angle)
    {
        //Rotate the verticies matrix
        Matrix rotatedMatrix = modifiedVerticesMatrix.Duplicate();
        for (int i = 0; i < listRotationDimension.Length; i++)
        {
            Matrix rotationMatrix = MatrixRotationND.Rotation(angle, dimension, listRotationDimension[i].x, listRotationDimension[i].y);
            rotatedMatrix = Matrix.StupidMultiply(rotatedMatrix, rotationMatrix);
        }

        Vector3[] rotatedVertices = new Vector3[rotatedMatrix.rows];

        //Convert matrix to 3D coordinate
        for (int i = 0; i < rotatedMatrix.rows; i++)
        {
            rotatedVertices[i] = dimensionReader.NDtoVector3(rotatedMatrix.GetRow(i)) * size;
        }
        return rotatedVertices;
    }

    public void GenerateNCube(NDCube root)
    {
        this.root = root;
        size = root.size;
        vertices = new List<VectorN>();
        lineGroups = new List<LineGroup>();
        min1DimensionCubeLeft = GenerateLowerDimension();
        min1DimensionCubeRight = GenerateLowerDimension();

        //block at 2dim
        if (min1DimensionCubeLeft == null || min1DimensionCubeRight == null)
        {
            GenerateSquare();
            return;
        }

        AddLowerDimensionVertices(min1DimensionCubeLeft, -0.5f);
        AddLowerDimensionVertices(min1DimensionCubeRight, 0.5f);
        Link2LowerDimensions();
        CollectLineGroups();
    }

    NDCube GenerateLowerDimension()
    {
        if(dimension <= minDimension)
        {
            return null;
        }

        int lowerDimension = dimension - 1;
        GameObject lowerDimGo = new GameObject("Dimension" + lowerDimension);
        lowerDimGo.transform.SetParent(transform);
        lowerDimGo.transform.localPosition = Vector3.zero;
        lowerDimGo.transform.localScale = Vector3.one;

        NDCube ncube = lowerDimGo.AddComponent<NDCube>();
        ncube.dimension = lowerDimension;

        ncube.GenerateNCube(root);

        return ncube;
    }

    void GenerateSquare()
    {
        float[] v1 = { -0.5f, -0.5f };
        float[] v2 = { -0.5f,  0.5f };
        float[] v3 = {  0.5f,  0.5f };
        float[] v4 = {  0.5f, -0.5f };

        vertices.Add(new VectorN(v1));
        vertices.Add(new VectorN(v2));
        vertices.Add(new VectorN(v3));
        vertices.Add(new VectorN(v4));

        LineGroup lineGroup = Instantiate(root.lineGroupPrefab, transform);

        lineGroup.lineRenderer.startColor = GetColorByDimension();
        lineGroup.lineRenderer.endColor = GetColorByDimension();

        lineGroup.transform.localPosition = Vector3.zero;
        lineGroup.transform.localScale = Vector3.one;

        lineGroup.indexTransform = new int[5];
        for (int i = 0; i < 4; i++)
        {
            lineGroup.indexTransform[i] = i;
        }
        lineGroup.indexTransform[4] = 0;

        lineGroups.Add(lineGroup);
    }

    void AddLowerDimensionVertices(NDCube min1DimensionCube, float newValue)
    {
        //Copy vertices from lower dimension, and add the newValue at the end of the vertex
        //if lower dimension = 2,  copy x y, and add z as newValue;

        for (int i = 0; i < min1DimensionCube.vertices.Count; i++)
        {
            VectorN newDimensionVertex = new VectorN(min1DimensionCube.vertices[i]);
            newDimensionVertex.AddDimension(newValue);
            vertices.Add(newDimensionVertex);
        }
    }

    void Link2LowerDimensions()
    {
        //Create connections and link both dimensions together
        ConnectionNull = new GameObject("Connection Null " + dimension);
        ConnectionNull.transform.SetParent(transform);
        ConnectionNull.transform.localPosition = Vector3.zero;
        ConnectionNull.transform.localScale = Vector3.one;

        int min1Length = min1DimensionCubeLeft.vertices.Count;
        for (int i = 0; i < min1Length; i++)
        {
            LineGroup lineGroup = Instantiate(root.lineGroupPrefab, ConnectionNull.transform);
            lineGroup.transform.localPosition = Vector3.zero;
            lineGroup.transform.localScale = Vector3.one;

            lineGroup.lineRenderer.startColor = GetColorByDimension();
            lineGroup.lineRenderer.endColor = GetColorByDimension();

            lineGroup.indexTransform = new int[2];
            lineGroup.indexTransform[0] = i;
            lineGroup.indexTransform[1] = i + min1Length;
            lineGroups.Add(lineGroup);
        }
    }

    void CollectLineGroups()
    {
        //Add ups both linegroups
        lineGroups.AddRange(min1DimensionCubeLeft.lineGroups);
        lineGroups.AddRange(min1DimensionCubeRight.lineGroups);
        
        //off set the second one
        int count = min1DimensionCubeRight.lineGroups.Count;
        int offset = min1DimensionCubeRight.vertices.Count;

        for (int i = 0; i < count; i++)
        {
            min1DimensionCubeRight.lineGroups[i].ApplyOffsetToIndices(offset);
        }
    }

    protected void UpdateLineGroups()
    {
        for (int i = 0; i < lineGroups.Count; i++)
        {
            lineGroups[i].ApplyLineToGroup(rotatedVertices);
            lineGroups[i].lineRenderer.startWidth = lineSize;
            lineGroups[i].lineRenderer.endWidth = lineSize;

            //without line renderer
            //Vector3[] positions = lineGroups[i].GetPositions(rotatedVertices);
            //CameraLineRenderer.Instance.RenderLines(positions,  null, null);
        }
    }

    Color GetColorByDimension()
    {
        float t = (float)(dimension - minDimension) / (root.dimension - minDimension);
        return root.gradientOverDimensions.Evaluate(t);
    }

    [System.Serializable]
    public class RotationByDimension
    {
        public int dimension;
        public Vector2Int rotationAroundDimension;
        public float angle;
        public float speed;

        public void Update()
        {
            angle += Time.deltaTime * speed;
        }

        public Matrix RotatePartialMatrix(Matrix matrix)
        {
            //Concept 
            /* On rotate une matrice par l'angle et les dimensions
             * Si on rotate par la 2ieme dimension, il y a 4 vertex
             * Donc on alterne en 4 vertex de la  matrice orinal et de la rotated
             * 
             * Si on rotate par la 3ième, il y a 8 vertex.. même chose
             * Si on rotate par la Nième dimension, il y a (2 ^ n) vertex
             */

            Matrix partialRotatedMatrix = new Matrix(matrix.rows, matrix.cols);
            Matrix rotationMatrix = MatrixRotationND.Rotation(angle * Mathf.Deg2Rad, matrix.cols, rotationAroundDimension.x, rotationAroundDimension.y);

            Matrix fullyRotatedMatrix = Matrix.StupidMultiply(matrix, rotationMatrix);

            int twoPowerOfDimension = (int)Mathf.Pow(2, dimension);
            bool takeFromOriginal = true;
            int internalCounter = 0;
            for (int i = 0; i < matrix.rows; i++)
            {
                if(takeFromOriginal)
                {
                    partialRotatedMatrix.SetRow(i, matrix.GetRow(i));
                }
                else
                {
                    partialRotatedMatrix.SetRow(i, fullyRotatedMatrix.GetRow(i));
                }

                internalCounter++;
                if (internalCounter == twoPowerOfDimension)
                {
                    internalCounter = 0;
                    takeFromOriginal = !takeFromOriginal;
                }
            }
            return partialRotatedMatrix;
        }
    }
}

public class MatrixRotationND
{
    public static Matrix Rotation(float angle, int dimension, int rotateDim1, int rotateDim2)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);

        double[,] matrix = new double[dimension, dimension];
        //Diagonal de 1
        for (int i = 0; i < dimension; i++)
        {
            matrix[i, i] = 1;
        }
        matrix[rotateDim1, rotateDim1] = cos;
        matrix[rotateDim1, rotateDim2] = -sin;
        matrix[rotateDim2, rotateDim2] = cos;
        matrix[rotateDim2, rotateDim1] = sin;

        return new Matrix (matrix);
    }
}