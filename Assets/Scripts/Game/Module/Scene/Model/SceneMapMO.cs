using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class SceneMapMO
    {
		private int MaxInterpolationGridNum = 1;
        private SceneMapData _sceneMapData;
        //每个的高度数据（可能会有多个高度,也可能没有）
        private float[][] _heightData;
        private float _mapWidth;
		public float MapWidth{get{return _mapWidth;}}
        private float _mapHeight;
		public float MapHeight{get{return _mapHeight;}}
		private float _maxMapX;
		public float MaxMapX{get{return _maxMapX;}}
		private float _maxMapY;
		public float MaxMapY{get{return _maxMapY;}}
		public float MapOriginX{get{ return _sceneMapData.OriginX;}}
		public float MapOriginY{get{ return _sceneMapData.OriginY;}}

		private float MaxInterpolationHeight;

		private Mesh _mapMesh;
        public SceneMapMO(SceneMapData mapData)
        {
            this._sceneMapData = mapData;
			MaxInterpolationHeight = _sceneMapData.WidthPerGrid * MaxInterpolationGridNum;
            int width = _sceneMapData.Map.GetLength(0);
            int height = _sceneMapData.Map.GetLength(1);
            _mapWidth = width * _sceneMapData.WidthPerGrid;
            _mapHeight = height * _sceneMapData.WidthPerGrid;
			_maxMapX = _sceneMapData.OriginX + _mapWidth;
			_maxMapY = _sceneMapData.OriginY + _mapHeight;
            //初始化每个的所有高度数据
            _heightData = new float[width][];
            List<int> tempList = new List<int>();
            for (int i = 0; i < width; i++)
            {
                bool walkable = false;
                for (int j = 0; j < height; j++)
                {
                    byte data = _sceneMapData.Map[i, j];
                    if (!IsWalkable(data))
                    {
                        walkable = true;
                    }
                    else if(walkable)
                    {
                        tempList.Add(j - 1);
                        walkable = false;
                    }
                }
                _heightData[i] = new float[tempList.Count];
                for (int j = 0; j < tempList.Count; j++)
                {
                    _heightData[i][j] = (tempList[j] + 1) * _sceneMapData.WidthPerGrid;
                }
                tempList.Clear();
            }
        }

		/// <summary>
		/// 获取可移动的x坐标
		/// </summary>
		/// <returns>The x.</returns>
		/// <param name="x">The x coordinate.</param>
		public float GetX(float x)
		{
			if (x < _sceneMapData.OriginX)
			{
				return _sceneMapData.OriginX;
			}
			else if (x > _maxMapX)
			{
				return _maxMapX;
			}
			return x;
		}
		/// <summary>
		/// 获取可移动的y坐标
		/// </summary>
		/// <returns>The y.</returns>
		/// <param name="y">The y coordinate.</param>
		public float GetY(float y)
		{
			if (y < _sceneMapData.OriginY)
			{
				return _sceneMapData.OriginY;
			}
			else if (y > _maxMapY)
			{
				return _maxMapY;
			}
			return y;
		}

        public float GetCurMapHeight(float x,float y)
        {
            float tempX = x - _sceneMapData.OriginX;
            float tempY = y - _sceneMapData.OriginY;
            if (tempX < 0 || tempY < 0 || x >= _mapWidth || y >= _mapHeight)
            {
                CLog.Log("cur x="+x + ",y="+y+" over map!");
                return _sceneMapData.OriginY;
            }
            int widthIndex = Mathf.FloorToInt(tempX / _sceneMapData.WidthPerGrid);
            float[] heights = _heightData[widthIndex];
            int length = heights.Length;
			int i;
            for (i = length - 1; i >= 0; i--)
            {
                if(y > heights[i])
                {
//					break;
                    return heights[i];
                }
            }
//			if (i >= 0)
//			{
//				float subValue = x - Mathf.FloorToInt (x);
//				if (subValue > 0.5f && i + 1 < _heightData.Length && Mathf.Abs(heights[i+1] - heights[i]) < MaxInterpolationHeight)
//				{
//					return Mathf.Lerp (heights [i], heights [i + 1], subValue - 0.5f);
//				}
//				else if(subValue < 0.5f && i - 1 >= 0 && Mathf.Abs(heights[i - 1] - heights[i]) < MaxInterpolationHeight)
//				{
//					return Mathf.Lerp (heights [i - 1], heights [i], subValue + 0.5f);
//				}
//				return heights[i];
//			}
            //默认返回地图最低值，如果是脚底下没有路了
            return _sceneMapData.OriginY;

        }

        public bool IsWalkable(float x,float y)
        {
            int result = GetMapPosData(x, y);
            if (result < 0) return false;
            return IsWalkable((byte)result);
        }

        public bool IsWalkable(byte data)
        {
            return (data & SceneMapData.Walkable) != 0;
        }

        public bool IsClimbable(float x,float y)
        {
            int result = GetMapPosData(x, y);
            if (result < 0) return false;
            return IsClimbable((byte)result);
        }

        public bool IsClimbable(byte data)
        {
            return (data & SceneMapData.Climbable) != 0;
        }

        public int GetMapPosData(float x, float y)
        {
            float tempX = x - _sceneMapData.OriginX;
            float tempY = y - _sceneMapData.OriginY;
            if (tempX < 0 || tempY < 0 || x >= _mapWidth || y >= _mapHeight)
            {
                CLog.Log("cur x=" + x + ",y=" + y + " over map!");
                return -1;
            }
            int widthIndex = Mathf.FloorToInt(tempX / _sceneMapData.WidthPerGrid);
            int heightIndex = Mathf.FloorToInt(tempY / _sceneMapData.WidthPerGrid);
            return (int)_sceneMapData.Map[widthIndex,heightIndex];
        }


		public Mesh GetMapMesh()
		{
			if (_mapMesh != null)
				return _mapMesh;
			_mapMesh = new Mesh ();
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int> ();
			List<Color> colors = new List<Color> ();
			List<Vector3> normals = new List<Vector3> ();
			//使用贪心算法将网格数据计算出来
			int width = _sceneMapData.Map.GetLength(0);
			int height = _sceneMapData.Map.GetLength(1);
			bool[,] mapIsDone = new bool[width,height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (mapIsDone [i, j])
						continue;
					byte curValue = _sceneMapData.Map [i, j];
					int curWidth;
					int curHeight;
					for (curWidth = i + 1; curWidth < width; curWidth++)
					{
						if (mapIsDone [curWidth, j] || _sceneMapData.Map [curWidth, j] != curValue)
						{
							break;
						}
					}
					for (curHeight = j + 1; curHeight < height; curHeight++)
					{
						int k;
						for (k = i; k < curWidth; k++)
						{
							if (mapIsDone [k, curHeight] || _sceneMapData.Map [k, curHeight] != curValue)
							{
								break;
							}
						}
						if (k < curWidth)
						{
							break;
						}
					}
					for (int x = i; x < curWidth; x++) {
						for (int y = j; y < curHeight; y++) {
							mapIsDone [x, y] = true;
						}
					}

					//添加网格
					Vector3 v1 = new Vector3 (i * _sceneMapData.WidthPerGrid, j * _sceneMapData.WidthPerGrid, 0);
					Vector3 v2 = new Vector3 (curWidth * _sceneMapData.WidthPerGrid, j * _sceneMapData.WidthPerGrid, 0);
					Vector3 v3 = new Vector3 (curWidth * _sceneMapData.WidthPerGrid, curHeight * _sceneMapData.WidthPerGrid, 0);
					Vector3 v4 = new Vector3 (i * _sceneMapData.WidthPerGrid, curHeight * _sceneMapData.WidthPerGrid, 0);
					vertices.Add (v1);
					vertices.Add (v2);
					vertices.Add (v3);
					vertices.Add (v4);

					triangles.Add (vertices.Count - 1);
					triangles.Add (vertices.Count - 3);
					triangles.Add (vertices.Count - 4);

					triangles.Add (vertices.Count - 1);
					triangles.Add (vertices.Count - 2);
					triangles.Add (vertices.Count - 3);

					bool walkable = IsWalkable (curValue);
					bool climbable = IsClimbable (curValue);
					Color color;
					if (!walkable || climbable)
					{
						color = new Color (walkable ? 0 : 1, climbable ? 1f : 0, 0);
					}
					else
					{
						color = Color.clear;
					}
					colors.Add (color);
					colors.Add (color);
					colors.Add (color);
					colors.Add (color);

					normals.Add (new Vector3(0,0,-1f));
					normals.Add (new Vector3(0,0,-1f));
					normals.Add (new Vector3(0,0,-1f));
					normals.Add (new Vector3(0,0,-1f));
				}
			}
			_mapMesh.vertices = vertices.ToArray ();
			_mapMesh.triangles = triangles.ToArray ();
			_mapMesh.colors = colors.ToArray ();
			_mapMesh.normals = normals.ToArray ();
			return _mapMesh;
		}

    }
}
