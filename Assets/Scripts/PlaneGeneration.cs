using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlaneGeneration : MonoBehaviour
{
    [Range(1,100)]
    public int Size = 20;
    [Range((float)0.01, 100)]
    public float scale = 1.0f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private int verticiesLength = 0;




    // Start is called before the first frame update
    void OnValidate()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        verticiesLength = (Size + 1) * (Size + 1);

        UpdatePlaneVerticies();
        UpdateMesh();
    }

    private void Start()
    {
        flatShader();
        UpdateMesh();
    }

    void flatShader() 
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUVs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUVs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertices;
        uvs = flatShadedUVs;
    }

    void UpdatePlaneVerticies()
    {
        vertices = new Vector3[verticiesLength];
        uvs = new Vector2[vertices.Length];

        float halfSizeX = (scale * Size) / 2;
        float halfSizeZ = (scale * Size) / 2;

        int i = 0;
        for (int z = 0; z <= Size; z++)
        {
            for (int x = 0; x <= Size; x++)
            {
                float xPos = (x * scale) - halfSizeX;
                float zPos = (z * scale) - halfSizeZ;
                float yPos = 0;

                vertices[i] = new Vector3(xPos, yPos, zPos);



                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }

        triangles = new int[Size * Size * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < Size; z++)
        {
            for (int x = 0; x < Size; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + Size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + Size + 1;
                triangles[tris + 5] = vert + Size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        flatShader();
        mesh.RecalculateNormals();
        flatShader();
    }

}