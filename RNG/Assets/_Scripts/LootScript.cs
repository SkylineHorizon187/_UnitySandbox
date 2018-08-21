using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour {

    public List<LootItem> LootTable = new List<LootItem>();
    public List<LootRarityTint> ColorTable = new List<LootRarityTint>();

    private int dropWeight;
    private int rarityWeight;

	void Start () {
        dropWeight = 0;
        for (int i = 0; i < LootTable.Count; i++)
        {
            dropWeight += LootTable[i].dropRarity;
        }

        rarityWeight = 0;
        for (int i = 0; i < ColorTable.Count; i++)
        {
            rarityWeight += ColorTable[i].dropRarity;
        }
	}
	
    public LootItem GetLoot()
    {
        int dropChance = Random.Range(0, dropWeight);

        for (int i = 0; i < LootTable.Count; i++)
        {
            if (dropChance < LootTable[i].dropRarity)
            {
                return LootTable[i];
            }
            dropChance -= LootTable[i].dropRarity;
        }
        return null;
    }

    public LootItem GetLoot(int PercentChance)
    {
        if (Random.Range(0, 101) < PercentChance)
        {
            return GetLoot();
        }
        return null;
    }

    public Color GetRarity()
    {
        Color c = Color.white;
        int rarityChance = Random.Range(0, rarityWeight);

        for (int i = 0; i < ColorTable.Count; i++)
        {
            if (rarityChance < ColorTable[i].dropRarity)
            {
                c = ColorTable[i].Tint;
                break;
            }
            rarityChance -= ColorTable[i].dropRarity;
        }
        return c;
    }
}
