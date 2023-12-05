using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        PlayerController.OnHealthChanged += PlayerController_OnHealthChanged;
    }

    private void OnDestroy()
    {
        PlayerController.OnHealthChanged -= PlayerController_OnHealthChanged;
    }

    private void PlayerController_OnHealthChanged(int health)
    {
        image.fillAmount = (float)health / K.PlayerStartingHealth;
    }
}