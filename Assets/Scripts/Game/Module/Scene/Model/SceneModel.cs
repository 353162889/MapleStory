using Framework;

namespace Game
{

    public class SceneModel : BaseModel<SceneModel>
    {
        public SceneMapMO SceneMapMO { get; set; }
        public SceneCO SceneCO { get; set; }
        public SceneState State { get; set; }

        public override void InitModel()
        {
            this.State = SceneState.Unknow;
            this.SceneCO = null;
            this.SceneMapMO = null;
        }

        public void Clear()
        {
            this.SceneMapMO = null;
            this.SceneCO = null;
            this.State = SceneState.Unknow;
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

    public enum SceneState
    {
        Unknow,
        InScene,
        SceneLoading
    }
}
