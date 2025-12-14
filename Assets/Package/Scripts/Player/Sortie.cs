using UnityEngine;
using static UnityEngine.UI.Image;

public class Sortie : MonoBehaviour
{
    [SerializeField] private Transform center;          // perso (centre du cercle)
    [SerializeField] private Transform endLevel;        // cible à viser
    [SerializeField] private float radius = 2f;         // rayon du cercle
    [SerializeField] private float angularSpeedDeg = 360f; // vitesse de suivi le long du cercle
    [SerializeField] private float rotationOffsetDeg = 0f; // si la pointe n'est pas +X
    [SerializeField] private float fixedZ = 0f;         // Z constant en 2D

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, Range(0f, 1f)] private float alpha = 0.3f;
    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        Color Color = spriteRenderer.color;
        Color.a = alpha;
        spriteRenderer.color = Color;

        if (!center || !endLevel) return;

        Vector2 c = center.position;
        Vector2 toEnd = (Vector2)endLevel.position - c;
        if (toEnd.sqrMagnitude < 1e-8f) return;

        // Direction souhaitée sur le cercle (vers EndLevel)
        Vector2 desiredDir = toEnd.normalized;

        // Direction actuelle (du centre vers le mesh) ; si mal placé, on “snap” au besoin
        Vector2 fromCenter = (Vector2)transform.position - c;
        Vector2 currentDir = fromCenter.sqrMagnitude > 1e-8f ? fromCenter.normalized : desiredDir;

        // Avance angulairement vers la direction cible le long de la périphérie
        float curAng = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;
        float desAng = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg;
        float nextAng = Mathf.MoveTowardsAngle(curAng, desAng, angularSpeedDeg * Time.deltaTime);

        Vector2 finalDir = new Vector2(Mathf.Cos(nextAng * Mathf.Deg2Rad), Mathf.Sin(nextAng * Mathf.Deg2Rad));

        // Position finale sur le cercle
        Vector3 pos = new Vector3(c.x + finalDir.x * radius, c.y + finalDir.y * radius, fixedZ);
        transform.position = pos;

        // Orientation de la pointe vers EndLevel
        Vector2 face = (Vector2)endLevel.position - (Vector2)transform.position;
        float rotZ = Mathf.Atan2(face.y, face.x) * Mathf.Rad2Deg + rotationOffsetDeg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

    }
}