using UnityEngine;

public class ChargeAwareness : MonoBehaviour
{
    EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            enemyMovement.SpeedBoost();
        }
    }
}