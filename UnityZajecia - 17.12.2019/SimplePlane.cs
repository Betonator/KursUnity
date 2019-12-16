using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SimplePlane : MonoBehaviour{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    private void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreatePlane();
        UpdateMesh();
    }

    private void CreatePlane(){
        vertices = new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0)
        };

        triangles = new int[]{
            0, 1, 2,
            1, 3, 2
        };
    }

    private void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}