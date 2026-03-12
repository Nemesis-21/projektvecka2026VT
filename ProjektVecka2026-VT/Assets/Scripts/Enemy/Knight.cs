using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Knight : BaseEnemyClass // Målet med skript

// här är planen
// scripten kollar efter andra Knight componenter och kollar ifall de har "Attacking" bool på, om de har det så är de "passive" mode och 
// de försöker omringa spelaren för att vara intimadating.

// det här är den svåraste delen men vi får se.
{
    private Follower followerScript;
    [HideInInspector] public static bool attacking;
    
    public bool passive;
    private float timer = 0;
    private float passiveTimer = 0;
    private float attackTimer = 0;
    private float attackDuration;
    private float patience;
    private float waitGauge;
    private bool called = false;
    private int strafeDirection = 0;
    




    public override void Start()
    {
        base.Start();
        followerScript = gameObject.GetComponent<Follower>();
        StartCoroutine(Check());
        waitGauge = Random.Range(1.5f, 4f);
        patience = Random.Range(7, 14);
        attackDuration = Random.Range(6, 12);
    }

    public override void Update()
    {
        
        
        if (!activate)
        {
            base.Update();

            return;
        }
        
        if (!passive && !knocked)
        {
            base.Update();
            followerScript.agent.speed = 5;
            followerScript.agent.stoppingDistance = 2;
            anim.SetBool("WalkFront", false);
            anim.SetBool("WalkBack", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkRight", false);

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attacking = false;
                passive = true;
                attackTimer = 0;
                attackDuration = Random.Range(6, 12);
            }
        }
        else
        {
            if (anim.GetBool("Attacking")) return;
            if (knocked) return;

            followerScript.agent.speed = 1.75f;
            followerScript.agent.stoppingDistance = 0;

            passiveTimer += Time.deltaTime;

            if (passiveTimer >= patience) StartCoroutine(Check());

            if (Vector3.Distance(transform.position, player.transform.position) < 8)
            {
                anim.SetBool("WalkFront", false);


                followerScript.agent.speed = 0;

                timer += Time.deltaTime;

                if (Vector3.Distance(transform.position, player.transform.position) < 6)
                {
                    anim.SetBool("WalkBack", true);



                    timer = 0;
                    called = false;

                    StopCoroutine(Strafe());
                    anim.SetBool("WalkLeft", false);
                    anim.SetBool("WalkRight", false);

                    followerScript.agent.Move(-transform.forward * 1.75f * Time.deltaTime);
                    
                    
                }
                else
                {
                    anim.SetBool("WalkBack", false);
                    
                    
                }

                Vector3 direction = (player.transform.position - transform.position).normalized;
                direction.y = 0; // Keep the agent upright

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
                }

                if (called)
                {
                    if (strafeDirection == 1)
                    {
                        anim.SetBool("WalkLeft", true);
                        followerScript.agent.Move(-transform.right * 1.75f * Time.deltaTime);
                    }
                    else if (strafeDirection == 2)
                    {
                        anim.SetBool("WalkRight", true);
                        followerScript.agent.Move(transform.right * 1.75f * Time.deltaTime);
                    }
                }


                if (timer >= waitGauge && !called) StartCoroutine(Strafe());
            }
            else
            {
                anim.SetBool("WalkFront", true);
                StopCoroutine(Strafe());
                anim.SetBool("WalkLeft", false);
                anim.SetBool("WalkRight", false);
                called = false;
                timer = 0;

            }
        }

        
    }

    private IEnumerator Strafe()
    {
        called = true;

        
        strafeDirection = (Random.value > 0.5f) ? 1 : 2;

        
        yield return new WaitForSeconds(Random.Range(3f, 9f));

        
        anim.SetBool("WalkRight", false);
        anim.SetBool("WalkLeft", false);
        strafeDirection = 0;
        timer = 0;
        waitGauge = Random.Range(1.5f, 4f);
        called = false;
    }

    public IEnumerator Check()
    {
        if (!attacking)
        {
            passive = false;
            attacking = true;
        }
        else
        {
            passive = true;
            
        }

        passiveTimer = 0;
        patience = Random.Range(7, 14);

        yield return null;
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        if (passive)
        {
            passive = false;
            attacking = true;
        }
    }

    
}
