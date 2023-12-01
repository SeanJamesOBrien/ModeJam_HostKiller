using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Transform player;

    void Start()
    {
        player = GetComponent<Enemy>().Player;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.fixedDeltaTime);
    }
}