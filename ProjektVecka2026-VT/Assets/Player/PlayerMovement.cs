//Edgar Åberg, 2026-03-02

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerMovement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{ 
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    public Vector2 movedirection;

    private Rigidbody rb;
    private Animator animator;
    private PlayerInput pInput;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(new Vector3(movedirection.x* moveSpeed, 0, movedirection.y*moveSpeed));
        rb.linearVelocity = new Vector3(movedirection.x * moveSpeed, rb.linearVelocity.y, movedirection.y * moveSpeed);

        
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        movedirection = context.ReadValue<Vector2>();
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 6, rb.linearVelocity.z);
    }
}
