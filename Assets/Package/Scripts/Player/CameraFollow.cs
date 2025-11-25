using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow2D : MonoBehaviour
{
   [Header("Cibles")]
    public Transform target; // transform du plater

    [Header("Réglages caméra")]
    [SerializeField] private float followSpeed = 5f;  // vitesse de lissage
    [SerializeField] private float maxOffset = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || cam == null)
            return;

        // --- 1. Position de base ---
        Vector3 basePos = target.position;

        // --- 2. Position relative de la souris à l'écran (de -1 à 1) ---
        Vector2 mouseViewport = cam.ScreenToViewportPoint(Input.mousePosition);
        Vector2 offsetNorm = (mouseViewport - new Vector2(0.5f, 0.5f)) * 2f;

        // --- 3. Application du décalage ---
        Vector3 offset = new Vector3(offsetNorm.x, offsetNorm.y, 0f) * maxOffset;
        Vector3 targetPos = basePos + offset;
        targetPos.z = -10f;

        // --- 4. Lissage ---
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }
}