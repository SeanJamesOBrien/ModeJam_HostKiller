using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDestroyed = delegate { };
    public static event Action OnPlayerDestroyedComplete = delegate { };
    public static event Action<int> OnHealthChanged = delegate { };
    public static event Action<bool> OnGodModeChanged = delegate { };
    Animator animator;
    bool isGodMode = false;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    [SerializeField] Transform spritePosition;
    Vector3 faceRight = new Vector3(1.25f, 0, 0);
    Vector3 faceLeft = new Vector3(-1.25f, 0, 0);

    [Header("Combat")]
    //PlayerModes currentMode = PlayerModes.Cross;
    bool isMeleeMode = false;
    [SerializeField] float rangedAttackSpeed = 0.75f;
    [SerializeField] GameObject projectile;  
    float rangedAttackTimer = 0;
    [SerializeField] EventReference rangedAttackSound;

    [Header("Melee")]
    [SerializeField] float meleeAttackSpeed = 1.5f;
    [SerializeField] float meleeRange = 1.5f;
    [SerializeField] int meleeDamage = 1000;
    [SerializeField] LayerMask attackMask;
    [SerializeField] EventReference meleeAttackHitSound;
    [SerializeField] EventReference meleeAttackMissSound;
    float meleeAttackTimer = 0;

    [Header("Health")]
    int health = K.PlayerStartingHealth;
    [SerializeField] float healthRegen = 1f;
    [SerializeField] float invulnerabilityDuration;
    [SerializeField] int invulnerabilityFlickers;
    float invulnerabilityFlickerDuration;
    int flickerBuffer;
    float healthRegenTimer = 0;
    bool hasInvulnerability = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] Color normalColour;
    [SerializeField] Color invulnerabilityColour;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = normalColour;
        invulnerabilityFlickerDuration = invulnerabilityDuration / invulnerabilityFlickers;
    }

    private void Start()
    {
        EnemySpawner.OnEnemiesDefeated += EnemySpawner_OnEnemiesDefeated;
    }

    private void OnDestroy()
    {
        EnemySpawner.OnEnemiesDefeated -= EnemySpawner_OnEnemiesDefeated;
    }

    private void Update()
    {
        if (health > 0)
        { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleMode();
            }
            rangedAttackTimer += Time.deltaTime;
            meleeAttackTimer += Time.deltaTime;
            HandleMode();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGodMode = !isGodMode;
            
        }
    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            Movement();
            HealthRegen();
        }
    }

    private void HealthRegen()
    {
        if (health < K.PlayerStartingHealth &&
            health > 0)
        {
            healthRegenTimer += Time.fixedDeltaTime;
            if (healthRegenTimer > healthRegen)
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

        if (xInput > 0)
        {
            spritePosition.localPosition = faceRight;        
            spriteRenderer.flipX = false;
        }
        else if (xInput < 0)
        {
            spritePosition.localPosition = faceLeft;
            spriteRenderer.flipX = true;
        }

        if (tempVect.magnitude > 0)
        {         
            animator.SetBool("IsWalking", true);
        }
        else
        {      
            animator.SetBool("IsWalking", false);
        }
    }

    private void ToggleMode()
    {
        isMeleeMode = !isMeleeMode;
        rangedAttackTimer = 0;
    }

    private void HandleMode()
    {
        if (isMeleeMode)
        {
            if (meleeAttackTimer > meleeAttackSpeed)
            {
                Melee();
            }
        }
        else
        {
            if (rangedAttackTimer > rangedAttackSpeed)
            {
                CrossFire();
            }          
        }
    }

    private void Melee()
    {
        animator.SetTrigger("Attack");      
        meleeAttackTimer = 0;
    }

    private void CrossFire()
    {
        AudioController.Instance.PlayOneShot(rangedAttackSound, transform.position);
        for (int i = 45; i < 360; i+=90)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.eulerAngles = new Vector3(0, 0, i);
            if(isGodMode)
            {
                newProjectile.GetComponent<Projectile>().Damage = 1000;
            }
        }
        rangedAttackTimer = 0;
    }

    public void CalculateDamage(int damage)
    {
        if(hasInvulnerability ||
           isGodMode)
        {
            return;
        }

        health -= damage;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            OnPlayerDestroyed?.Invoke();
            //Time.timeScale = 0;
            animator.SetTrigger("Death");
            moveSpeed = 0;
        }
        if (health > 0)
        {
            flickerBuffer = 0;
            StartCoroutine(TemporaryInvulnerablity());
            StartCoroutine(InvulnerablityFlicker());
        }
    }

    private IEnumerator InvulnerablityFlicker()
    {
        flickerBuffer++;
        float time = 0;
        if(spriteRenderer.color == normalColour)
        {
            spriteRenderer.color = invulnerabilityColour;
        }
        else
        {
            spriteRenderer.color = normalColour;
        }
        while (time < invulnerabilityFlickerDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (flickerBuffer < invulnerabilityFlickers)
        {
            StartCoroutine(InvulnerablityFlicker());
        }
        else
        {
            spriteRenderer.color = normalColour;
        }
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

    private void EnemySpawner_OnEnemiesDefeated()
    {
        hasInvulnerability = true;
    }

    void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, meleeRange, attackMask);
        if (colliders.Length > 0)
        {
            AudioController.Instance.PlayOneShot(meleeAttackHitSound, transform.position);
            IDamageable damageable = colliders[UnityEngine.Random.Range(0, colliders.Length - 1)].GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.CalculateDamage(meleeDamage);
            }
        }
        else
        {
            AudioController.Instance.PlayOneShot(meleeAttackMissSound, transform.position);
        }
    }

    void OnDeath()
    {
        OnPlayerDestroyedComplete?.Invoke();
        Destroy(gameObject);
    }
}