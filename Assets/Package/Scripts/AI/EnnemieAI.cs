using System.Collections;
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

    [Header("EnemyPatrol")]
    public Transform[] PatrolPoints;
    public int TargetPoint;
    public float speed;

    private Rigidbody2D rb;
    public bool IsCharacter = false;
    public Vector3 CharacterPosition = Vector3.zero;

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
        if (IsCharacter == false)
        {
            Déplacement();
        }
        else if (IsCharacter == true)
        {
            DéplacementToCharacter();
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

    public void DéplacementToCharacter()
    {
        float distance = Vector3.Distance(rb.transform.position, CharacterPosition);

        if (distance < 1f)
        {
            Debug.Log("Corountine entry");
            StartCoroutine(Delay(10));
            Debug.Log("Corountine Sortie");
            CharacterPosition = Vector2.zero;
            IsCharacter = false;
        }
        else
        {
            Agent.SetDestination(CharacterPosition);
        }
    }

    private IEnumerator Delay(int delay)
    { 
        yield return new WaitForSeconds(delay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

}


