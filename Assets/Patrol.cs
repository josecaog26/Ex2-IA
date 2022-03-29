using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;


public class Patrol : MonoBehaviour
{
    NavMeshAgent agent;

    float random_destination_radius = 10.0f;
    Vector3 InitialPosition;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitialPosition = agent.transform.position;
        
    }
    

    [Task]
    bool SetDestination_Random()
    {
        var distancia = this.transform.position + (Random.insideUnitSphere * random_destination_radius);
        agent.SetDestination(distancia);
        return true;
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
        if(agent.transform.position==InitialPosition)
        {
                Task.current.Succeed();
        }
        
    }

}