using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Loot Table/Fixed loot")]
public class FixedLootTable : LootTable
{
	public Drop[] fixedDrops;
	public override Drop[] CreateDrops(float reward) => fixedDrops;
}