using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<Enemy> OnEnemyDestroyed = delegate { };
    [SerializeField] int health;
    Transform player;

    public Transform Player { get => player; set => player = value; }

    void Start()
    {
        if(health <= 0)
        {
            health = 2;
        }
    }

    public void CalculateDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnEnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
