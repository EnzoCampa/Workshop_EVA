using UnityEngine;
using UnityEngine.AI;

public class EnnemieAI : MonoBehaviour
{
    public NavMeshAgent Agent;
    [SerializeField] Transform Target;
    [SerializeField] PlayerController Player;

    bool IsCharacter = false;

    Vector3 CharacterPosition;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }
    private void Update()
    {
        if (IsCharacter)
        {
            Debug.Log("test");
            DéplacementToCharacter();
        }
        else
        {
            
            Déplacement();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("InsideCollision");
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
        if (collision.CompareTag("Vision"))
        {
            Debug.Log("InsideCollision");
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("InsideCollision");
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
        if (collision.CompareTag("Vision"))
        {
            Debug.Log("InsideCollision");
            CharacterPosition = Target.position;
            IsCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("OutsideCOllistion");
            CharacterPosition = Vector2.zero; IsCharacter = false;
            IsCharacter = true;
        }
        if (collision.CompareTag("Vision"))
        {
            Debug.Log("OutsideCOllistion");
            CharacterPosition = Vector2.zero; IsCharacter = false;
            IsCharacter = true;
        }
    }   
    public void Déplacement()
    {

    }

    public void DéplacementToCharacter()
    {
        Agent.SetDestination(CharacterPosition);
    }
}
