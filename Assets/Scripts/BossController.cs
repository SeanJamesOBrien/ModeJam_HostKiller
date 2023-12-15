using FMOD;
using FMODUnity;
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
    bool isDead = false;
    bool isShootingMode = true;
    Transform player;
    Animator animator;
    [SerializeField] EventReference winSound;
    [SerializeField] EventReference deathSound;

    [Header("Shooting")]
    [SerializeField] float shootingDurationMin = 5;
    [SerializeField] float shootingDurationMax = 10;
    [SerializeField] GameObject projectileAttack;
    [SerializeField] GameObject circleAttack;
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
    [SerializeField] EventReference chargeSound;


    private void Awake()
    {
        if(player == null)
        {
            player = FindAnyObjectByType<PlayerController>().transform;
        }
        animator = GetComponent<Animator>();
        //projectileAttack = GetComponentInChildren<ProjectileAttack>();
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
                AudioController.Instance.PlayOneShot(chargeSound, transform.position);
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
        ToggleShooting(mode);
        enemyMovement.enabled = mode;
        currentCharges = 0;
        time = 0;
    }

    private void ToggleShooting(bool mode)
    {
        if(isSecondHealthBar)
        {
            projectileAttack.SetActive(mode);
        }
        else
        {       
            circleAttack.SetActive(mode);
        }
    }

    public void CalculateDamage(int damage)
    {
        if(isDead)
        {
            return;
        }
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            if (!isSecondHealthBar)
            {
                circleAttack.SetActive(false);
                isSecondHealthBar = true;
                maxHealth = secondHealth;
                health = secondHealth;
                animator.SetTrigger("Transform");
                currentCharges = 0;
                isCharging = false;
                isShootingMode = true;
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
        if (health <= 0)
        {
            SceneController.Instance.LoadNextScene(K.EndCardScene);
        }
    }

    private void DestroyEnemy()
    {        
        AudioController.Instance.PlayOneShot(deathSound, transform.position);
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
        AudioController.Instance.PlayOneShot(winSound, transform.position);
        Destroy(gameObject);
    }

    void OnTransform()
    {
        projectileAttack.SetActive(true);
        isDead = false;
    }
}