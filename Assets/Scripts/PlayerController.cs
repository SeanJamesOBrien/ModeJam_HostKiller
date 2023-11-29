using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    [Header("Combat")]
    PlayerModes currentMode = PlayerModes.Cross;
    [SerializeField] float attackSpeed = 0.75f;
    [SerializeField] GameObject projectile;
    float time = 0;
   // Quaternion angle;

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
        throw new NotImplementedException();
    }

    private void HandleMode()
    {
            switch (currentMode)
        {
            case PlayerModes.Melee:
                break;
            case PlayerModes.Closest:
                break;
            case PlayerModes.Cross:
                CrossFire();
                break;
        }

    }

    private void CrossFire()
    {

        //for (int i = 0; i < numProjectiles; i++)
        //{
            
        //   // IProjectile bullet = BulletPoolController.SharedInstance.GetPooledBossBullet();
        //    //bullet.Transform.gameObject.SetActive(true);
        //    bullet.Transform.position = transform.position;
        //    bullet.Transform.rotation = i;
        //    bullet.ResetBullet();
        //    bullet.Damage = damage;
        //    bullet.Speed = projectileSpeed;
        //    angle.eulerAngles = new Vector3(0, 0, rotateAngle + i * (360 / numProjectiles));
        //}
        //rotateAngle += angleIncrement;

        for (int i = 45; i < 360; i+=90)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.eulerAngles = new Vector3(0, 0, i);
        }   
    }
}
