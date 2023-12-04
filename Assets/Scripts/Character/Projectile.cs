using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] int damage;
    float lifeTime = 0;
    bool isEnemy = false;

    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public float LifeTime { get => lifeTime; set => lifeTime = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    private void Start()
    {
        Debug.Log(lifeTime);
        if(lifeTime != 0)
        {
            Invoke(nameof(DestroyProjectile), lifeTime);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(K.DefaultLayer))
        {
            return;
        }
        if (CheckForFriendlyFire(collision))
        {
            return;
        }
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.CalculateDamage(damage);
        }
        Destroy(gameObject);
    }

    private bool CheckForFriendlyFire(Collider2D collision)
    {
        if (IsEnemy)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(K.EnemyLayer))
            {
                return true;
            }
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
            {
                return true;
            }
        }
        return false;
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}