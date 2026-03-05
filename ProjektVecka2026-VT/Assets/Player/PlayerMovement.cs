//Edgar Åberg, 2026-03-02
//While this is not the most impressive thing, I designed the whole player including animations and such.
//I would prefare too add a better way to sync animations with the players action but it think that my "cheat" solutions work good, but would not scale well for a bigger game.
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
    [SerializeField] float GroundedDistance;//0.1f is a good value
    [SerializeField] LayerMask ground;//Plane layer that the mouse compares its self too. 
    [SerializeField] float maxhp;
    

    [Header("Attack Variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemylayer;
    [SerializeField] ParticleSystem hitSpark;

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


        if (Actionable())
        {
            //Locomotion
            lookAround();
            rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.y * moveSpeed);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.2f && CanAttack())
        {
            //I want the hitbox to be thrown out closer to when the attack is actually going on. This is the easiest work around to syncing the animation with the hitbox
            Attack();

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
        //A Ray basically compares the the mouse position on the screen to a plane in the world.

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

        //For animation
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
        
        
        if (context.performed) 
        {
            if (IsGrounded()&&(CanAttack()) || Actionable())
            {
                animator.SetTrigger("Attack");
                lookAround();
                rb.linearVelocity = Vector3.zero;


                if (!IsGrounded())
                {
                    rb.linearVelocity = new Vector3(0, 20, 0);
                }
                else
                {
                    rb.AddForce(transform.forward * 2, ForceMode.Impulse);
                }
                
            }

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Simply checks if u are grounded and can do anything.
        if (IsGrounded() && Actionable())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Just to get the value of the mouse position on screen
        if (context.performed)
        {
            mousePos = context.ReadValue<Vector2>();
        }
        
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
                hitSpark.Clear();
                rb.linearVelocity = Vector3.zero;
                Debug.Log("Hit");
                obj.TakeDamage(1);
                comboCounter++;
                if (animatorStreak) animatorStreak.SetTrigger("Combo");
                hitSpark.Play();
            }
        }
    }
    bool IsGrounded()
    {
        //Just checks if the player is grounded.
            return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundedDistance);        
    }

    bool Actionable()
    {
        //Checks the animator to se if the player i a idle state
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
    }

    bool CanAttack()
    {
        //Checks the animator to se if the player i a attacking state
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }
    public void Camera()
    {
        //This was orginally a standalone code but it is easier to keep track off when its a part of the player script.
        //Becouse of the fixed camera, i did not need to make it flexible.
        //It just zooms in when the player walks past a position. The camera also follows the players Y cordinate.
        if (transform.position.y > 4f)
        {
            changeY = transform.position.y;
        }
        else
        {
            changeY = 4f;
        }


        //I also added smothing to the movment.
        changeZ = Mathf.Max(transform.position.z - 10f, -10f);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(transform.position.x, changeY, changeZ), Time.deltaTime * 1f);
    }


}
