using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;

public class BaseEnemyClass : MonoBehaviour, IDamageable
{
    // bedo 2026-03-04

    // försöker använda polymorphism för första gången

    [SerializeField, Range(1, 500)] float health;
    [HideInInspector] public bool knocked;
    [SerializeField] private Vector3 hitboxSize;
    [SerializeField] private float forwardOffset;
    
    public bool activate;

    private GameObject player;
    
    
    public Rigidbody rb;
    public Animator anim;

    [SerializeField] LayerMask maskToAttack;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("Player");



    }

    public virtual void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            anim.SetTrigger("Attacking");
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 25)
        {
            Activate();
        }
    }

    public void Attack(float damage) // attack genom att skapa en hitbox What really????
    {
        Collider[] hitTargets = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardOffset), hitboxSize, transform.rotation,maskToAttack);
        print("attack");
        foreach(Collider collider in hitTargets)
        {
            print("träffar " + collider.gameObject.name);
            
            IDamageable obj = collider.GetComponent<IDamageable>();

            obj.TakeDamage(damage);

            anim.ResetTrigger("Attacking");

        }
    }

    public void Activate()
    {
        activate = true;
    }

    

    public void TakeDamage(float damageAmount)
    {
        print("ajjj" + gameObject.name);
        health -= damageAmount;

        StartCoroutine(Knockback(1.5f, 3));

        if (health <= 0) StartCoroutine(Die()); // jag vet inte jag tror han dog?
    }

    public virtual IEnumerator Die() // oh no, our table... its broken!
    {
        knocked = true;
        anim.SetBool("Fallen", true);
        StartCoroutine(Knockback(14, 7));

        yield return new WaitForSeconds(7);

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

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardOffset), hitboxSize);
    }
}
