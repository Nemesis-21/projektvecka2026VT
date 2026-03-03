using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenScript : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private PlayerInput pInput;
    [SerializeField] Animator animator;

    private void Start()
    {
        
    }
    public void OnNavigate(InputAction.CallbackContext context)
    {

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {

        
    }

    public void OnCancel(InputAction.CallbackContext context)
    {

    }

    public void OnPoint(InputAction.CallbackContext context)
    {

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Transition");
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {

    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {

    }
}
