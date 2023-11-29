using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    [Header("Combat")]
    //PlayerModes currentMode = PlayerModes.Cross;
    bool isMeleeMode = false;
    [SerializeField] float attackSpeed = 0.75f;
    [SerializeField] GameObject projectile;
    float time = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMode();
        }
        time += Time.deltaTime;
        if (time > attackSpeed)
        {
            HandleMode();
            time = 0;
        }
    }

    void FixedUpdate()
    {
        Movement();
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
    }

    private void HandleMode()
    {
        if (isMeleeMode)
        {
            Melee();
        }
        else
        {
            CrossFire();
        }
    }

    private void Melee()
    {
        Debug.Log("melee attack");
    }

    private void CrossFire()
    {
        for (int i = 45; i < 360; i+=90)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.eulerAngles = new Vector3(0, 0, i);
        }   
    }
}
