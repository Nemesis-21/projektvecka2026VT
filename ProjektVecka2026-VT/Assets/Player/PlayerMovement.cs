//Edgar Åberg, 2026-03-02

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour, InputSystem_Actions.IPlayerActions, IDamageable
{
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityScale;
    [SerializeField] float GroundedDistance;
    [SerializeField] LayerMask ground;

    [Header("Attack Variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemylayer;

    
    [Header("Combo counter TMP on HUD")]
    [SerializeField] GameObject streakCheck;
    [Header("Regulare ol´ cam")]
    [SerializeField] Camera mainCam;

    [Header("Debug / Read Only")]
    public Vector2 movedirection;
    public Vector2 mousePos;
    public int comboCounter = 0;
    public bool walking=false;
    


    //other compomponents that u get with Awake()
    private Animator animatorStreak;
    private TextMeshProUGUI textStreak;
    private PlayerInput pInput;
    private Rigidbody rb;
    private Animator animator;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
        animatorStreak = streakCheck.GetComponent<Animator>();
        textStreak = streakCheck.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Locomotion
        lookAround();
        rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.y * moveSpeed);
        
        
        
        
        //HUD
        textStreak.text = "X" + comboCounter;
        //Animator
        animator.SetBool("Walk", walking);
        animator.SetBool("OnGround", IsGrounded());


    }

    private void FixedUpdate()
    {
        //Gravity
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
    public void lookAround()
    {
        
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ground))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = targetPos - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0);
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {

        movedirection = context.ReadValue<Vector2>();

        if (context.performed)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
           


    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
        
        if (animatorStreak && context.performed) 
        {
            animator.SetTrigger("Attack");


            Collider[] HitEnemys = Physics.OverlapSphere(attackPoint.position, attackRadius, enemylayer);
            foreach (Collider enemyCollider in HitEnemys)
            {
                IDamageable obj = enemyCollider.GetComponent<IDamageable>();
                if (obj!=null)
                {
                    obj.TakeDamage(10);
                    comboCounter++;
                    animatorStreak.SetTrigger("Combo");
                }
            }

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mousePos = context.ReadValue<Vector2>();
        }
        
    }

    bool IsGrounded()
    {
        if (rb.linearVelocity.y == 0)
        {
            return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundedDistance);
        }
        else
        {
            return false;
        }
    }
}
