using System;
using Framework;

namespace Game
{
	[Serializable]
	public class SceneMapData
	{
		public static byte Walkable = 1;
		public static byte Climbable = 2;
		
		public float WidthPerGrid{get;private set;}
		public float OriginX{ get; private set;}
		public float OriginY{ get; private set;}
		public byte[,] Map{ get; private set;}
		public SceneMapData(float originX,float originY,float widthPerGrid,byte[,] map)
		{
			this.OriginX = originX;
			this.OriginY = originY;
			this.WidthPerGrid = widthPerGrid;
			this.Map = map;
		}
	}
}

