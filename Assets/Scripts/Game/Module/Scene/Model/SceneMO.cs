using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SceneMO
    {
        public SceneMapMO SceneMapMO { get; private set; }
        public SceneCO SceneCO { get; private set; }
        public SceneMO(SceneCO sceneCO,SceneMapMO sceneMapMO)
        {
            this.SceneCO = sceneCO;
            this.SceneMapMO = SceneMapMO;
        }

        public void UpdateSceneData(string sceneData)
        {
            SceneMapData sceneMapData = StringUtil.Deserialize<SceneMapData>(sceneData);
            SceneMapMO = new SceneMapMO(sceneMapData);
        }

		/// <summary>
		/// 获取可移动的x坐标
		/// </summary>
		/// <returns>The x.</returns>
		/// <param name="x">The x coordinate.</param>
		public float GetX(float x)
		{
			return SceneMapMO.GetX (x);
		}
		/// <summary>
		/// 获取可移动的y坐标
		/// </summary>
		/// <returns>The y.</returns>
		/// <param name="y">The y coordinate.</param>
		public float GetY(float y)
		{
			return SceneMapMO.GetY (y);
		}

        public float GetCurMapHeight(float x, float y)
        {
            return SceneMapMO.GetCurMapHeight(x, y);
        }

        public bool IsWalkable(float x, float y)
        {
            return SceneMapMO.IsWalkable(x, y);
        }

        public bool IsWalkable(byte data)
        {
            return SceneMapMO.IsWalkable(data);
        }

        public bool IsClimbable(float x, float y)
        {
            return SceneMapMO.IsClimbable(x, y);
        }

        public bool IsClimbable(byte data)
        {
            return SceneMapMO.IsClimbable(data);
        }

        public int GetMapPosData(float x, float y)
        {
            return SceneMapMO.GetMapPosData(x, y);
        }
    }

  
}
