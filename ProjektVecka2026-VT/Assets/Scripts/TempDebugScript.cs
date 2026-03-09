using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class TempDebugScript : MonoBehaviour, InputSystem_Actions.IDebugActions
{
    public GameObject enemy;
    
    public float power;
    public float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnKnockback(InputAction.CallbackContext context)
    {
        Debug.Log("Attemping to knockback");
        
        StartCoroutine(enemy.GetComponent<BaseEnemyClass>().Knockback(power, duration));
    }
}