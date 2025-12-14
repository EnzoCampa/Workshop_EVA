using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using UnityEngine.Splines;
using NUnit.Framework.Constraints;
using UnityEngine.SceneManagement;

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
    private Renderer meshRenderer;


    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
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
            meshRenderer.enabled = IsCharacter;
        }
        else if (IsCharacter == true)
        {
            DéplacementToCharacter();
            meshRenderer.enabled = IsCharacter;
        }
    }
    void Déplacement()
    {
        if (Vector2.Distance(transform.position, PatrolPoints[TargetPoint].position) < 0.1f)
        {
            IncreaseTargetInt();
        }

        Agent.SetDestination(PatrolPoints[TargetPoint].position);

        Vector3 moveDir = Agent.desiredVelocity;

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
            // Utilise la transition pour charger la scène de défaite (index 3)
            if (SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadSceneWithTransition(3);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(3); // Fallback
            }
        }
    }

}


