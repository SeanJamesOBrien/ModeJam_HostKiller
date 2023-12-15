using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float attackSpeed;
    [SerializeField] float projectileSpeed;
    [SerializeField] int angleIncrement;
    Quaternion angle;
    float time = 0;
    float rotateAngle;

    void Start()
    {
        rotateAngle = angle.eulerAngles.y;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= attackSpeed)
        {
            time = 0;
            Projectile newProjectile = Instantiate(projectile);
            newProjectile.gameObject.transform.position = transform.position;
            newProjectile.gameObject.transform.rotation = angle;
            newProjectile.IsEnemy = true;
            newProjectile.MovementSpeed = projectileSpeed;
            angle.eulerAngles = new Vector3(0, 0, rotateAngle);
            rotateAngle += angleIncrement;
        }
    }
}
