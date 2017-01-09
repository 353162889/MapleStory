using System;
using UnityEditor;
using UnityEngine;
using Game;
using System.IO;
using Framework;

[CustomEditor(typeof(SceneMapWalkAreaMono))]
public class SceneMapWalkAreaEditor : Editor
{
	private static bool editor = false;
	private GameObject _go;

	void OnDisable()
	{
		if (_go != null)
		{
			GameObject.DestroyImmediate (_go);
			_go = null;
		}
	}

    Vector2 keyOrigin = Vector2.zero;
    Vector2 keyCurrent = Vector2.zero;
    bool keyDown = false;
    void OnSceneGUI( )
	{
		SceneMapWalkAreaMono walkArea = target as SceneMapWalkAreaMono;

		if (walkArea == null || walkArea.transform.localScale.x <= 0 || walkArea.transform.localScale.y <= 0)
		{
			return;
		}
		Handles.BeginGUI ();
		if (GUILayout.Button ("刷新地图数据", GUILayout.Width (80)))
		{
			walkArea.ResetMapMO (true);
		}
		if (GUILayout.Button ("清除地图数据", GUILayout.Width (80)))
		{
			walkArea.ResetMapMO (false);
		}
		Handles.EndGUI ();
		if (!editor)
		{
			if (_go != null)
			{
				GameObject.DestroyImmediate (_go);
				_go = null;
			}
//			GUILayout.BeginArea (new Rect (Screen.width - 200, 10, 100, 200));
			Handles.BeginGUI ();
//			if (GUILayout.Button ("刷新地图数据", GUILayout.Width (80)))
//			{
//				walkArea.ResetMapMO (true);
//			}
//			if (GUILayout.Button ("清除地图数据", GUILayout.Width (80)))
//			{
//				walkArea.ResetMapMO (false);
//			}
			if (GUILayout.Button ("载入地图数据", GUILayout.Width (80)))
			{
                //暂时用txt结尾，因为unity在Resources.load下只能识别TXT和xml，如果用byte类型，必须用www下载
				string dirPath = EditorUtility.OpenFilePanel ("载入地图数据", "", "txt");
				if (!string.IsNullOrEmpty (dirPath))
				{
					string content = File.ReadAllText (dirPath);
					SceneMapData mapMO = StringUtil.Deserialize<SceneMapData> (content);
					walkArea.mapMO = mapMO;
					walkArea.widthPerGrid = mapMO.WidthPerGrid;
				    float width = walkArea.mapMO.Map.GetLength(0)*mapMO.WidthPerGrid;
				    float height = walkArea.mapMO.Map.GetLength(1)*mapMO.WidthPerGrid;
                    walkArea.gameObject.transform.localScale = new Vector3(width,height,1f);
				}
			}
			if (GUILayout.Button ("保存地图数据", GUILayout.Width (80)))
			{
				if (walkArea.mapMO != null)
				{
					string dirPath = EditorUtility.SaveFilePanel ("保存地图数据", "", "SceneData", "txt");
					if (!string.IsNullOrEmpty (dirPath))
					{
						string content = StringUtil.Serialize<SceneMapData> (walkArea.mapMO);
						File.WriteAllText (dirPath, content);
                        AssetDatabase.Refresh();
					}
				}
				else
				{
					Debug.Log ("walkArea.mapMO is null");
				}

			}
			if (GUILayout.Button ("进入编辑模式", GUILayout.Width (80)))
			{
				editor = true;
			}
			Handles.EndGUI ();
//			GUILayout.EndArea ();
		}
		else
		{
			if (_go == null)
			{
				_go = GameObject.CreatePrimitive (PrimitiveType.Quad);
				_go.transform.position = Vector3.zero;
				_go.transform.localScale = new Vector3 (10000, 10000, 1);
				_go.GetComponent<MeshRenderer> ().enabled = false;
			}
//			GUILayout.BeginArea (new Rect (Screen.width - 200, 10, 100, 50));
			Handles.BeginGUI ();
			if (GUILayout.Button ("退出编辑模式", GUILayout.Width (80)))
			{
				editor = false;
			}
			Handles.EndGUI ();
//			GUILayout.EndArea ();
		}
		if (walkArea.mapMO != null && editor)
		{
			Vector2 origin = new Vector2 (walkArea.mapMO.OriginX, walkArea.mapMO.OriginY);
			float WidthPerGrid = walkArea.mapMO.WidthPerGrid;
			byte[,] mapData = walkArea.mapMO.Map;
			for (int i = 0; i < mapData.GetLength(0); i++)
			{
				for (int j = 0; j < mapData.GetLength(1); j++) {
					float x = origin.x + i * WidthPerGrid;
					float y = origin.y + j * WidthPerGrid;
					byte value = mapData[i,j];
					if (value < 0)
						continue;
					bool walkable = (value & SceneMapData.Walkable) != 0;
					bool climb = (value & SceneMapData.Climbable) != 0;
					if (!walkable || climb)
					{
						Color color = new Color (walkable ? 0 : 1, climb ? 1f : 0, 0);
						Handles.DrawSolidRectangleWithOutline (new Rect (x, y, WidthPerGrid, WidthPerGrid), color, Color.clear);
					}
				}
			}
//			Handles.DrawSolidRectangleWithOutline (new Rect (origin.x, origin.y, mapData.GetLength (0) * WidthPerGrid, mapData.GetLength (1) * WidthPerGrid), Color.clear, Color.white);

			Color old = Handles.color;
			Handles.color = Color.blue;
			for (int i = 0; i < mapData.GetLength (0) + 1; i++)
			{
				float x = origin.x + i * WidthPerGrid;
				float y = origin.y + mapData.GetLength (1) * WidthPerGrid;
				Handles.DrawLine (new Vector3(x,origin.y,0),new Vector3(x,y,0));
			}
			for (int i = 0; i < mapData.GetLength (1) + 1; i++)
			{
				float y = origin.y + i * WidthPerGrid;
				float x = origin.x + mapData.GetLength (0) * WidthPerGrid;
				Handles.DrawLine (new Vector3(origin.x,y,0),new Vector3(x,y,0));
			}
			Handles.color = old;

			//事件监听
			Event e = Event.current;
			if (e.isKey)
			{
                if (e.type == EventType.keyDown && !keyDown)
                {
                    keyOrigin = e.mousePosition;
                    keyDown = true; 
                }
                else if (e.type == EventType.KeyDown)
                {
                    keyCurrent = e.mousePosition;
                }
                else if (keyDown && e.type == EventType.keyUp)
                {
                    keyDown = false;

                    Ray ray = HandleUtility.GUIPointToWorldRay(keyOrigin);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Vector2 hitPoint1 = new Vector2(hitInfo.point.x, hitInfo.point.y);
                        ray = HandleUtility.GUIPointToWorldRay(keyCurrent);
                        RaycastHit hitInfo1;
                        if (Physics.Raycast(ray, out hitInfo1))
                        {
                            Vector2 hitPoint2 = new Vector2(hitInfo1.point.x, hitInfo1.point.y);

                            float minX = Mathf.Min(hitPoint1.x, hitPoint2.x);
                            float maxX = Mathf.Max(hitPoint1.x, hitPoint2.x);
                            float minY = Mathf.Min(hitPoint1.y, hitPoint2.y);
                            float maxY = Mathf.Max(hitPoint1.y, hitPoint2.y);

                            float mapMaxX = origin.x + mapData.GetLength(0) * WidthPerGrid;
                            float mapMaxY = origin.y + mapData.GetLength(1) * WidthPerGrid;
                            for (float i = minX; i <= maxX; i+=WidthPerGrid)
                            {
                                for (float j = minY; j <= maxY; j+=WidthPerGrid)
                                {
                                    if (i > origin.x && i < mapMaxX && j > origin.y && j < mapMaxY)
                                    {
                                        int indexX = Mathf.FloorToInt((i - origin.x) / WidthPerGrid);
                                        int indexY = Mathf.FloorToInt((j - origin.y) / WidthPerGrid);
                                        if (e.keyCode == KeyCode.N)
                                        {
                                            byte curValue = mapData[indexX, indexY];
                                            bool walkable = (curValue & SceneMapData.Walkable) != 0;
                                            if (walkable)
                                            {
                                                mapData[indexX, indexY] = (byte)(curValue & (~SceneMapData.Walkable));
                                            }
                                        }else if(e.keyCode == KeyCode.M)
                                        {
                                            byte curValue = mapData[indexX, indexY];
                                            bool climbable = (curValue & SceneMapData.Climbable) != 0;
                                            if (!climbable)
                                            {
                                                mapData[indexX, indexY] = (byte)(curValue | SceneMapData.Climbable);
                                            }
                                        }
                                        else if(e.keyCode == KeyCode.Space)
                                        {
                                            mapData[indexX, indexY] = (byte)(mapData[indexX, indexY] & (~SceneMapData.Climbable));
                                            mapData[indexX, indexY] = (byte)(mapData[indexX, indexY] | SceneMapData.Walkable);
                                           
                                        }
                                    }
                                }
                            }
                            Repaint();
                        }
                    }

                  
                }
                //if (keyDown)
                //{
                //    float minx = Mathf.Min(keyOrigin.x, keyCurrent.x);
                //    float miny = Mathf.Min(keyOrigin.y, keyCurrent.y);
                //    float width = Mathf.Abs(keyOrigin.x - keyCurrent.x);
                //    float height = Mathf.Abs(keyOrigin.y - keyCurrent.y);
                //    Debug.Log("minx=" + minx + ",miny=" + miny + ",width=" + width + ",height=" + height);
                //    Handles.DrawSolidRectangleWithOutline(new Rect(minx, miny, width, height), Color.red, Color.red);
                //    Repaint();
                //}

                //if (e.type == EventType.KeyDown)
                //{
                //    if (e.keyCode == KeyCode.N)
                //    {
                //        Vector2 pos = e.mousePosition;
                //        DrawNotWalkable(pos, mapData, WidthPerGrid, origin);
                //    }
                //    else if (e.keyCode == KeyCode.M)
                //    {
                //        Vector2 pos = e.mousePosition;
                //        DrawClimbable(pos, mapData, WidthPerGrid, origin);
                //    }
                //    else if (e.keyCode == KeyCode.Space)
                //    {
                //        Vector2 pos = e.mousePosition;
                //        ClearData(pos, mapData, WidthPerGrid, origin);
                //    }
                //}
            }
		}
	}

    //private void DrawNotWalkable(Vector2 pos,int[,] mapData,float WidthPerGrid,Vector2 origin)
    //{
    //    Ray ray = HandleUtility.GUIPointToWorldRay(pos);
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(ray, out hitInfo))
    //    {
    //        float maxX = origin.x + mapData.GetLength(0) * WidthPerGrid;
    //        float maxY = origin.y + mapData.GetLength(1) * WidthPerGrid;
    //        Vector3 hitPoint = hitInfo.point;
    //        if (hitPoint.x > origin.x && hitPoint.x < maxX && hitPoint.y > origin.y && hitPoint.y < maxY)
    //        {
    //            int i = Mathf.FloorToInt((hitPoint.x - origin.x) / WidthPerGrid);
    //            int j = Mathf.FloorToInt((hitPoint.y - origin.y) / WidthPerGrid);
    //            int curValue = mapData[i, j];
    //            bool walkable = (curValue & SceneMapData.Walkable) != 0;
    //            if (walkable)
    //            {
    //                mapData[i, j] = (curValue & (~SceneMapData.Walkable));
    //                Repaint();
    //            }
    //        }
    //    }
    //}

    //private void DrawClimbable(Vector2 pos, int[,] mapData, float WidthPerGrid, Vector2 origin)
    //{
    //    Ray ray = HandleUtility.GUIPointToWorldRay(pos);
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(ray, out hitInfo))
    //    {
    //        float maxX = origin.x + mapData.GetLength(0) * WidthPerGrid;
    //        float maxY = origin.y + mapData.GetLength(1) * WidthPerGrid;
    //        Vector3 hitPoint = hitInfo.point;
    //        if (hitPoint.x > origin.x && hitPoint.x < maxX && hitPoint.y > origin.y && hitPoint.y < maxY)
    //        {
    //            int i = Mathf.FloorToInt((hitPoint.x - origin.x) / WidthPerGrid);
    //            int j = Mathf.FloorToInt((hitPoint.y - origin.y) / WidthPerGrid);
    //            int curValue = mapData[i, j];
    //            bool climbable = (curValue & SceneMapData.Climbable) != 0;
    //            if (!climbable)
    //            {
    //                mapData[i, j] = (curValue | SceneMapData.Climbable);
    //                Repaint();
    //            }
    //        }
    //    }
    //}

    //private void ClearData(Vector2 pos, int[,] mapData, float WidthPerGrid, Vector2 origin)
    //{
    //    Ray ray = HandleUtility.GUIPointToWorldRay(pos);
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(ray, out hitInfo))
    //    {
    //        float maxX = origin.x + mapData.GetLength(0) * WidthPerGrid;
    //        float maxY = origin.y + mapData.GetLength(1) * WidthPerGrid;
    //        Vector3 hitPoint = hitInfo.point;
    //        if (hitPoint.x > origin.x && hitPoint.x < maxX && hitPoint.y > origin.y && hitPoint.y < maxY)
    //        {
    //            int i = Mathf.FloorToInt((hitPoint.x - origin.x) / WidthPerGrid);
    //            int j = Mathf.FloorToInt((hitPoint.y - origin.y) / WidthPerGrid);
    //            mapData[i, j] = (mapData[i, j] & (~SceneMapData.Climbable));
    //            mapData[i, j] = (mapData[i, j] | SceneMapData.Walkable);
    //            Repaint();
    //        }
    //    }
    //}


	
}



