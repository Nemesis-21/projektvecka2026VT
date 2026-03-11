using UnityEngine;

public class DissolveParent : MonoBehaviour
{
    void Start()
    {
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
