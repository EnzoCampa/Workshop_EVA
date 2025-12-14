using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;  // obstacles
    [SerializeField] private float fov = 45f;
    [SerializeField] private float viewDistance = 15f;
    [SerializeField] private float offset = 10f;
    [SerializeField] int rayCount = 50;
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    private PolygonCollider2D poly;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        poly = GetComponent<PolygonCollider2D>();
        poly.isTrigger = true;
    }

    void LateUpdate()
    {
        transform.position = origin;

        
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        // Chemin du polygon collider (en local)
        List<Vector2> path = new List<Vector2>(rayCount + 2);
        path.Add(Vector2.zero);

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = UtilsClass.GetVectorFromAngle(angle);
            Vector3 vertexLocal;

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, layerMask);
            if (hit.collider == null)
                vertexLocal = dir * viewDistance;
            else
                vertexLocal = (Vector3)hit.point - origin;


            vertices[vertexIndex] = vertexLocal;
            path.Add((Vector2)vertexLocal);

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * (viewDistance * 2f + 1f));

        // Met à jour le polygon collider (1 seul chemin)
        poly.pathCount = 1;
        poly.SetPath(0, path);
    }

    public void SetOrigin(Vector3 origin) => this.origin = origin;

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }
}
