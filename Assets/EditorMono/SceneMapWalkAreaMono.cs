using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

[ExecuteInEditMode]
public class SceneMapWalkAreaMono : MonoBehaviour {
	public float widthPerGrid = 10f;
	[System.NonSerialized]
	public SceneMapData mapMO;

	void Awake()
	{
		ResetMapMO (false);
	}

	public void ResetMapMO(bool saveMapData)
	{
		Vector3 pos = this.gameObject.transform.position;
		Vector2 origin = new Vector2(pos.x,pos.y);
		float tempWidthPerGrid = this.widthPerGrid > 0 ? widthPerGrid : 1f;

		Vector3 localScale = this.gameObject.transform.localScale;
		float width = localScale.x > 0 ? localScale.x : 1;
		float height = localScale.y > 0 ? localScale.y : 1;
		int x = Mathf.CeilToInt (width / tempWidthPerGrid);
		int y = Mathf.CeilToInt (height / tempWidthPerGrid);
		byte[,] mapData = new byte[x,y];
		for (int i = 0; i < mapData.GetLength (0); i++)
		{
			for (int j = 0; j < mapData.GetLength (1); j++)
			{
				mapData [i, j] = 1;
				if (saveMapData && mapMO != null)
				{
					if (i < mapMO.Map.GetLength (0) && j < mapMO.Map.GetLength (1))
					{
						mapData [i, j] = mapMO.Map [i, j];
					}
				}
			}
		}
		mapMO = new SceneMapData (origin.x,origin.y, tempWidthPerGrid, mapData);
	}
}


