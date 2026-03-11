//Edgar Åberg, 2026-03-02
//While this is not the most impressive thing, I designed the whole player including animations and such.
//I would prefare too add a better way to sync animations with the players action but it think that my "cheat" solutions work good, but would not scale well for a bigger game.
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour, PlayerInput.IPlayerActions, IDamageable
{
    [Header("Movement Variables")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpHeight;
    [SerializeField] public float gravityScale;
    [SerializeField] public float maxhp;
    

    [Header("Attack Variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemylayer;
    

    public float currentHp;
    [HideInInspector] public int score = 0;
    [HideInInspector] public float comboTimer = 0;
    [HideInInspector] public int comboCounter = 0;

    private Vector3 movedirection;
    //other compomponents that u get with Awake()
    private Rigidbody rb;
    private Animator animator;
    private Camera m_MainCamera;


    void Awake()
    {
        currentHp = maxhp;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;
        

    }

    void Update()
    {
        
        if (currentHp > 0)
        {
            //Locomotion
            if (Actionable())
            {
                rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.z * moveSpeed);

                if (movedirection != Vector3.zero)
                {
                    //LookAround//
                    Quaternion targetRotation = Quaternion.LookRotation(movedirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
                }
            }
            
            ComboUpdate();
            //Animator
            animator.SetBool("OnGround", IsGrounded());
            //Camera
            CameraMovement();
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            if (!GotHit()) animator.SetTrigger("Knockdown");
            m_MainCamera.transform.SetPositionAndRotation(new Vector3(transform.position.x, 15, transform.position.z), Quaternion.Euler(90f, 0f, 0f));
            //Death();
        }

        
    }







    //INPUT////////////////////////////////////////
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readValue= context.ReadValue<Vector2>();
        movedirection = new Vector3(readValue.x, 0, readValue.y).normalized;
        animator.SetBool("Walk", context.performed);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //very rigid system but it gets the job done. 
        if (context.performed) animator.SetTrigger("Attack");

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Simply checks if u are grounded and can do anything.
        if (IsGrounded() && Actionable()) ChangeVelocityY(jumpHeight);
    }

    //CAMERA/////////////////////////////////
    public void CameraMovement()
    {
        //I also added smothing to the movment.
        float targetZ = Mathf.Max(transform.position.z - 10f, -10f);
        Vector3 CameraTargetPos = new Vector3(Mathf.Max(transform.position.x, 0f), transform.position.y + 5f, targetZ);
        m_MainCamera.transform.position = Vector3.Lerp(m_MainCamera.transform.position, CameraTargetPos, Time.deltaTime * 1f);
    }



    //MOVEMENT/////////////////////////////////////////////////////////
    public void ChangeVelocityX(float newX)
    {
        //Made to communicate with Animator events//
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        rb.AddForce(transform.forward * newX, ForceMode.Impulse);//X

    }

    public void ChangeVelocityY(float newY)
    {
        //Made to communicate with Animator events//
        rb.AddForce(transform.up * newY, ForceMode.Impulse);//Y

    }
    bool IsGrounded()
    {
        //Just checks if the player is grounded.
        //0.1f is the distance that the ray cast will travel until it countes you as grounded.
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f);
    }

    private void FixedUpdate()
    {
        //Gravity
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    //ATTACK/ GET ATTACKED////////////////////////////////////////////


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
                GetCombo();

            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        animator.SetTrigger("GetHit");
        currentHp -= damageAmount;

    }
    bool GotHit()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit");
    }

    bool Actionable()
    {
        //Checks the animator to se if the player i a idle state
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("Actionable");
    }
    public void Death()
    {
        SceneManager.LoadSceneAsync(1);
    }
    //COMBO//////////////////////////////////////////////
    public void GetCombo()
    {
        comboTimer = 5f;
        comboCounter++;
        score+=(100* comboCounter);
    }

    public void ComboUpdate()
    {
        comboTimer -= 8f*Time.deltaTime;
        if (comboTimer <= 0)
        {
            comboCounter = 0;
            comboTimer = 0;
        }
        Mathf.Max(comboTimer, 0f);

    }


}
