using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackSpeed;
    float time = 0;

    protected virtual void Update()
    {
        time += Time.deltaTime;
        if (time > attackSpeed)
        {
            Attack();
            time = 0;
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("base attack");
    }
}
