using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] int damage;
    bool isEnemy = false;

    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }

    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CheckForFriendlyFire(collision))
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
            Debug.Log("is enemy?");
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return true;
            }
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        return false;
    }
}