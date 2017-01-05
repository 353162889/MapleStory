using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class AvatarCO
	{
		private static Dictionary<AvatarPartType,Vector2> MapAnimType = new Dictionary<AvatarPartType, Vector2>{
			{AvatarPartType.Base,new Vector2(2000,20000)},
			{AvatarPartType.Face,new Vector2(20000,30000)},
			{AvatarPartType.Hair,new Vector2(30000,40000)},
		};
		public string ID{ get; private set;}
		public AvatarPartType avatarType{get;private set;}

		public string ResPath{ get; private set;}

		public void Parse(string ID)
		{
			this.ID = ID;
			int value = int.Parse (ID);
			bool hasAvatarType = false;
			foreach (var item in MapAnimType)
			{
				if (value >= item.Value.x && value < item.Value.y)
				{
					avatarType = item.Key;
					hasAvatarType = true;
					break;
				}
			}
			if (!hasAvatarType)
			{
				Debug.LogError ("can not find animType with " + ID);
			}
			ResPath = "Action/Player/" + avatarType.ToString() + "/prefab/" + ID + ".asset";
		}

	}

	public enum AvatarPartType
	{
		Base,		//基础，身体和头
		Face,		//表情
		Hair		//头发
	}
}

