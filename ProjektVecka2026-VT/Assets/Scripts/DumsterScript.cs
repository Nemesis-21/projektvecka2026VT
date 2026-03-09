using UnityEngine;

public class DumsterScript : MonoBehaviour, IDamageable
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void TakeDamage(float damageAmount)
    {
        rb.linearVelocity = new Vector3(0, 2, 0);
    }

}
