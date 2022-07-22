using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RadialGraph : MonoBehaviour
{
    Vector3[] vertices = new Vector3[6];
    MeshFilter filter;
    Mesh mesh;

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
        vertices = new Vector3[6];
    }

    private void Update()
    {
        vertices[0] = new Vector3(0f, 1f) * top * size;
        vertices[1] = new Vector3(0.95f, 0.3f) * rightTop * size;
        vertices[2] = new Vector3(0.6f, -0.8f) * rightBot * size;
        vertices[3] = new Vector3(-0.6f, -0.8f) * leftBot * size;
        vertices[4] = new Vector3(-0.95f, 0.3f) * leftTop * size;
        vertices[5] = Vector3.zero;

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 5, 1, 2, 5, 2, 3, 5, 3, 4, 5, 4, 0, 5 };
        filter.mesh = mesh;
    }

}
