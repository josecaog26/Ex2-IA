using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class Sentry : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 InitialPosition;
    public GameObject door;

    public int vida = 4;

    public GameObject BalaInicio;
    public GameObject BalaPrefab;
    public float BalaVelocidad;

    public float lastShoot = 0f;
    public float ritmoDisparo = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitialPosition = agent.transform.position;


        door = GameObject.Find("Door");
        
    }

    Vector3 enemyLastSeenPosition;
    
    bool awareOfPlayer = false;


    float tiempo = 15f;
    
    [Task]
    bool Contador()
    {
 
        tiempo -= Time.deltaTime;
        if (tiempo <= 0){
            return false;
            
        }
        return true;
    }

    [Task]
    void GoLastSeenPosition()
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
                Task.current.Fail();
            }
            
        }  
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
                tiempo = 15.0f;
                RotarPlayer();

                if (Time.time - lastShoot >= ritmoDisparo)
                {
                    Disparar();

                }
                return true;
            }
            agent.SetDestination(hit.point);
            
        }
        return false;
    }

    void RotarPlayer()
    {
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 mouseOnScreen = (Vector2)Camera.main.WorldToViewportPoint(enemyLastSeenPosition);
        mouseOnScreen.z = 0;

        Vector3 direction = mouseOnScreen - positionOnScreen;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }

    void Disparar()
    {
        GameObject BalaTemporal = Instantiate(BalaPrefab, BalaInicio.transform.position, BalaInicio.transform.rotation) as GameObject;
 
        Rigidbody rb = BalaTemporal.GetComponent<Rigidbody>();
       
        rb.AddForce(transform.forward * BalaVelocidad);
 
        Destroy(BalaTemporal, 4.0f);

        lastShoot = Time.time;
        
    }

    [Task]
    void ChasePlayer()
    {
        float rangoMax = 4f;
        float distancia = Vector3.Distance(agent.transform.position, enemyLastSeenPosition);
        var patrol = GameObject.FindGameObjectWithTag("Sentry").GetComponent<Transform>();
        var ActualPosition = patrol.transform.position;

        if (distancia >= rangoMax)
        {
            agent.SetDestination(enemyLastSeenPosition);
        }
        else
        {
            agent.SetDestination(ActualPosition);
        }
        
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

    [Task]
    void CloseDoors()
    {
        door.SetActive(true);
        Task.current.Succeed();
    }
}
