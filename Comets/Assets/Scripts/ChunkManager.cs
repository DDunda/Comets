using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
	public Rigidbody2D shipRigidBody;

	public int chunkSize;
	public List<(int x, int y)> chunks = new List<(int x, int y)>();

	public float clearRadius;
	public int generateSize;

	public AnimationCurve cometDensity;

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

	public ObjectProbabilities cometConfig = new ObjectProbabilities();


	void SpawnSomething(Vector2 pos) {
		ObjectConfig config = cometConfig.SelectRandom(pos.magnitude);
		GameObject obj = Instantiate(config.prefab, pos, config.prefab.transform.rotation);
		obj.transform.localScale *= config.scale;
		obj.GetComponent<Rigidbody2D>().velocity = Utility.RandomDirection() * config.velocity;
	}


	void PopulateChunk(Vector2 pos) {
		float area = chunkSize * chunkSize;
		Vector2 chunkCenter = pos + Vector2.one * chunkSize / 2f;
		int comets = Mathf.FloorToInt(area * cometDensity.Evaluate(chunkCenter.magnitude));

		for (int i = 0; i < comets; i++) {
			Vector2 cometPos = pos + Utility.RandomWithinBox(chunkSize, chunkSize);
			SpawnSomething(cometPos);
		}
	}

	void Update()
	{
		for(int i = 0; i < chunks.Count; i++) {
			Vector2 pos = new Vector2(chunks[i].x, chunks[i].y);

			float shortestDistance = float.PositiveInfinity;
			foreach(Vector2 corner in Utility.corners) {
				float dist = Vector2.Distance(shipRigidBody.position, (pos + corner) * chunkSize);
				if (dist < shortestDistance)
					shortestDistance = dist;
			}

			if(shortestDistance > clearRadius) {
				chunks.RemoveAt(i--);
			}
		}

		(int x, int y) shipChunk = (Mathf.RoundToInt(shipRigidBody.position.x / chunkSize), Mathf.RoundToInt(shipRigidBody.position.y / chunkSize));

		for(int y = -generateSize; y <= generateSize; y++) {
			for(int x = -generateSize; x <= generateSize; x++) {
				(int x, int y) chunkCoord = (shipChunk.x + x, shipChunk.y + y);
				if (chunks.Contains(chunkCoord))
					continue;

				PopulateChunk(new Vector2(chunkCoord.x, chunkCoord.y) * chunkSize);
				chunks.Add(chunkCoord);
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;

		Utility.DrawCircleGizmo(shipRigidBody.position, clearRadius);

		(int x, int y) shipChunk = (Mathf.RoundToInt(shipRigidBody.position.x / chunkSize), Mathf.RoundToInt(shipRigidBody.position.y / chunkSize));

		Utility.DrawSquareGizmo(
			new Vector2(shipChunk.x - generateSize, shipChunk.y - generateSize) * chunkSize,
			Vector2.one * chunkSize * (generateSize * 2 + 1)
		);
		
		Gizmos.color = Color.red;
		foreach(var chunkCoord in chunks) {
			Utility.DrawSquareGizmo(
				new Vector2(chunkCoord.x, chunkCoord.y) * chunkSize + Vector2.one * 0.5f,
				Vector2.one * (chunkSize - 1)
			);
		}
	}
}