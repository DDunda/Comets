using UnityEngine;

public class Utility {
	public static Vector2 FromAngle(float angle) {
		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

	public static Vector2 RandomDirection() {
		return FromAngle(Random.Range(0, Mathf.PI * 2));
	}

	public static Vector2 RotateDirection(Vector2 v, float a) {
		Vector2 r = FromAngle(a);
		return new Vector2(v.x * r.x - v.y * r.y, v.x * r.y + v.y * r.x);
	}

	public static Vector2 RandomWithinCircle(float minRadius, float maxRadius) {
		float mR = 1 - Mathf.Sqrt(1 - minRadius / maxRadius);

		float randomDist = Random.Range(mR, 1);
		randomDist = (2 - randomDist) * randomDist * maxRadius;

		return RandomDirection() * randomDist;
	}

	public static Vector2 RandomWithinCircle(Range range) {
		return RandomWithinCircle(range.min, range.max);
	}


	public static Vector2 RandomWithinBox(float width, float height) {
		return new Vector2(Random.Range(0, width), Random.Range(0, height));
	}


	[System.Serializable]
	public class Range {
		public float min, max;
		public Range(float min, float max) {
			this.min = min;
			this.max = max;
		}
		public static implicit operator float(Range r) {
			return Random.Range(r.min, r.max);
		}
		public static implicit operator Range((float min, float max) tup) {
			return new Range(tup.min, tup.max);
		}
	}

	public static Vector2[] corners = { Vector2.zero, Vector2.up, Vector2.one, Vector2.right };

	public static void DrawSquareGizmo(Vector2 pos, Vector2 size) {

		Vector2 lcorner, corner = Vector2.Scale(corners[3], size);

		for (int i = 0; i < 4; i++) {
			lcorner = corner;
			corner = Vector2.Scale(corners[i], size);
			Gizmos.DrawLine(
				pos + lcorner,
				pos + corner
			);
		}
	}

	public static void DrawCircleGizmo(Vector2 pos, float r, int n = 32) {
		Vector2 lpoint, point = Vector2.right * r + pos;

		for (int i = 0; i < n; i++) {
			lpoint = point;
			point = FromAngle((i + 1) / (float)n * 2 * Mathf.PI) * r + pos;
			Gizmos.DrawLine(
				lpoint,
				point
			);
		}
	}
}