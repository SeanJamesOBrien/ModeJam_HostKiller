using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    Image healthBar;
    void Start()
    {
        healthBar = GetComponent<Image>();
        BossController.OnBossDamaged += BossController_OnBossDamaged;
    }

    private void OnDestroy()
    {
        BossController.OnBossDamaged -= BossController_OnBossDamaged;
    }

    private void BossController_OnBossDamaged(int health, int maxHealth)
    {
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }
}