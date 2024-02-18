using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagicPath : MonoBehaviour
{
    [Header("Path Generation")]
    public MeshFilter _plane;
    public Transform pathStart;
    public Transform pathEnd;
    public float pathWidth;

    [Header("Objects on Path")] 
    public GameObject[] objects;
    public int numberOfObjects;

    // Start is called before the first frame update
    void Awake()
    {
        GeneratePath();
        GenerateObjects();
    }

    [ContextMenu("GeneratePath")]
    void GeneratePath()
    {
        Transform planeTransform = _plane.transform;
        
        Mesh pathMesh = new Mesh();
        
        List<Vector3> verticesList = new List<Vector3>();
        List<int> trianglesList = new List<int>();

        // Add vertices for pathStart
        verticesList.Add(planeTransform.InverseTransformPoint(pathStart.position + pathStart.right.normalized * pathWidth / 2));
        verticesList.Add(planeTransform.InverseTransformPoint(pathStart.position + pathStart.right.normalized * -pathWidth / 2));
        
        //Add path end
        verticesList.Add(planeTransform.InverseTransformPoint(pathEnd.position + pathEnd.right.normalized * -pathWidth / 2));
        verticesList.Add(planeTransform.InverseTransformPoint(pathEnd.position + pathEnd.right.normalized * pathWidth / 2));
        
        trianglesList.Add(0);           // Vertex 0
        trianglesList.Add(1);       // Vertex 1
        trianglesList.Add(2);       // Vertex 2

        trianglesList.Add(2);       // Vertex 1
        trianglesList.Add(1);       // Vertex 3
        trianglesList.Add(3);       // Vertex 2
        
        // Convert lists to arrays
        Vector3[] vertices = verticesList.ToArray();
        int[] triangles = trianglesList.ToArray();

        // Set vertices and triangles to the mesh
        pathMesh.vertices = vertices;
        pathMesh.triangles = triangles;

        pathMesh.RecalculateBounds();
        pathMesh.RecalculateNormals();
        pathMesh.RecalculateTangents();

        _plane.sharedMesh = pathMesh;
    }

    void GenerateObjects()
    {
        Mesh path = _plane.mesh;
        Bounds bounds = path.bounds;
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
            Transform newObject = Instantiate(objects[Random.Range(0, objects.Length)], _plane.transform).transform;
            newObject.transform.localPosition = randPos;
        }
    }
}