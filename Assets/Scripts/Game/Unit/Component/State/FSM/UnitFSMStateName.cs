using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
	public class UnitFSMStateName
	{
		public static string Ground_Free = "Ground_Free";
		public static string Float_Free = "Float_Free";
		public static string Climb_Free = "Climb_Free";

		public static string[] FSMStateArr = new string[]{
			Ground_Free,Float_Free,Climb_Free
		};

		private static Dictionary<string,int> map = new Dictionary<string, int>{
			{Ground_Free, (1 << (int)UnitStateEnum.Ground) + (1 << (int)UnitStateEnum.Free)},
			{Float_Free, (1 << (int)UnitStateEnum.Float) + (1 << (int)UnitStateEnum.Free)},
			{Climb_Free, (1 << (int)UnitStateEnum.Climb) + (1 << (int)UnitStateEnum.Free)},
		};
		public static int GetPropState(string stateName)
		{
			if (map.ContainsKey (stateName))
			{
				return map [stateName];
			}
			else
			{
				CLog.LogError ("not config UnitFSMStateName:"+stateName + " PropState!");
				return 0;
			}
		}
	}
}

