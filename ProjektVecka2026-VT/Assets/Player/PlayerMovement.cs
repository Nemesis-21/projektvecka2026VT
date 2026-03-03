//Edgar Åberg, 2026-03-02

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{ 
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityScale;
    public Vector2 movedirection;

    private Rigidbody rb;
    [SerializeField] Animator animator;
    private PlayerInput pInput;

    public Camera mainCam;
    public Vector2 mousePos;
    public LayerMask ground;
    public int comboCounter = 0;
    [SerializeField] GameObject streakCheck;
    public Animator animatorStreak;
    public TextMeshProUGUI textStreak;

    private bool walking;
    private bool onGround;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pInput = GetComponent<PlayerInput>();

        animatorStreak = streakCheck.GetComponent<Animator>();
        textStreak = streakCheck.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        lookAround();
        rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.y * moveSpeed);
        textStreak.text = "X" + comboCounter;




        animator.SetBool("Walk", walking);
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
            comboCounter++;
            animatorStreak.SetTrigger("Combo");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mousePos = context.ReadValue<Vector2>();
        }
        
    }
}
