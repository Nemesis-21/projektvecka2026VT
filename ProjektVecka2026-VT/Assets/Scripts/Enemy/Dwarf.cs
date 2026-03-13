using UnityEngine;
using UnityEngine.AI;


public class Dwarf : BaseEnemyClass
{
    bool taunted;



    public override void Start()
    {
        base.Start();
        

    }

    public override void Update()
    {
        base.Update();


    }

    public override void Activate()
    {
        base.Activate();

        if (taunted) return;

        tauntSound.Play();

        taunted = true; 
    }
}