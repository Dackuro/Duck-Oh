using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(ShockWave))]
public class ProceduralTorus : MonoBehaviour
{
    [Header("Torus Settings")]
    [SerializeField] private string torusName;
    [Space]
    [Range(1f, 100f)] public float majorRadius = 1f;        // Torus Size
    [Range(0.05f, 2f)] public float minorRadius = 0.25f;    // Tube Radius
    [Space]
    [Range(3, 128)] public int majorSegments = 64;          // Torus Segments
    [Range(3, 64)] public int minorSegments = 32;           // Torus Radius

    private Mesh torusMesh;

    [Header("Optimizations")]
    [SerializeField] private int frameUpdate;
    [SerializeField] private int startFrame;


    void Awake() => GenerateTorus();

    void OnValidate() => GenerateTorus();

    void Start() => startFrame = Time.frameCount;
    

    void Update()
    {
        if ((Time.frameCount - startFrame) % frameUpdate == 0)        
            GenerateTorus();
    }

    void GenerateTorus()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (torusMesh == null)
        {
            torusMesh = new Mesh();
            torusMesh.name = torusName;
            meshFilter.sharedMesh = torusMesh;
        }
        else
            torusMesh.Clear();

        Vector3[] vertices = new Vector3[majorSegments * minorSegments];
        int[] triangles = new int[majorSegments * minorSegments * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        int triIndex = 0;

        for (int i = 0; i < majorSegments; i++)
        {
            float theta = (float)i / majorSegments * Mathf.PI * 2f;
            Quaternion majorRot = Quaternion.Euler(0f, Mathf.Rad2Deg * theta, 0f);

            for (int j = 0; j < minorSegments; j++)
            {
                float phi = (float)j / minorSegments * Mathf.PI * 2f;
                Vector3 pos = new Vector3(
                    (majorRadius + minorRadius * Mathf.Cos(phi)),
                    minorRadius * Mathf.Sin(phi),
                    0f
                );

                int index = i * minorSegments + j;
                vertices[index] = majorRot * pos;
                uvs[index] = new Vector2((float)i / majorSegments, (float)j / minorSegments);

                int nextI = (i + 1) % majorSegments;
                int nextJ = (j + 1) % minorSegments;

                int a = i * minorSegments + j;
                int b = nextI * minorSegments + j;
                int c = nextI * minorSegments + nextJ;
                int d = i * minorSegments + nextJ;

                triangles[triIndex++] = a;
                triangles[triIndex++] = b;
                triangles[triIndex++] = c;

                triangles[triIndex++] = a;
                triangles[triIndex++] = c;
                triangles[triIndex++] = d;
            }
        }

        torusMesh.vertices = vertices;
        torusMesh.triangles = triangles;
        torusMesh.uv = uvs;
        torusMesh.RecalculateNormals();
    }
}
