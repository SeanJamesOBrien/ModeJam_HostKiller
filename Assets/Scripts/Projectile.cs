using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colliison " + collision.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger " + collision.tag);
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}