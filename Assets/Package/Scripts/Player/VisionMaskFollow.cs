using UnityEngine;

public class VisionMaskFollow : MonoBehaviour
{
    public Transform visionMask; // Drag & drop VisionMask
    public float visionRange = 8f; // rayon/longueur
    public float coneScale = 1f;   // selon ton sprite

    void LateUpdate()
    {
        // Position/rotation identiques au joueur
        visionMask.position = transform.position;
        visionMask.rotation = transform.rotation; // ou l’angle de visée

        // Adapter la taille à la portée (selon le sprite)
        visionMask.localScale = Vector3.one * (visionRange * coneScale);
    }
}