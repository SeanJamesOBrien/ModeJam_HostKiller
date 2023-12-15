using FMODUnity;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<int> OnEnemyDestroyed = delegate { };
    [SerializeField] int health;
    [SerializeField] EventReference spawnSound;
    [SerializeField] EventReference deathSound;
    [SerializeField] EventReference collisionSound;
    Transform player;
    int id;
    Animator animator;

    public Transform Player { get => player; set => player = value; }
    public int Id { get => id; set => id = value; }

    void Start()
    {
        if(health <= 0)
        {
            health = 2;
        }
        animator = GetComponent<Animator>();
        AudioController.Instance.PlayOneShot(spawnSound, transform.position);
    }

    public void CalculateDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {            
            DestroyEnemy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            AudioController.Instance.PlayOneShot(collisionSound, transform.position);
            collision.gameObject.GetComponent<IDamageable>().CalculateDamage(K.EnemyDamage);
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    { 
        OnEnemyDestroyed?.Invoke(id);

        if(animator.runtimeAnimatorController)
        {
            AudioController.Instance.PlayOneShot(deathSound, transform.position);
            GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Death");
            GetComponent<EnemyMovement>().enabled = false;
            EnemyAttack attack = GetComponentInChildren<EnemyAttack>();
            if(attack)
            {
                attack.enabled = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}