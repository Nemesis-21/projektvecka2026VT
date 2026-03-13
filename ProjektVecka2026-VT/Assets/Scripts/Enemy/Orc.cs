using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orc : BaseEnemyClass
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Follower followerScript;
    [SerializeField] float timer;
    [SerializeField] float threshold;
    [SerializeField] GameObject bullet;
    

    public override void Start()
    {
        base.Start();

        followerScript = gameObject.GetComponent<Follower>();
    }

    public override void Update()
    {
        base.Update();

        if (!activate) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep the agent upright

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }
    }

    public override void Activate()
    {
        base.Activate();
        
    }

    public override IEnumerator Die()
    {
        StartCoroutine(Knockback(65, 5));

        Time.timeScale = 0.2f;

        yield return new WaitForSecondsRealtime(4);

        Time.timeScale = 1;
        
        SceneManager.LoadScene(2);

        yield return null;
    }
}
