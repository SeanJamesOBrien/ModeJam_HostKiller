using UnityEngine;

public class ProjectileAttack : EnemyAttack
{
    [SerializeField] Projectile projectile;
    Transform target;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifeTime;
    [SerializeField] Transform[] projectileStarts;   
    Vector2 direction;
    float angle;

    private void Start()
    {
        //Enemy target = GetComponentInParent<Enemy>();
        if(target == null)
        {
            target = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
        }
        PlayerController.OnPlayerDestroyed += PlayerController_OnPlayerDestroyed;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDestroyed -= PlayerController_OnPlayerDestroyed;
    }

    private void PlayerController_OnPlayerDestroyed()
    {
        target = null;
    }


    protected override void Update()
    {
        base.Update();
        if (target)
        {
            LookAtTarget();
        }
    }

    void LookAtTarget()
    {
        direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void Attack()
    {
        if(!target)
        {
            return;
        }
        foreach (Transform projectileStart in projectileStarts)
        {
            Projectile newProjectile = Instantiate(projectile, projectileStart.position, projectileStart.rotation);
            newProjectile.IsEnemy = true;
            newProjectile.LifeTime = projectileLifeTime;
            if(projectileSpeed != 0)
            {
                newProjectile.MovementSpeed = projectileSpeed;
            }
        }
    }
}