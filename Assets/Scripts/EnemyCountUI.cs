using TMPro;
using UnityEngine;

public class EnemyCountUI : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int numEnemies)
    {
        text.text = "Enemies Remaining: " + numEnemies;
    }
}