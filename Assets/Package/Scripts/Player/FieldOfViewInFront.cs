using UnityEngine;

public class FieldOfViewMouse : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    public Transform cone;                  // sprite du cône (pointe vers +Y local conseillé)
    public bool pivotAtBase = true;

    [Header("Taille de base")]
    public float baseLength = 4f;           // longueur par défaut (sans obstacle)
    public float baseWidth = 1.5f;         // largeur de base
    public bool clampToMax = true;
    public float maxLength = 6f;           // limite dure éventuelle

    [Header("Offsets d’origine")]
    public float startOffset = 0.2f;        // avance le cône devant le joueur
    public float sideOffset = 0f;          // décalage perpendiculaire au rayon (+gauche, -droite)
    public Vector2 playerLocalOffset = new Vector2(0f, -0.2f);
    // (x = décalage selon player.right, y = décalage selon player.up ; y<0 => "plus bas")

    [Header("Orientation du sprite")]
    public Vector2 spriteForward = Vector2.up;  // mettre Vector2.up si le triangle pointe vers le haut
    public float extraRotationOffset = 0f;

    [Header("Collisions")]
    public LayerMask obstacleMask;

    float spriteHeight = 1f, spriteWidth = 1f;
    float baseScaleX = 1f, baseScaleY = 1f;

    void Awake()
    {
        var sr = cone.GetComponent<SpriteRenderer>();
        if (sr && sr.sprite)
        {
            spriteHeight = sr.sprite.bounds.size.y; // longueur locale (Y)
            spriteWidth = sr.sprite.bounds.size.x; // largeur locale (X)
        }
        baseScaleX = cone.localScale.x;
        baseScaleY = cone.localScale.y;
    }

    void LateUpdate()
    {
        if (!player || !cone) return;

        // Direction joueur -> souris
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = player.position.z;
        Vector2 dir = ((Vector2)(mouse - player.position)).normalized;
        if (dir.sqrMagnitude < 1e-6f) return;

        // Offsets d’origine (local joueur puis latéral par rapport au rayon)
        Vector3 worldFromPlayerLocal =
            (Vector3)(player.right * playerLocalOffset.x + player.up * playerLocalOffset.y);

        Vector2 perp = new Vector2(-dir.y, dir.x); // perpendiculaire gauche
        Vector3 origin = player.position + worldFromPlayerLocal + (Vector3)(dir * startOffset) + (Vector3)(perp * sideOffset);

        // Longueur cible (base), bornée par max si demandé
        float targetBase = clampToMax ? Mathf.Min(baseLength, maxLength) : baseLength;

        // Raycast jusqu’à la limite utile
        float rayMax = clampToMax ? maxLength : baseLength;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, rayMax, obstacleMask);

        // Longueur finale = base raccourcie par l’obstacle éventuel
        float length = targetBase;
        if (hit) length = Mathf.Min(length, hit.distance);

        // Orientation
        float angle = Vector2.SignedAngle(spriteForward.normalized, dir) + extraRotationOffset;
        cone.rotation = Quaternion.Euler(0f, 0f, angle);

        // Mise à l’échelle : longueur sur Y, largeur sur X (restent indépendantes)
        Vector3 s = cone.localScale;
        s.y = (length / spriteHeight) * baseScaleY;     // LONGUEUR
        s.x = (baseWidth / spriteWidth) * baseScaleX;   // LARGEUR fixe
        cone.localScale = s;

        // Position selon le pivot
        cone.position = pivotAtBase ? origin : origin + (Vector3)(dir * (length * 0.5f));

        Debug.DrawRay(origin, dir * length, Color.yellow);
    }
}
