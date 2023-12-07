//using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Transform player;
    [SerializeField] bool hasRandomMovement = false;
    [SerializeField] bool isBoosting = false;
    bool isAware = false;
    bool isFollowing = true;

    float time = 0;
    float changeDirectionTimer = 0;
    float XBounds = 15.5f;
    float YBounds = 8.5f;
    float randomX;
    float randomY;

    public bool IsAware { get => isAware; set { isAware = value; UpdateAwareness(); } }

    void Start()
    {
        player = GetComponent<Enemy>().Player;
        if(!player)
        {
            player = FindAnyObjectByType<PlayerController>().transform;
        }
        isFollowing = !hasRandomMovement;
    }

    void FixedUpdate()
    {
        if(!player)
        {
            return;
        }
        if (isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            RandomMovement();
        }
    }


    private void RandomMovement()
    {
        time += Time.deltaTime;
        if (time >= changeDirectionTimer)
        {
            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f); 
            changeDirectionTimer = Random.Range(0.5f, 1.5f);
            time = 0;
        }
        transform.Translate(new Vector3(randomX, randomY, 0) * moveSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -XBounds, XBounds), Mathf.Clamp(transform.position.y, -YBounds, YBounds) ,0);
    }

    void SpeedBoost()
    {
        if (isBoosting)
        { 
            isBoosting = false;
            moveSpeed *= K.ChargeMultiplier;
        }
    }

    private void FollowPlayer()
    {
        isFollowing = true;
    }

    private void UpdateAwareness()
    {
        SpeedBoost();
        FollowPlayer();
    }
}