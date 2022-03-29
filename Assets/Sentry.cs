using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class Sentry : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 InitialPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitialPosition = agent.transform.position;
        
    }

    Vector3 enemyLastSeenPosition;
    
    bool awareOfPlayer = false;


    [Task]
    bool CanSeePlayer()
    {
        
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        RaycastHit hit;

        Vector3 direction = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                enemyLastSeenPosition = player.transform.position;
                awareOfPlayer = true;
                return true;
            }
            agent.SetDestination(hit.point);
            
        }
        return false;
    }

    [Task]
    void ChasePlayer()
    {
        agent.SetDestination(enemyLastSeenPosition);
        Task.current.Succeed();

    }

    [Task]
    bool AwareOfPlayer()
    {
        return awareOfPlayer;
    }

    [Task]
    void ComeBack()
    {
        agent.SetDestination(InitialPosition);
        Task.current.Succeed();
    }
}
