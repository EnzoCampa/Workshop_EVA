using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 newPos = target.position;
        newPos.z = -10; 
        transform.position = newPos;
    }           
}
