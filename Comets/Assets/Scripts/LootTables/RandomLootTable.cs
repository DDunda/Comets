using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Loot Table/Random loot")]
public class RandomLootTable : LootTable
{
	public WeightedArray<RewardPair> randomDrops = new WeightedArray<RewardPair>();

	public override Drop[] CreateDrops(float reward)
	{
		List<Drop> objects = new List<Drop>();

		while (reward > 0)
		{
			RewardPair drop = randomDrops.SelectRandom(x => x.reward <= reward);
			if (drop == null) break;

			reward -= drop.reward * drop.count;
			objects.Add(drop.drop);
		}

		return objects.ToArray();
	}
}