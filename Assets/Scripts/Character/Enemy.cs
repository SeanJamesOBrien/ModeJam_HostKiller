using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<int> OnEnemyDestroyed = delegate { };
    [SerializeField] int health;
    Transform player;
    int id;

    public Transform Player { get => player; set => player = value; }
    public int Id { get => id; set => id = value; }

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
            OnEnemyDestroyed?.Invoke(id);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            OnEnemyDestroyed?.Invoke(id);
            collision.gameObject.GetComponent<IDamageable>().CalculateDamage(K.EnemyDamage);
            Destroy(gameObject);
        }
    }
}