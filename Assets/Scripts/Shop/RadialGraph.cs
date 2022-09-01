using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialGraph : MonoBehaviour
{
    Vector3[] vertices = new Vector3[6];
    MeshFilter filter;
    Mesh mesh;
    LineRenderer line;

    [Header("Position")]
    public Vector3[] VerticesPos = new Vector3[5];

    [Header("Value")]
    public float size = 1f;
    [Range(0f, 1f)] public float top = 1f;
    [Range(0f, 1f)] public float rightTop = 1f;
    [Range(0f, 1f)] public float rightBot = 1f;
    [Range(0f, 1f)] public float leftBot = 1f;
    [Range(0f, 1f)] public float leftTop = 1f;

    void Start()
    {
        mesh = new Mesh();
        filter = GetComponent<MeshFilter>();
        line = GetComponent<LineRenderer>();
        vertices = new Vector3[6];
    }

    private void Update()
    {
        vertices[0] = VerticesPos[0] * top * size;
        vertices[1] = VerticesPos[1] * rightTop * size;
        vertices[2] = VerticesPos[2] * rightBot * size;
        vertices[3] = VerticesPos[3] * leftBot * size;
        vertices[4] = VerticesPos[4] * leftTop * size;
        vertices[5] = Vector3.zero;

        float[] pivot = new float[5] { top, rightTop, rightBot, leftBot, leftTop };

        for (int i = 0; i < VerticesPos.Length; i++)
        {
            line.SetPosition(i, VerticesPos[i] * pivot[i] * (size + 1)+ transform.position + new Vector3(0, 0, -1));
        }
        line.SetPosition(5, VerticesPos[0] * pivot[0] * (size + 1) + transform.position + new Vector3(0, 0, -1));

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 5, 1, 2, 5, 2, 3, 5, 3, 4, 5, 4, 0, 5 };
        filter.mesh = mesh;
    }

}
