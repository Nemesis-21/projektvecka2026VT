using UnityEngine;
[RequireComponent(typeof(Animator))]
public class UIFindPlayer : MonoBehaviour
{
    public PlayerMovement player;
    public Animator animator;
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        OtherAwake();
    }

    public virtual void OtherAwake() { }

}
