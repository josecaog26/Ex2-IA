using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;


public class PlayerController : MonoBehaviour {

    public Camera cam;

    public NavMeshAgent agent;
    public GameObject door;
    public GameObject door2;

    public bool IsDetected;
    public int vida = 5;

    public GameObject BalaInicio;
    //Agregan Bala Prefab
    public GameObject BalaPrefab;
    //Agregar Bala Velocidad
    public float BalaVelocidad;
    public float lastShoot = 0f;
    public float ritmoDisparo = 2f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        door = GameObject.Find("Door");
        door2 = GameObject.Find("Door2");

    }

    // Update is called once per frame
    void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            
           /* Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {*/
                
                RotarPlayer();
                if (Time.time - lastShoot >= ritmoDisparo)
                {
                    Disparar();

                }
                
            //}
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        
        OpenDoor1();
        CloseDoor1();
        OpenDoor2();
        CloseDoor2();
        
    }

    void RotarPlayer()
    {
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 mouseOnScreen = (Vector2)Camera.main.WorldToViewportPoint(Input.mousePosition);
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

    void OpenDoor1()
    {
        if (Vector3.Distance(agent.transform.position, door.transform.position) <= 1.5f)
        {
            door.SetActive(false);
        }
    }
    
    void CloseDoor1()
    {
        if (Vector3.Distance(agent.transform.position, door.transform.position) >= 1.5f)
        {
            door.SetActive(true);
        }
    }
    void OpenDoor2()
    {
        if (Vector3.Distance(agent.transform.position, door2.transform.position) <= 1.5f)
        {
            door2.SetActive(false);
        }
    }
    
    void CloseDoor2()
    {
        if (Vector3.Distance(agent.transform.position, door2.transform.position) >= 1.5f)
        {
            door2.SetActive(true);
        }
    }

    [Task]
    void CloseDoors()
    {
        
    }

    float tiempo = 15.0f;

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
    bool Detected()
    {

        var patrol = GameObject.FindGameObjectWithTag("Patrol").GetComponent<Transform>();
        var sentry = GameObject.FindGameObjectWithTag("Sentry").GetComponent<Transform>();
        
        RaycastHit hit;

        Vector3 direction = patrol.transform.position - transform.position;
        Vector3 direction2 = sentry.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Patrol"))
            {
                tiempo = 5.0f;
                IsDetected = true;
                return true;
            }
            
        }
        if (Physics.Raycast(transform.position, direction2, out hit))
        {
            if (hit.collider.CompareTag("Sentry"))
            {
                tiempo = 5.0f;
                IsDetected = true;
                return true;
            }
            
        }
        IsDetected = false;
        return false;
    }


}