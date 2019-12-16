using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainColorGenerator : MonoBehaviour{
    [Header("Mesh")]
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    [SerializeField]
    private int sizeX = 100;
    [SerializeField]
    private int sizeZ = 100;
    [SerializeField]
    private Gradient gradient;

    [Header("PerlinNoise")]
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetZ;
    [SerializeField]
    private float scale = 10;
    [SerializeField]
    private float amplitude = 20;

    private float minTerrainHeight;
    private float maxTerrainHeight;

    private void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreatePlane();
        UpdateMesh();
    }

    private void CreatePlane(){
        vertices = new Vector3[(sizeX + 1)*(sizeZ + 1)];

        for(int index = 0, z = 0; z <= sizeZ; z++){
            for(int x = 0; x <= sizeX; x++){
                float y = PerlinNoise(x, z);
                vertices[index] = new Vector3(x,y,z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

                index++;
            }
        }

        triangles = new int[sizeX * sizeZ * 6];

        int vert = 0;
        int tris = 0;

        for(int z = 0; z < sizeZ; z++){
            for(int x = 0; x < sizeX; x++){
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + sizeX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + sizeX + 1;
                triangles[tris + 5] = vert + sizeX + 2;

                vert++;
                tris+=6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= sizeZ; z++)
        {
            for (int x = 0; x <= sizeX; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private float PerlinNoise(int x, int z){
        float xCoord  = ((float)(x + offsetX)/ sizeX) * scale;
        float zCoord  = ((float)(z + offsetZ) / sizeZ) * scale;
        float perlin = Mathf.PerlinNoise(xCoord, zCoord);
        perlin *= amplitude;
        return perlin;
    }
}