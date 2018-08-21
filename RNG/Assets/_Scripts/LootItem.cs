using UnityEngine;

[System.Serializable]
public class LootItem {
    public string Name;
    public GameObject Item;
    [Range(1, 100)]
    public int dropRarity = 1;
}

[System.Serializable]
public class LootRarityTint
{
    public string Name;
    public Color Tint;
    [Range(1, 100)]
    public int dropRarity = 1;
}
