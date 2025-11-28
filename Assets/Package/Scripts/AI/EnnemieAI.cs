using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using UnityEngine.Splines;
using NUnit.Framework.Constraints;

public class EnnemieAI : MonoBehaviour
{

    [Header("Lien")]
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Transform Target;
    [SerializeField] PlayerController Player;


    [Header("Spline")]
    [SerializeField] private SplineContainer spline;     

    [Header("EnemyPatrol")]
    public Transform[] PatrolPoints;
    public int TargetPoint;
    public float speed;

    private Rigidbody2D rb;

    [Header("SplineComponent")]
    

    bool IsCharacter = false;

    Vector3 movement;
    Vector3 CharacterPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        TargetPoint = 0;
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
        else if (IsCharacter == false) 
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
    {                                                  // afin de stopper les déplacemnts. De plus, je vide les info de position
        if (collision.CompareTag("Player") || collision.CompareTag("Vision"))            
        {
            CharacterPosition = Vector2.zero; IsCharacter = false;
            IsCharacter = false;
            Debug.Log("test");
        }
    }

    void Déplacement()
    {
        if (Vector2.Distance(transform.position, PatrolPoints[TargetPoint].position) < 0.1f)
        {
            IncreaseTargetInt();
        }
        Agent.SetDestination(Vector3.MoveTowards(transform.position, PatrolPoints[TargetPoint].position, speed * Time.deltaTime));
    }
        
    void IncreaseTargetInt()
    {
        TargetPoint++;
        if (TargetPoint >= PatrolPoints.Length)
        {
            TargetPoint = 0;
        }
    }

    void DéplacementToCharacter()
    {
        Agent.SetDestination(CharacterPosition);
    }
}
