using UnityEngine;
using UnityEngine.AI;


public class Dwarf : BaseEnemyClass
{
    private Follower followerScript;
    private Vector3 startPos; // VIKTIG FÖR ANIMATOR



    public override void Start()
    {
        base.Start();
        followerScript = gameObject.GetComponent<Follower>();
        startPos = gameObject.transform.position;

    }

    public override void Update()
    {
        base.Update();


    }
}