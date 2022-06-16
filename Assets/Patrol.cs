using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;


public class Patrol : MonoBehaviour
{
    NavMeshAgent agent;

   // float random_destination_radius = 10.0f;
    
    Vector3 InitialPosition;

    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    //private float speed = 4f;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitialPosition = agent.transform.position;
        
        
    }

    void Update()
    {
        Transform wp = waypoints[currentWaypointIndex];
        if (Vector3.Distance(agent.transform.position, wp.position) < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    [Task]
    bool MoveToWaypoints()
    {
        Transform wp = waypoints[currentWaypointIndex];

        agent.SetDestination(wp.position);
        return true;

    }

    
/*
    [Task]
    void MoveToWaypoints()
    {
        Transform wp = waypoints[currentWaypointIndex];
        
        transform.position = Vector3.MoveTowards(transform.position, wp.position, speed * Time.deltaTime);
        agent.SetDestination(transform.position);
    }
*/
    
    float tiempo = 5.0f;
    
    [Task]
    bool Contador()
    {
        /*var distancia = this.transform.position + (Random.insideUnitSphere * random_destination_radius);
        agent.SetDestination(distancia);
        return true;*/
        
        
        tiempo -= Time.deltaTime;
        if (tiempo <= 0){
            return false;
            
        }
        return true;
    }

    
    Vector3 enemyLastSeenPosition;
    
    bool awareOfPlayer = false;

    [Task]
    void GoLastSeenPosition()
    {
        /*var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyLastSeenPosition = player.transform.position;
        agent.SetDestination(enemyLastSeenPosition);
*/
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        RaycastHit hit;

        Vector3 direction = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                enemyLastSeenPosition = player.transform.position;
                awareOfPlayer = true;
                Task.current.Fail();
            }
            
        }
        
        
 /*       if (Vector3.Distance(agent.transform.position, enemyLastSeenPosition) < 2f)
        {
        
        }
        */
        
    }   

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
                tiempo = 5.0f;
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