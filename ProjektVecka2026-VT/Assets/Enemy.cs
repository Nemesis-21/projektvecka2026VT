using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // bedo - 2026-03-03

    // common stuff for every enemy ai to handle.

    // useless script now lowkey

    private Rigidbody rb;
    private Animator anim;
    [SerializeField, Range(1, 100)]public float health;
    [HideInInspector] public bool knocked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) StartCoroutine(Die()); // jag vet inte jag tror han dog?
    }

    public IEnumerator Knockback(float power, float duration) // public function för att skicka enemy tillbaka genom knockback params.
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

    //public void takeDamage(float amount)
    //{
    //    health -= amount;
    //}

    public IEnumerator Die() // oh no, our table... its broken!
    {
        knocked = true;

        yield return new WaitForSeconds(7);

        Destroy(gameObject);
        
        yield return null;
    }

    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent called at " + Time.time + " with a value of " + s);
    }
}
