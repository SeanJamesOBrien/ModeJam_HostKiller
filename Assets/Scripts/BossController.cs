using System;
using UnityEngine;

public class BossController : Enemy
{
    public static event Action<int, int> OnBossDamaged = delegate { };
    int maxHealth;

    private void Awake()
    {
        maxHealth = health;
    }

    public override void CalculateDamage(int damage)
    {
        base.CalculateDamage(damage);
        OnBossDamaged?.Invoke(health, maxHealth);
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        SceneController.Instance.LoadNextScene(K.MainMenuScene);
    }
}