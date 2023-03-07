using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int flag = 0;
    Mesh mesh;

    Vector3[] vertices;
    Color[] colors;
    int[] triangles;
    Vector2[] uv;
    List<Vector3> vertList = new List<Vector3>{};
    List<int> triList = new List<int>{};
    List<Vector2> uvList = new List<Vector2>{};
    public Gradient gradient;
    public int xSize;
    public int zSize;
    public float stepLength = 0.2f;
    public float xPos { get; set; }
    public float zPos { get; set; }
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

        switch(flag){
            case 0:
                xPos = 0f;
                zPos = 0f;
                frequency = 0.03f;
                amplitude = 3f;
                octwave = 2;
                lacunarity = 2f;
                sliderEvent();
                break;
            case 1:
                sphere();
                UpdateMesh();
                break;
        }
    }


    public void sliderEvent()
    {
        Debug.Log($"{xPos} {zPos} freq:{frequency} ampl:{amplitude} octwave:{octwave} lacu{lacunarity}");
        quadUV();
        UpdateMesh();
    }

    void quadUV()
    {
        float minHeight, maxHeight;
        minHeight = float.MaxValue;
        maxHeight = float.MinValue;
        float temp;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = fractalNoise(new Vector3(x + xPos, 0, z + zPos));
                vertices[i] = new Vector3(x - (xSize/2), Mathf.Clamp(y, -400f, 400f), z - (zSize/2)) * stepLength;
                if(vertices[i].y > maxHeight) maxHeight = vertices[i].y;
                if(vertices[i].y < minHeight) minHeight = vertices[i].y;
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

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                temp = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(temp);
                i++;
            }
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;
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


    public void sphere() {
        // float temp;
        // float minHeight, maxHeight;
        int sectorCount = xSize;
        int stackCount = zSize;
        //vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float sphereRadius = stepLength;
        // init va
        // temp variables
        Vector3 sphereVertexPos;
        Vector2 textureCoordinate;
        float xy;
        float sectorStep = 2.0f * Mathf.PI / (float)sectorCount;
        float stackStep = Mathf.PI / (float)stackCount;
        float sectorAngle, stackAngle;


        // compute vertices and normals
        for (int i = 0; i <= stackCount; ++i) {
            stackAngle = Mathf.PI / 2.0f - i * stackStep;
            xy = sphereRadius * Mathf.Cos(stackAngle);
            sphereVertexPos.z = sphereRadius * Mathf.Sin(stackAngle);

            for (int j = 0; j <= sectorCount; ++j) {
                sectorAngle = j * sectorStep;

                // vertex position
                sphereVertexPos.x = xy * Mathf.Sin(sectorAngle);
                sphereVertexPos.y = xy * Mathf.Cos(sectorAngle);
                vertList.Add(sphereVertexPos);

                // normalized vertex normal

                // calculate texture coordinate
                textureCoordinate.x = (float)(j) / (float)sectorCount;
                textureCoordinate.y = (float)(i) / (float)stackCount;
                uvList.Add(textureCoordinate);
            }
        }
        uv = uvList.ToArray();
        vertices = vertList.ToArray();
        // compute triangle indices
        //triangles = new int[xSize * zSize * 6];
        triList = new List<int>{};
        //int vert = 0;
        int tris = 0;
        int k1, k2;

        for (int i = 0; i < stackCount; ++i) {
            k1 = i * (sectorCount + 1);
            k2 = k1 + sectorCount + 1;

            for (int j = 0; j < sectorCount; ++j, ++k1, ++k2) {
                // 2 triangles per sector excluding first and last stacks
                // k1 => k2 => k1+1
                if (i != 0) {
                    //indices.push_back(glm::ivec3(k1, k2, k1 + 1));
                    triList.Add(k1 + 1);
                    triList.Add(k2);
                    triList.Add(k1);
                }
                // k1+1 => k2 => k2+1
                if (i != (stackCount - 1)) {
                    //indices.push_back(glm::ivec3(k1 + 1, k2, k2 + 1));
                    triList.Add(k2 + 1);
                    triList.Add(k2);
                    triList.Add(k1 + 1);
                }

            }
        }
        triangles = triList.ToArray();

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //temp = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(0.5f);
                i++;
            }
        }

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
            Gizmos.DrawSphere(vertices[i], 0.001f);
        }
    }


}