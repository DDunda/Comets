using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeightedArray<T> {
	[System.Serializable]
	public class WeightedItem {
		public T item;
		public float weight;
	}

	public delegate bool Filter(T item);

	public WeightedItem[] items;

	public T SelectRandom() {
		float weightSum = 0;
		foreach(var item in items) {
			weightSum += item.weight;
		}

		float weight = Random.Range(0, weightSum);

		foreach (var pair in items)
		{
			if(weight < pair.weight) {
				return pair.item;
			}

			weight -= pair.weight;
		}

		return default(T);
	}

	public T SelectRandom(Filter filter) {
		float weightSum = 0;
		foreach(var item in items) {
			if(filter(item.item)) {
				weightSum += item.weight;
			}
		}

		float weight = Random.Range(0, weightSum);

		foreach (var item in items)
		{
			if(!filter(item.item)) continue;

			if(weight < item.weight) {
				return item.item;
			}

			weight -= item.weight;
		}

		return default(T);
	}
}


[System.Serializable]
[CreateAssetMenu(menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
	[System.Serializable]
	public class Drop {
		public GameObject prefab;
		public int count;
	}

	[System.Serializable]
	public class RewardPair {
		public Drop drop;
		public float reward;
	}

	public Drop[] fixedDrops;
	public WeightedArray<RewardPair> randomDrops = new WeightedArray<RewardPair>();


	public GameObject[] CreateDrops(float reward) {
		List<GameObject> objects = new List<GameObject>();

		RewardPair randDrop;

		float rewardGiven = 0;

		while((randDrop = randomDrops.SelectRandom(x => x.reward <= reward)) != null) {
			reward -= randDrop.reward;
			rewardGiven += randDrop.reward;
			for(int i = 0; i < randDrop.drop.count; i++) {
				objects.Add(randDrop.drop.prefab);
			}
		}

		foreach (Drop drop in fixedDrops)
		{
			for(int i = 0; i < drop.count; i++) {
				objects.Add(drop.prefab);
			}
		}

		return objects.ToArray();
	}
}