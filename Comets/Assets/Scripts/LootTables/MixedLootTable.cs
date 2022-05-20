using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Loot Table/Mixed loot")]
public class MixedLootTable : LootTable
{
	public Drop[] fixedDrops;
	public WeightedArray<RewardPair> randomDrops = new WeightedArray<RewardPair>();

	public override Drop[] CreateDrops(float reward)
	{
		List<Drop> objects = new List<Drop>(fixedDrops);

		while (reward > 0)
		{
			RewardPair drop = randomDrops.SelectRandom(x => x.reward <= reward);
			if (drop == null) break;

			reward -= drop.reward;
			objects.Add(drop.drop);
		}

		return objects.ToArray();
	}
}