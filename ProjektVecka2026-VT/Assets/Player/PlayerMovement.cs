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
    [SerializeField] float maxhp;
    

    [Header("Attack Variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemylayer;

    
    [Header("Combo counter TMP on HUD")]
    [SerializeField] GameObject streakCheck;
    [Header("Regulare ol´ cam")]
    [SerializeField] Camera mainCam;
    [Header("Healthbar in HUD animator")]
    [SerializeField] Animator hpbar;

    [Header("Debug / Read Only")]
    public float currentHp;
    public Vector2 movedirection;
    public Vector2 mousePos;
    public int comboCounter = 0;
    public bool walking=false;
    public float movementWait=0;
    


    //other compomponents that u get with Awake()
    private Animator animatorStreak;
    private TextMeshProUGUI textStreak;
    private PlayerInput pInput;
    private Rigidbody rb;
    private Animator animator;

    //Camera
    private float changeY;
    private float changeZ;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
        animatorStreak = streakCheck.GetComponent<Animator>();
        textStreak = streakCheck.GetComponent<TextMeshProUGUI>();
        currentHp = maxhp;
    }

    void Update()
    {
        //Locomotion
        lookAround();

        if (Actionable())
        {
            rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.y * moveSpeed);
        }

        if (movementWait > 0)
        {
            movementWait -= 0.1f;
        }
        else
        {
            movementWait = 0;
        }

            
        
        
        //HUD
        textStreak.text = "X" + comboCounter;
        //Animator
        animator.SetBool("Walk", walking);
        animator.SetBool("OnGround", IsGrounded());
        //Healthbar
        if (hpbar) hpbar.SetFloat("Hp", 1f-(currentHp/ maxhp));
        //Camera
        Camera();
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
        
        
        if (context.performed && Actionable()) 
        {
            rb.linearVelocity = Vector3.zero;
            animator.SetTrigger("Attack");



            if (!IsGrounded())
            {
                rb.linearVelocity = new Vector3(0, 20, 0);
            }




            
            Collider[] HitEnemys = Physics.OverlapSphere(attackPoint.position, attackRadius, enemylayer);
            foreach (Collider enemyCollider in HitEnemys)
            {
                IDamageable obj = enemyCollider.GetComponent<IDamageable>();
                if (obj!=null)
                {
                    obj.TakeDamage(10);
                    comboCounter++;
                    if (animatorStreak) animatorStreak.SetTrigger("Combo");
                }
            }

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && Actionable())
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

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }

    }
    bool IsGrounded()
    {
            return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundedDistance);        
    }

    bool Actionable()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
    }
    public void Camera()
    {
        if (transform.position.y > 4f)
        {
            changeY = transform.position.y;
        }
        else
        {
            changeY = 4f;
        }



        changeZ = Mathf.Max(transform.position.z - 10f, -10f);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(transform.position.x, changeY, changeZ), Time.deltaTime * 1f);
    }


}
