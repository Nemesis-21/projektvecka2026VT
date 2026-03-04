using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;

public class BaseEnemyClass : MonoBehaviour
{
    // bedo 2026-03-04

    // försöker använda polymorphism för första gången

    [SerializeField, Range(1, 250)] float health;
    [HideInInspector] public bool knocked;
    private bool hitDetected;
    private RaycastHit hit;
    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0) StartCoroutine(Die()); // jag vet inte jag tror han dog?
    }

    public virtual void Attack(Vector3 box) // attack genom att skapa en hitbox What really????
    {
        hitDetected = Physics.BoxCast(box, new Vector3(0, 0, 0), transform.forward, transform.rotation);
    }

    public virtual void takeDamage(float amount)
    {
        health -= amount;
    }

    public virtual IEnumerator Die() // oh no, our table... its broken!
    {
        knocked = true;

        yield return new WaitForSeconds(7);

        Destroy(gameObject);

        yield return null;
    }

    public virtual IEnumerator Knockback(float power, float duration) // public function för att skicka enemy tillbaka genom knockback params.
    {
        if (knocked) yield return null;

        knocked = true;
        rb.AddForce(transform.forward * -power, ForceMode.Impulse);
        anim.SetBool("Fallen", true);

        yield return new WaitForSeconds(duration);

        knocked = false;
        anim.SetBool("Fallen", false);


        yield return null;
    }

    public void OnDrawGizmos()
    {
        
    }
}
