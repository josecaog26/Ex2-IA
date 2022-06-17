using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour
{

    public Canvas CanvasObject;

    public GameObject npc;

    public NavMeshAgent agent;

    
    // Start is called before the first frame update
    void Start()
    {
        CanvasObject.GetComponent<Canvas>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        EndGame();
    }

    void EndGame()
    {
        
        if(Vector3.Distance(agent.transform.position, npc.transform.position) <= 2f)
        {
            CanvasObject.GetComponent<Canvas>().enabled = true;
        }
        
        


    }
}
