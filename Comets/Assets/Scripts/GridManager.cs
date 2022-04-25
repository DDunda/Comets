using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public Camera shipCamera;
	public Transform shipTransform;
	public GameObject pointPrefab;
	public float pointSpacing = 1f;
	public float ZPosition = 1f;

	private List<GameObject> points = new List<GameObject>();


	void Start()
	{
		Vector2 halfsize = new Vector2(1 * shipCamera.aspect, 1) * shipCamera.orthographicSize;

		int w = Mathf.CeilToInt(halfsize.x * 2 / pointSpacing) + 1;
		int h = Mathf.CeilToInt(halfsize.y * 2 / pointSpacing) + 1;
		for (int y = -1; y <= h; y++)
		{
			for (int x = -1; x <= w; x++)
			{
				points.Add(
					Instantiate(
						pointPrefab,
						transform.position + (Vector3)(new Vector2(x,y) * pointSpacing - halfsize),
						Quaternion.identity,
						transform
					)
				);
			}
		}
	}


	void Update()
	{
		Vector3 tmp = shipCamera.transform.position;
		tmp.x = Mathf.RoundToInt(tmp.x / pointSpacing) * pointSpacing;
		tmp.y = Mathf.RoundToInt(tmp.y / pointSpacing) * pointSpacing;
		tmp.z = ZPosition;
		transform.position = tmp;
	}
}