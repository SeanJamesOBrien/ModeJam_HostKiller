using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SpawnSetting", fileName = "SpawnSetting.asset")]
public class SpawnChanceSO : ScriptableObject
{
    [SerializeField] public int[] spawnChances;

    public int ChooseSpawnedEnemy()
    {
        int randomIndex = Random.Range(0, spawnChances.Length);

        int itemPicked = Random.Range(0, GetTotalWeight());
        for (int i = 0; i < spawnChances.Length; i++)
        {
            if (itemPicked <= spawnChances[i])
            {
                randomIndex = i;
                break;
            }
            else
            {
                itemPicked -= spawnChances[i];
            }
        }

        return randomIndex;
    }


    public int GetTotalWeight()
    {
        int totalWeight = 0;
        foreach (int spawnChance in spawnChances)
        {
            totalWeight += spawnChance;
        }
        return totalWeight;
    }
}
