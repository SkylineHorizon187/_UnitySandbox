using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour {

    public List<LootItem> LootTable = new List<LootItem>();
    private int dropWeight;

	void Start () {
        dropWeight = 0;
        for (int i = 0; i < LootTable.Count; i++)
        {
            dropWeight += LootTable[i].dropRarity;
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
}
