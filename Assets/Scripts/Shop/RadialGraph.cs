using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RadialGraph : MonoBehaviour
{
    Vector3[] vertices = new Vector3[5];
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
    }

    private void Update()
    {
        vertices[0] = new Vector3(0f, 1f) * top * size;
        vertices[1] = new Vector3(0.95f, 0.3f) * rightTop * size;
        vertices[2] = new Vector3(0.6f, -0.8f) * rightBot * size;
        vertices[3] = new Vector3(-0.6f, -0.8f) * leftBot * size;
        vertices[4] = new Vector3(-0.95f, 0.3f) * leftTop * size;

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
        filter.mesh = mesh;
    }

}
