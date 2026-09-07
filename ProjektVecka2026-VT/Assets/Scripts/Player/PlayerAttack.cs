using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //Edgar.Aberg 26-09-01
    [Header("Attack Variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemylayer;
    private Animator animator;
    private Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAttack(InputValue value)
    {
        //very rigid system but it gets the job done. 
        if (value.isPressed) animator.SetTrigger("Attack");

    }

    public void Attack()
    {
        //Makes a list of all Enemy GameObjects that collides with a overlapsphere.
        //It then itterates throuh all the enemys to damage em. This uses the IDamageble interface to make it easier to manage.
        Collider[] HitEnemys = Physics.OverlapSphere(attackPoint.position, attackRadius, enemylayer);
        foreach (Collider enemyCollider in HitEnemys)
        {
            IDamageable obj = enemyCollider.GetComponent<IDamageable>();
            if (obj != null)
            {

                rb.linearVelocity = Vector3.zero; ;
                obj.TakeDamage(1);
                //GetCombo();

            }
        }
    }
}
