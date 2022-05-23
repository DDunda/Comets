using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
	[System.Serializable]
	public class Chunk {
		public List<GameObject> stars = new List<GameObject>();
		public Vector2Int coords;
		public ChunkManager manager;
		public bool populated = false;

		private Vector2 worldCoords {get => coords * manager.chunkSize;}


		public Chunk(Vector2Int coords, ChunkManager manager) {
			this.coords = coords;
			this.manager = manager;
		}


		public Chunk(int x, int y, ChunkManager manager) {
			this.coords = new Vector2Int(x,y);
			this.manager = manager;
		}


		~Chunk() {
			DeleteStars();
		}


		public void DeleteStars() {
			foreach(var star in stars) {
				Destroy(star);
			}
			stars.Clear();
		}


		void SpawnStar(Vector2 pos) {
			GameObject prefab = manager.stars.SelectRandom();
			GameObject star = Instantiate(prefab, new Vector3(pos.x, pos.y, manager.starDepth), prefab.transform.rotation);

			star.transform.localScale *= manager.starScale;
			star.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, manager.starBrightness);

			stars.Add(star);
		}

		
		void SpawnSomething(Vector2 pos) {
			ObjectConfig config = manager.cometConfig.SelectRandom(pos.magnitude);
			GameObject obj = Instantiate(config.prefab, pos, config.prefab.transform.rotation);

			obj.transform.localScale *= config.scale;
			obj.GetComponent<Rigidbody2D>().velocity = Utility.RandomDirection() * config.velocity;
		}


		public void PopulateStars() {
			float numStarsf = manager.starsPerSector * manager.sectorMultiplier;
			int numStars = Mathf.FloorToInt(numStarsf) + (numStarsf % 1 > Random.value ? 1 : 0);

			for (int i = 0; i < numStars; i++) {
				Vector2 starPos = worldCoords + Utility.RandomWithinBox(manager.chunkSize, manager.chunkSize);
				SpawnStar(starPos);
			}
		}


		public void Populate() {
			Vector2 chunkCenter = worldCoords + Vector2.one / 2f * manager.chunkSize;

			float numCometsf = manager.cometsPerSector.Evaluate(
				Mathf.Clamp01(chunkCenter.magnitude / manager.maxSpawnRange),
				Random.value
			) * manager.sectorMultiplier;
			int numComets = Mathf.FloorToInt(numCometsf) + (numCometsf % 1 > Random.value ? 1 : 0);

			for (int i = 0; i < numComets; i++) {
				Vector2 cometPos = worldCoords + Utility.RandomWithinBox(manager.chunkSize, manager.chunkSize);
				SpawnSomething(cometPos);
			}

			populated = true;
		}
	}


	[System.Serializable]
	public class ObjectConfig {
		public GameObject prefab;
		public Utility.Range velocity;
		public Utility.Range scale;
	}


	[System.Serializable]
	public class ObjectProbabilities {
		[System.Serializable]
		public class ProbabilityPair {
			public ObjectConfig config;
			public AnimationCurve weight;
		}

		public ProbabilityPair[] objects;


		public ObjectConfig SelectRandom(float distance) {
			float weightSum = 0;
			foreach (var item in objects) {
				weightSum += item.weight.Evaluate(distance);
			}

			float weight = Random.Range(0, weightSum);

			foreach (var item in objects) {
				float _weight = item.weight.Evaluate(distance);
				if (weight < _weight) {
					return item.config;
				}

				weight -= _weight;
			}

			return null;
		}
	}


	public List<Chunk> chunks = new List<Chunk>();
	public int chunkSize;
	public float sectorArea;

	public int cometGenerationSize;
	public int starGenerationSize;
	public float clearRadius;

	public ParticleSystem.MinMaxCurve cometsPerSector;
	public float maxSpawnRange;
	public ObjectProbabilities cometConfig = new ObjectProbabilities();

	public Utility.Range starsPerSector;
	public WeightedArray<GameObject> stars = new WeightedArray<GameObject>();
	public Utility.Range starScale;
	public Utility.Range starDepth;
	public Utility.Range starBrightness;

	private int chunkArea {get => chunkSize * chunkSize;}
	private float sectorMultiplier {get => chunkArea / sectorArea;}
	private Vector2Int CenterChunk { get => Vector2Int.RoundToInt(transform.position / chunkSize); }


	void Update()
	{
		for(int i = 0; i < chunks.Count; i++) {
			Vector2Int pos = chunks[i].coords;

			float shortestDistance = float.PositiveInfinity;
			foreach(Vector2 corner in Utility.corners) {
				float dist = Vector2.Distance(transform.position, (pos + corner) * chunkSize);
				if (dist < shortestDistance)
					shortestDistance = dist;
			}

			if(shortestDistance > clearRadius) {
				chunks[i].DeleteStars();
				chunks.RemoveAt(i--);
			}
		}

		Vector2Int center = CenterChunk;

		for(int y = -starGenerationSize; y <= starGenerationSize; y++) {
			for(int x = -starGenerationSize; x <= starGenerationSize; x++) {
				Vector2Int coords = center + new Vector2Int(x, y);
				if (chunks.FindIndex(_chunk => _chunk.coords == coords) != -1)
					continue;

				Chunk chunk = new Chunk(coords, this);
				chunk.PopulateStars();
				chunks.Add(chunk);
			}
		}

		for(int y = -cometGenerationSize; y <= cometGenerationSize; y++) {
			for(int x = -cometGenerationSize; x <= cometGenerationSize; x++) {
				Vector2Int coords = center + new Vector2Int(x, y);
				int index;
				if ((index = chunks.FindIndex(_chunk => _chunk.coords == coords)) == -1) {
					Chunk chunk = new Chunk(coords, this);
					chunk.PopulateStars();
					chunks.Add(chunk);
				} else if (chunks[index].populated) continue;

				chunks[index].Populate();
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;

		Utility.DrawCircleGizmo(transform.position, clearRadius);
		Utility.DrawCircleGizmo(Vector2.zero, maxSpawnRange);

		Vector2Int center = CenterChunk;

		Utility.DrawSquareGizmo(
			(center - Vector2.one * cometGenerationSize) * chunkSize,
			Vector2.one * (cometGenerationSize * 2 + 1) * chunkSize
		);

		Gizmos.color = Color.white;

		Utility.DrawSquareGizmo(
			(center - Vector2.one * starGenerationSize) * chunkSize,
			Vector2.one * (starGenerationSize * 2 + 1) * chunkSize
		);
		
		foreach(var chunk in chunks) {
			Gizmos.color = chunk.populated ? Color.red : Color.yellow;
			Utility.DrawSquareGizmo(
				chunk.coords * chunkSize + Vector2.one * 0.25f,
				Vector2.one * (chunkSize - 0.5f)
			);
		}
	}
}