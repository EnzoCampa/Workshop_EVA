using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using UnityEngine.Splines;

public class EnnemieAI : MonoBehaviour
{

    [Header("Lien")]
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Transform Target;
    [SerializeField] PlayerController Player;


    [Header("Spline")]
    [SerializeField] private SplineContainer spline;     // Laisse vide : auto-find ou auto-create
    [SerializeField] private bool closedLoop = false;    // Fermer la courbe
    
    [Header("Déplacement")]
    [SerializeField] private float speed = 2f;           // unités/seconde le long de la spline

    private Rigidbody2D rb;

    bool IsCharacter = false;
    Vector3 movement;
    Vector3 CharacterPosition;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spline == null)
            spline = GetComponent<SplineContainer>();
        int count = spline.Spline.Count;
        for (int i = 0; i < count; i++)
        {
            BezierKnot knot = spline.Spline[i];
            float3 local = knot.Position;
            Vector3 world = transform.TransformPoint((Vector3)local);
        }
    }
   
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>(); //je met les infos du navmesh dans mon agent
        Agent.updateRotation = false; 
        Agent.updateUpAxis = false;
    }
    private void Update()
    {
        if (IsCharacter)
        {
            DéplacementToCharacter(); 
        }
        else
        {
            Déplacement();
        }
    }


    public void OnTriggerEnter2D(Collider2D collision) // quand je trigger ma collision box je met mon is character a true
    {                                                  // afin de start les déplacemnts. De plus, je récupère les info de
        if (collision.CompareTag("Player"))            // position pour les intégrer dans variable "CharacterPosition"
        {
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
        if (collision.CompareTag("Vision"))
        {
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)  // quand je trigger ma collision box je met mon is character a true
    {                                                  // afin de start les déplacemnts. De plus, je récupère les info de
        if (collision.CompareTag("Player"))            // position pour les intégrer dans variable "CharacterPosition"
        {
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
        if (collision.CompareTag("Vision"))
        {
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // quand je sors de ma collision box je met mon is character a false
    {                                                  // afin de stopper les déplacemnts. De plus, je vide les info de
        if (collision.CompareTag("Player"))            // position
        {
            CharacterPosition = Vector2.zero; IsCharacter = false;
            IsCharacter = false;
        }
        if (collision.CompareTag("Vision"))
        {
            CharacterPosition = Vector2.zero; IsCharacter = false;
            IsCharacter = false;
        }
    }

    public void Déplacement()
    {
        Debug.Log("test");
        Vector3 pos0 = (Vector3)spline.Spline[0].Position;
        Vector3 pos1 = (Vector3)spline.Spline[1].Position;

        // Tolérance pour la comparaison de positions
        float tolerance = 1f;

        Vector2 movement = Vector2.zero;

        if (Vector3.Distance(transform.position, pos0) < tolerance)
        {
            movement = new Vector2(pos1.x - pos0.x, pos1.y - pos0.y).normalized;
        }
        else if (Vector3.Distance(transform.position, pos1) < tolerance)
        {
            movement = new Vector2(pos0.x - pos1.x, pos0.y - pos1.y).normalized;
        }

        rb.linearVelocity = movement * speed;
    
    }


    public void DéplacementToCharacter()
    {
        Agent.SetDestination(CharacterPosition);
    }
}
