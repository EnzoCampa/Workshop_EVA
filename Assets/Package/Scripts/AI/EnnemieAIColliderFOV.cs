using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))] // Au moins un RB2D (kinematic) pour les triggers
public class EnnemieAIColliderFOV : MonoBehaviour
{
    [Header("Liens")]
    [SerializeField] private Transform Target;                // Joueur
    [SerializeField] private ParticleSystem CircileSound;
    [SerializeField] private EnnemieAI EnnemieScriptBase;

    [Header("FOV")]
    [Range(10f, 360f)]
    [SerializeField] private float fov = 120f;                // Angle d'ouverture
    [SerializeField] private float viewDistance = 15f;        // Portée max en unités monde
    [SerializeField] private int rayCount = 60;               // Densité des raycasts

    [Header("Masques")]
    [SerializeField] private LayerMask obstacleMask;          // Murs, décors bloquants
    [SerializeField] private LayerMask targetMask;            // Joueur (optionnel si tu utilises tags)
    
    [Header("Orientation")]
    [SerializeField] private bool lookAtTarget = false;       // Sinon, utilise la direction de l’ennemi
    [SerializeField] private Vector2 forwardLocal = Vector2.right; // Avance locale (par défaut +X)

    [Header("Debug")]
    public Vector3 CharacterPosition;

    // Internes
    private Mesh mesh;
    private PolygonCollider2D poly;
    private Vector3 origin;
    private float startingAngleDeg; // angle de départ en degrés (bord gauche du cône)

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        poly = GetComponent<PolygonCollider2D>();
        poly.isTrigger = true;

        // Important : pour OnTriggerEnter/Stay/Exit, il faut un Rigidbody2D sur au moins un des deux objets.
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.simulated = true;
        }
    }

    private void LateUpdate()
    {
        // 1) Définir l’origine et l’orientation du FOV
        origin = transform.position;

        // Calcul de la direction avant en monde
        Vector2 forwardWorld;
        if (lookAtTarget && Target != null)
        {
            Vector2 dirToTarget = (Target.position - origin).normalized;
            forwardWorld = dirToTarget;
        }
        else
        {
            // Convertit un vecteur local (ex: +X) en monde
            forwardWorld = (Vector2)(transform.TransformDirection((Vector3)forwardLocal)).normalized;
        }

        // Centre du FOV
        float centerAngleDeg = GetAngleFromVectorFloat(forwardWorld);
        // Bord gauche = centre + fov/2 (sens horaire négatif ensuite)
        startingAngleDeg = centerAngleDeg + (fov * 0.5f);

        // 2) Construire le mesh + le chemin du PolygonCollider2D
        if (rayCount < 3) rayCount = 3;
        float angle = startingAngleDeg;
        float angleDecrease = fov / rayCount;

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
            Vector3 dir = UtilsClass.GetVectorFromAngle(angle);     // direction en monde (normale)
            Vector3 vertexLocal;

            // Raycast obstacles : limite la portée au premier obstacle
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, obstacleMask);
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
            angle -= angleDecrease;
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

    //    // 3) Détection « FOV + LOS » du joueur (option Timer si tu veux lisser la charge)
    //    if (Target != null && IsTargetInFOVAndVisible(Target.position, forwardWorld))
    //    {
    //        CharacterPosition = Target.position;
    //        if (CircileSound != null && CircileSound.isPlaying) CircileSound.Stop();
    //        if (EnnemieScriptBase != null)
    //        {
    //            EnnemieScriptBase.IsCharacter = true;
    //            EnnemieScriptBase.CharacterPosition = CharacterPosition;
    //        }
    //    }
    //    else
    //    {
    //        // Pas vu / pas de LOS
    //        CharacterPosition = Vector2.zero;
    //        if (EnnemieScriptBase != null) EnnemieScriptBase.IsCharacter = false;
    //    }
    //}

    private bool IsTargetInFOVAndVisible(Vector3 targetPos, Vector2 forwardWorld)
    {
        // Distance
        Vector2 toTarget = (Vector2)(targetPos - origin);
        float dist = toTarget.magnitude;
        if (dist > viewDistance) return false;

        // FOV : angle
        Vector2 dir = toTarget.normalized;
        float dot = Vector2.Dot(forwardWorld, dir);
        // Seuil = cos(fov/2)
        float threshold = Mathf.Cos(Mathf.Deg2Rad * (fov * 0.5f));
        if (dot < threshold) return false;

        // LOS : raycast vers le joueur, bloqué par obstacles
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, obstacleMask);
        if (hit.collider != null) return false; // un obstacle bloque la vue

        // Optionnel : vérifier qu’on touche bien la couche « target »
        if (targetMask.value != 0)
        {
            int targetLayer = Target.gameObject.layer;
            if (((1 << targetLayer) & targetMask) == 0) return false;
        }

        return true;
    }

    // Utilitaire : angle en degrés à partir d’un vecteur (0° = +X, anti-horaire)
    private static float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }

    // Triggers basés sur le PolygonCollider2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vision"))
        {
            CharacterPosition = Target != null ? Target.position : collision.transform.position;
            if (CircileSound != null) CircileSound.Stop();

            if (EnnemieScriptBase != null)
            {
                EnnemieScriptBase.IsCharacter = true;
                EnnemieScriptBase.CharacterPosition = CharacterPosition;
            }
        }
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Vision"))
        {
            CharacterPosition = Target != null ? Target.position : collision.transform.position;
            if (EnnemieScriptBase != null)
            {
                EnnemieScriptBase.IsCharacter = true;
                EnnemieScriptBase.CharacterPosition = CharacterPosition;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vision"))
        {
        }
    }

}