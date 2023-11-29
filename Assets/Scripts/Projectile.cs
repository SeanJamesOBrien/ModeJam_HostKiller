using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] int damage;

    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return; 
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().CalculateDamage(damage);
        }
        Destroy(gameObject);
    }
}