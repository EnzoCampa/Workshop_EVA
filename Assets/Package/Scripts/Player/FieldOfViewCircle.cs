using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
public class FieldOfViewCircle : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;  // obstacles
    [SerializeField] private float fov = 45f;
    [SerializeField] private float viewDistance = 15f;
    [SerializeField] private float offset = 10f;
    [SerializeField] int rayCount = 50;
    [SerializeField] private float circleRadius = 0.5f;

    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    private PolygonCollider2D poly;
    float angle = 0f;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        poly = GetComponent<PolygonCollider2D>();
        poly.isTrigger = true;
    }

    void LateUpdate()
{
    // Positionne le GameObject FOV au centre du perso
    transform.position = origin;

    if (rayCount < 3) rayCount = 3;
    float angleIncrease = fov / rayCount;

    // --- 1) OFFSET circulaire depuis la souris (cercle, pas disque) ---
    Vector2 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    Vector2 dirFromCenter = mouseViewport - new Vector2(0.5f, 0.5f);
    Vector2 offsetWorld = dirFromCenter.sqrMagnitude > 1e-8f
        ? dirFromCenter.normalized * circleRadius
        : Vector2.zero;

    // Origine REELLE des raycasts (décalée)
    Vector3 rayOrigin = origin + (Vector3)offsetWorld;

    // --- 2) Mesh/collider ---
    Vector3[] vertices = new Vector3[rayCount + 2];
    Vector2[] uv = new Vector2[vertices.Length];
    int[] triangles = new int[rayCount * 3];

    // Le centre du cône est décalé
    vertices[0] = offsetWorld;

    List<Vector2> path = new List<Vector2>(rayCount + 2);
    path.Add(offsetWorld);

    int vertexIndex = 1;
    int triangleIndex = 0;

    // IMPORTANT : réinitialiser l’angle !
    float angle = startingAngle;

    for (int i = 0; i <= rayCount; i++)
    {
        Vector3 dir = UtilsClass.GetVectorFromAngle(angle);

        // Raycast depuis l'origine décalée
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, viewDistance, layerMask);

        Vector3 vertexLocal = (hit.collider == null)
            ? (Vector3)offsetWorld + dir * viewDistance
            : (Vector3)hit.point - origin;       // hit.point est en monde ; on revient en local du GO (origin)
                                                // puis on ajoute offsetWorld juste après pour cohérence

        // Si on a utilisé hit.point - origin, ajoute aussi l’offset pour aligner le mesh
        if (hit.collider != null)
            vertexLocal += (Vector3)offsetWorld;

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

    poly.pathCount = 1;
    poly.SetPath(0, path);
}


    public void SetOrigin(Vector3 origin) => this.origin = origin;

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }
}
