using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D RB;

    [SerializeField] float movespeed = 6;
    [SerializeField] float lookOffset = 0f;

    float MoveHorizontal, MoveVertical;


    bool dead = false;


    Vector2 movement;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>(); // Cette ligne sert a remplir la variable RB avec les informations du rigibody
        RB.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
        Rotation();
    }

    void Deplacement()
    {
        if (dead) // dans ce if je vérifie si mon perso est vivant car si il ne l'est pas je bloque le vecteur de déplacement à 0
        {
            movement = Vector2.zero;
            return;
        }
        MoveHorizontal = Input.GetAxisRaw("Horizontal"); // ici je définis mon movehorizontal et vertical qui sont des float 
        MoveVertical = Input.GetAxisRaw("Vertical");     // avec l'imput vertical et horizontal

        movement = new Vector2(MoveHorizontal, MoveVertical).normalized; 
    }

    void Rotation()
    {
        Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseWorldPos.z = 0f;

        Vector2 RotationDir = (MouseWorldPos - transform.position);

        if (RotationDir.magnitude < 0.00001f)
            return; // évite les angles foireux si la souris est "sur" le perso

        float angle = Mathf.Atan2(RotationDir.y, RotationDir.x) * Mathf.Rad2Deg + lookOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

    }

    private void FixedUpdate()
    {
        RB.linearVelocity = movement * movespeed;
    }
}
