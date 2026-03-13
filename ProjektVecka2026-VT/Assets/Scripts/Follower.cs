using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Ref till Unitys AI funktioner

// bedo 2026-03-02

// N�r aktiv, kommer f�lja spelaren genom n�rmaste rutt

public class Follower : MonoBehaviour 
{
    public GameObject destination; //Ref till det objektet vi vill att Enemyn ska förfölja
    public NavMeshAgent agent; //Ref till vår NavMeshAgent komponent
    Rigidbody rb;
    public BaseEnemyClass bec;

    public bool stopFollow = false;
    float time = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Ref till vår NavMeshAgent komponent

        destination = GameObject.FindWithTag("Player");

        bec = GetComponent<BaseEnemyClass>();

        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        if (!bec.activate) return;
        if (bec.dead) return;

        

        if (!bec.knocked) // Kolla ifall man e knocked eller itne
        {
            if (bec.anim.GetBool("Attacking")) return;
            
            agent.enabled = true;
            
            if (gameObject.GetComponent<Knight>() != null)
            {
                if (!gameObject.GetComponent<Knight>().passive) bec.anim.SetBool("Running", true);
                else bec.anim.SetBool("Running", false);
            }
            else
            {
                bec.anim.SetBool("Running", true);
            }

            
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            //rb.useGravity = false; // super viktig att disabla gravity för att annars blir risken att fiended blir stuck 100 gånger större jag vet inte varför det blir så fuck you unity navigation more like unity naviGAY
            if (!stopFollow) agent.SetDestination(destination.transform.position); //R�r dig emot destination
        }
        else
        {
            agent.enabled = false;
            //rb.useGravity = true;
            time = 0;

            if (gameObject.GetComponent<Knight>() != null)
            {
                if (gameObject.GetComponent<Knight>().passive) bec.anim.SetBool("Running", false);
                else bec.anim.SetBool("Running", true);
            }
            else
            {
                bec.anim.SetBool("Running", false);
            }
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

