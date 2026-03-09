using UnityEngine;

public class KillBox : MonoBehaviour
{
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            player.GetComponent<PlayerMovement>().currentHp = 0;
        }
    }

}
