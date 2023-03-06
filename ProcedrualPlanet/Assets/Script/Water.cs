using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Water : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    public float xPos { get; set; }
    public float zPos { get; set; }
    public int xSize;
    public int zSize;
    public float stepLength = 0.2f;
    public float frequency { get; set; }
    public float amplitude { get; set; }
    public float octwave { get; set; }
    public float lacunarity { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        initMesh();
        // quadUV();
        // UpdateMesh();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            quadUV();
            UpdateMesh();
        }


    }

    public void initMesh(){
        xPos = 0f;
        zPos = 0f;
        frequency = 0.03f;
        amplitude = 3f;
        octwave = 2;
        lacunarity = 2f;
        sliderEvent();
    }


    public void sliderEvent()
    {
        Debug.Log($"{xPos} {zPos} freq:{frequency} ampl:{amplitude} octwave:{octwave} lacu{lacunarity}");
        quadUV();
        UpdateMesh();
    }

    void quadUV()
    {
        
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = 0;
                vertices[i] = new Vector3(x - (xSize/2), y, z - (zSize/2)) * stepLength;
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

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
        mesh.RecalculateNormals();
        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        /*
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        //*/
    }



    float fractalNoise(Vector3 point)
    {
        float noiseSum = 0;
        float amp = amplitude;
        float fre = frequency;
        for (int i = 0; i < (int)octwave; i++)
        {
            // Sample noise function and add to the result
            noiseSum += (Mathf.PerlinNoise(point.x * fre, point.z * fre) - 0.5f) * amp;
            // Make each layer more and more detailed
            fre *= lacunarity;// Make each layer contribute less and less to result
            amp /= lacunarity;
        }
        return noiseSum;
    }


    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }


}