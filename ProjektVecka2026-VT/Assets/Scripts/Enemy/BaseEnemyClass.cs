using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Audio;

public class BaseEnemyClass : MonoBehaviour, IDamageable
{
    // bedo 2026-03-04

    // försöker använda polymorphism för första gången

    [SerializeField, Range(1, 500)] float health;
    [HideInInspector] public bool knocked;
    [SerializeField] private Vector3 hitboxSize;
    [SerializeField] private float forwardOffset;
    [SerializeField] float damage;
    [SerializeField] float attackDistance;
    [SerializeField] float activateRange;

    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource swingSound;
    [SerializeField] AudioSource hurtSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] public AudioSource tauntSound;
    

    public bool activate;
    [HideInInspector] public bool dead;

    [HideInInspector] public GameObject player;
    
    
    public Rigidbody rb;
    public Animator anim;

    [SerializeField] LayerMask maskToAttack;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("Player");



    }

    public virtual void Update()
    {
        if (dead) return;
        
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            anim.SetBool("Attacking", true);

            
        }

        if (Vector3.Distance(transform.position, player.transform.position) < activateRange)
        {
            Activate();
        }
    }

    public void Attack() // attack genom att skapa en hitbox What really????
    {
        Collider[] hitTargets = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardOffset), hitboxSize, transform.rotation,maskToAttack);
        print("attack");
        swingSound.Play();
        foreach(Collider collider in hitTargets)
        {
            print("träffar " + collider.gameObject.name);
            
            IDamageable obj = collider.GetComponent<IDamageable>();
            hitSound.Play();
            obj.TakeDamage(damage);

        }
    }

    public void distanceCheck() // if player is not close stop attacking for FUCKS SAKE
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            anim.SetBool("Attacking", false);


        }
    }

    public virtual void Activate()
    {
        activate = true;
    }

    

    public virtual void TakeDamage(float damageAmount)
    {
        print("ajjj" + gameObject.name);
        health -= damageAmount;

        anim.SetBool("Attacking", false);
        anim.SetBool("WalkFront", false);
        anim.SetBool("WalkBack", false);
        anim.SetBool("WalkLeft", false);
        anim.SetBool("WalkRight", false);
        anim.SetBool("Running", false);

        hurtSound.Play();

        if (!knocked) StartCoroutine(Knockback(15, 3));

        if (health <= 0) StartCoroutine(Die()); // jag vet inte jag tror han dog?
    }

    public virtual IEnumerator Die() // oh no, our table... its broken!
    {
        knocked = true;
        anim.SetBool("Fallen", true);
        deathSound.Play();
        StartCoroutine(Knockback(30, 30));

        yield return new WaitForSeconds(3.5f);

        Destroy(gameObject);

        

        yield return null;
    }

    public IEnumerator Knockback(float power, float duration) // public function för att skicka enemy tillbaka genom knockback params.
    {
        if (knocked) yield return null;

        knocked = true;
        rb.AddForce(transform.forward * -power, ForceMode.Impulse);
        

        yield return new WaitForSeconds(duration);

        knocked = false;
        

        Debug.Log("YEEEOUUCH, enemy knocked");
        yield return null;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardOffset), hitboxSize);
    }
}
