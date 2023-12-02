using System;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            collision.gameObject.GetComponent<IDamageable>().CalculateDamage(K.EnemyDamage);
            Destroy(gameObject);
        }
    }
}