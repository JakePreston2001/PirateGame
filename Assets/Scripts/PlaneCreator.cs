using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCreator : MonoBehaviour
{/*
    [Range(2,256)]
    public int res = 10;
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilter;
    CustomPlane[] customPlane;

    private void Start()
    {
        Initialise();
        generateMesh();
    }
   
    void Initialise() 
    {
        if (meshFilter == null || meshFilter.Length == 0)
        {
            meshFilter = new MeshFilter[1];
        }
        customPlane = new CustomPlane[1];

        if (meshFilter[0] == null)
        {
            GameObject meshObj = new GameObject("Mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilter[0] = meshObj.AddComponent<MeshFilter>();
            meshFilter[0].sharedMesh = new Mesh();
        }
        customPlane[0] = new CustomPlane(meshFilter[0].sharedMesh, res, Vector3.up);
    }

    void generateMesh() 
    {
        foreach (CustomPlane plane in customPlane) 
        {
            plane.ConstructMesh();
        }
    }
    */
}
