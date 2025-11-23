using System;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("test");
            IsCharacter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("test");
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
