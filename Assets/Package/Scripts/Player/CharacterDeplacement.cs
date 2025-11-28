using CodeMonkey.Utils;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D RB;

    [SerializeField] float movespeed = 6;
    [SerializeField] float lookOffset = 0f;
    [SerializeField] private FieldOfView fieldOfView;

    float MoveHorizontal, MoveVertical;


    bool dead = false;


    Vector2 movement;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>(); // Cette ligne sert a remplir la variable RB avec les informations du rigibody
        RB.freezeRotation = true;


        FunctionUpdater.Create(() =>
        {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();
            Vector3 aimDir = (targetPosition - transform.position).normalized;
            fieldOfView.SetAimDirection(aimDir);
            fieldOfView.SetOrigin(transform.position);
        });
    }


        void Update()
            {
               
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
                Vector3 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // je récupère la position de ma souris en fonction de la camera
                MouseWorldPos.z = 0f;

                Vector2 RotationDir = (MouseWorldPos - transform.position);

                if (RotationDir.magnitude < 0.00001f)
                    return; // évite les angles foireux si la souris est "sur" le perso

                float angle = Mathf.Atan2(RotationDir.y, RotationDir.x) * Mathf.Rad2Deg + lookOffset; //opération mathématoqie pour la rotation sur l'axe X et Y
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

            }
    void LateUpdate()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 aimDir = (mouseWorld - transform.position).normalized;

        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(aimDir);
    }

    private void FixedUpdate()
        {
        Deplacement();
        Rotation();
        RB.linearVelocity = movement * movespeed; //Calcul du déplacement 
        }
    }
