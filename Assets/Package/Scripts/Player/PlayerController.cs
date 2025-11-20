using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D RB;
    float MoveHorizontal, MoveVertical;
    bool dead = false;
    Vector2 movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>(); // Cette ligne sert a remplir la variable RB avec les informations du rigibody
    }

    // Update is called once per frame
    void Update()
    {
        Deplacement();
    }

    void Deplacement()
    {
        if (dead) // dans ce if je vérifie si mon perso est vivant car si il ne l'est pas je bloque le vecteur a 0
        {
            movement = Vector2.zero;
            return;
        }
        MoveHorizontal = Input.GetAxisRaw("Horizontal");
        MoveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(MoveHorizontal, MoveVertical).normalized;
    }
}
