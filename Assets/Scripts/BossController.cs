using System;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    public static event Action<int, int> OnBossDamaged = delegate { };
    [SerializeField] int firstHealth;
    [SerializeField] int secondHealth;
    bool isSecondHealthBar = false;
    int health;
    int maxHealth;
    bool isShootingMode = true;
    Transform player;
    Animator animator;

    [Header("Shooting")]
    [SerializeField] float shootingDurationMin = 5;
    [SerializeField] float shootingDurationMax = 10;
    ProjectileAttack projectileAttack;
    EnemyMovement enemyMovement;
    float shootingDuration;
    float time;

    [Header("Charging")]
    [SerializeField] int chargeNumberMin = 2;
    [SerializeField] int chargeNumberMax = 5;
    int numOfCharges;
    int currentCharges;
    [SerializeField] float chargeSpeed = 14;
    Vector3 chargeDirection = Vector3.zero;
    [SerializeField] float chargeDuration;
    [SerializeField] float restDuration;
    bool isCharging = false;


    private void Awake()
    {
        if(player == null)
        {
            player = FindAnyObjectByType<PlayerController>().transform;
        }
        animator = GetComponent<Animator>();
        projectileAttack = GetComponentInChildren<ProjectileAttack>();
        enemyMovement = GetComponentInChildren<EnemyMovement>();
        maxHealth = firstHealth;
        health = maxHealth;
        shootingDuration = UnityEngine.Random.Range(shootingDurationMin, shootingDurationMax);
    }

    private void Update()
    {
        if(!player)
        {
            return;
        }
        time += Time.deltaTime;
        if (isShootingMode)
        {
            HandleShooting();
        }
        else
        {
            HandleCharging();
        }

    }

    private void HandleShooting()
    {
        if (time >= shootingDuration)
        {
            ToggleMode(false);
            numOfCharges = UnityEngine.Random.Range(chargeNumberMin, chargeNumberMax);
            isCharging = false;
        }
    }

    private void HandleCharging()
    {
        if (!isCharging)
        {
            if (time >= restDuration)
            {
                isCharging = true;
                chargeDirection = player.position - transform.position;
                time = 0;
            }
            else
            {
                return;
            }

        }
        if (isCharging)
        {
            transform.position = Vector3.MoveTowards(transform.position, chargeDirection, chargeSpeed * Time.fixedDeltaTime);
        }
        if(time >= chargeDuration)
        {
            isCharging = false;
            currentCharges++;
            time = 0;
        }
        
        
        if (currentCharges >= numOfCharges)
        {
            ToggleMode(true);           
            shootingDuration = UnityEngine.Random.Range(shootingDurationMin, shootingDurationMax);
        }
    }

    void ToggleMode(bool mode)
    {
        isShootingMode = mode;
        projectileAttack.enabled = mode;
        enemyMovement.enabled = mode;
        currentCharges = 0;
        time = 0;
    }

    public void CalculateDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if(!isSecondHealthBar)
            {
                isSecondHealthBar = true;
                maxHealth = secondHealth;
                health = secondHealth;
                animator.SetTrigger("Transform");
            }
            else
            {
                DestroyEnemy();
            }
            
        }
        OnBossDamaged?.Invoke(health, maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            collision.gameObject.GetComponent<IDamageable>().CalculateDamage(K.EnemyDamage);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer(K.WallLayer))
        {
            isCharging = false;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        SceneController.Instance.LoadNextScene(K.MainMenuScene);
    }

    private void DestroyEnemy()
    {
        if (animator && animator.runtimeAnimatorController)
        {
            animator.SetTrigger("Death");
            EnemyAttack attack = GetComponentInChildren<EnemyAttack>();
            if (attack)
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