using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Ref till Unitys AI funktioner

// bedo 2026-03-02

// N�r aktiv, kommer f�lja spelaren genom n�rmaste rutt

public class Follower : MonoBehaviour 
{
    public GameObject destination; //Ref till det objektet vi vill att Enemyn ska förfölja
    NavMeshAgent agent; //Ref till vår NavMeshAgent komponent
    Rigidbody rb;

    float time = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Ref till vår NavMeshAgent komponent

        destination = GameObject.FindWithTag("Player");

        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

        if (!gameObject.GetComponent<BaseEnemyClass>().knocked) // Kolla ifall man e knocked eller itne
        {
            agent.enabled = true;
            rb.useGravity = false; // super viktig att disabla gravity för att annars blir risken att fiended blir stuck 100 gånger större jag vet inte varför det blir så fuck you unity navigation more like unity naviGAY
            agent.SetDestination(destination.transform.position); //R�r dig emot destination
        }
        else
        {
            agent.enabled = false;
            rb.useGravity = true;
            time = 0;
        }

        
        if (time <= 10) // gör en timer för 10 sekunder
        {
            time += Time.deltaTime;
        }
        else
        {
            StartCoroutine(pathfindCheck()); // kalla funktionen för att kolla ifall man e fast eller inte.
            time = 0;
        }

        
    }

    private IEnumerator pathfindCheck() // inte ideal men det funkar för nu och fixar den flesta navigation buggar.
    {
        float distance = agent.remainingDistance;
        Debug.Log("Is agent moving towards destination check? " + distance + " versus " + (distance - 10));

        yield return new WaitForSeconds(5);

        if (distance - 10 >= agent.remainingDistance)
        {
            Debug.LogWarning("Agent did not pass atleast 5 distance, re-enabling...");
            
            agent.enabled = false;
            rb.AddForce(transform.forward * -25);

            yield return new WaitForSeconds(0.5f);

            agent.enabled = true;
        }
        else
        {
            Debug.Log("Agent passed the test");
        }

        yield return null;
    }

}

