using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDestroyed = delegate { };
    public static event Action<int> OnHealthChanged = delegate { };
    
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;

    [Header("Combat")]
    //PlayerModes currentMode = PlayerModes.Cross;
    bool isMeleeMode = false;
    [SerializeField] float rangedAttackSpeed = 0.75f;
    [SerializeField] GameObject projectile;  
    float attackTimer = 0;

    [Header("Melee")]
    [SerializeField] float meleeAttackSpeed = 1.5f;
    [SerializeField] float meleeRange = 1.5f;
    [SerializeField] int meleeDamage = 1000;
    [SerializeField] LayerMask attackMask;
    
    [Header("Health")]
    int health = K.PlayerStartingHealth;
    [SerializeField] float healthRegen = 1f;
    [SerializeField] float invulnerabilityDuration;
    float healthRegenTimer = 0;
    bool hasInvulnerability = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        EnemySpawner.OnLevelOver += EnemySpawner_OnLevelOver;
    }

    private void OnDestroy()
    {
        EnemySpawner.OnLevelOver -= EnemySpawner_OnLevelOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMode();
        }
        attackTimer += Time.deltaTime;
        HandleMode();          
    }

    void FixedUpdate()
    {
        Movement();
        HealthRegen();
    }

    private void HealthRegen()
    {      
        if (health < K.PlayerStartingHealth)
        {
            healthRegenTimer += Time.fixedDeltaTime;
            if(healthRegenTimer > healthRegen) 
            {
                health++;
                healthRegenTimer = 0;
                OnHealthChanged?.Invoke(health);
            }        
        }      
    }

    private void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(xInput, zInput, 0);
        tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
    }

    private void ToggleMode()
    {
        isMeleeMode = !isMeleeMode;
        attackTimer = 0;
    }

    private void HandleMode()
    {
        if (isMeleeMode)
        {
            if (attackTimer > meleeAttackSpeed)
            {
                Melee();
            }
        }
        else
        {
            if (attackTimer > rangedAttackSpeed)
            {
                CrossFire();
            }          
        }
    }

    private void Melee()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, meleeRange, attackMask);
        if (colliders.Length > 0)
        {
            IDamageable damageable = colliders[UnityEngine.Random.Range(0, colliders.Length - 1)].GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.CalculateDamage(meleeDamage);
            }          
        }
        attackTimer = 0;
    }

    private void CrossFire()
    {
        for (int i = 45; i < 360; i+=90)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.eulerAngles = new Vector3(0, 0, i);
        }
        attackTimer = 0;
    }

    public void CalculateDamage(int damage)
    {
        if(hasInvulnerability)
        {
            return;
        }

        health -= damage;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            //Time.timeScale = 0;
            OnPlayerDestroyed?.Invoke();
            Destroy(gameObject);
        }
        StartCoroutine(TemporaryInvulnerablity());
    }

    public IEnumerator TemporaryInvulnerablity()
    {
        hasInvulnerability = true;
        float time = 0;
        while (time < invulnerabilityDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        hasInvulnerability = false;
    }

    private void EnemySpawner_OnLevelOver()
    {
        this.enabled = false;
    }
}