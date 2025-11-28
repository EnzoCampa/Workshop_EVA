using UnityEngine;
using CodeMonkey.Utils;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fov = 45f;
    [SerializeField] private float viewDistance = 15f;

    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        // place l'objet FOV exactement à l'origine (monde)
        transform.position = origin;

        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        // +2 : centre + dernier vertex
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        // centre du cône en local
        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = UtilsClass.GetVectorFromAngle(angle);
            Vector3 vertexLocal;

            // raycast en MONDE
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, layerMask);
            if (hit.collider == null)
                vertexLocal = dir * viewDistance;              // local
            else
                vertexLocal = (Vector3)hit.point - origin;     // monde -> local

            vertices[vertexIndex] = vertexLocal;

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
    }

    public void SetOrigin(Vector3 origin) => this.origin = origin;

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }
}
