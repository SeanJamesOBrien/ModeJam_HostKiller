using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Transform player;
    bool isBoosting = false;

    void Start()
    {
        player = GetComponent<Enemy>().Player;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.fixedDeltaTime);
    }

    internal void SpeedBoost()
    {
        if (!isBoosting)
        { 
            isBoosting = true;
            moveSpeed *= K.ChargeMultiplier;
        }
    }
}