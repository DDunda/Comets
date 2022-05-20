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

	public WeightedItem Get(float weight) {
		foreach (var item in items) {
			weight -= item.weight;
			if (weight <= 0) return item;
		}

		return null;
	}
	public WeightedItem Get(float weight, Filter filter) {		
		foreach (var item in items) {
			if(!filter(item.item)) continue;

			weight -= item.weight;
			if (weight <= 0) return item;
		}

		return null;
	}

	public float TotalWeight() {
		float sum = 0;
		foreach(var item in items) {
			sum += item.weight;
		}
		return sum;
	}
	public float TotalWeight(Filter filter) {
		float sum = 0;
		foreach(var item in items) {
			if(!filter(item.item)) continue;
			sum += item.weight;
		}
		return sum;
	}

	public T this[float weight] {
		get {
			WeightedItem pair = Get(weight);
			return pair != null ? pair.item : default(T);
		}
	}
	public T this[float weight, Filter filter] {
		get {
			WeightedItem pair = Get(weight, filter);
			return pair != null ? pair.item : default(T);
		}
	}

	public T SelectRandom() {
		return this[Random.Range(0, TotalWeight())];
	}
	public T SelectRandom(Filter filter) {
		return this[Random.Range(0, TotalWeight(filter)), filter];
	}
}

[System.Serializable]
public abstract class LootTable : ScriptableObject
{
	[System.Serializable]
	public struct Drop
	{
		public GameObject prefab;
		public int count;

		public Drop(GameObject prefab, int count) {
			this.prefab = prefab;
			this.count = count;
		}
	}

	[System.Serializable]
	public class RewardPair
	{
		public GameObject prefab;
		public int count;
		public float reward;

		public Drop drop { get => new Drop(prefab, count); }
	}

	public abstract Drop[] CreateDrops(float reward);
}