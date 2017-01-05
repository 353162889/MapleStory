using System;
using System.Collections.Generic;

namespace Game
{
	public class UnitStateRelation
	{
		public UnitStateRelationType Type { get; private set; }
		public object[] Param { get; private set; }
		public UnitStateRelation(int type,object[] param)
		{
			this.Type = (UnitStateRelationType)type;
			this.Param = param;
		}
	}
	public enum UnitStateRelationType
	{
		/// <summary>
		/// 拒绝当前进入的状态
		/// </summary>
		Reject = 0,
		/// <summary>
		/// 添加追加状态，移除当前状态
		/// </summary>
		AddAndRemoveSelf = 1,
		/// <summary>
		/// 直接添加
		/// </summary>
		Add = 2,

	}

	public class UnitStateRelationManager
	{
		private List<UnitStateRelation> listRelation = new List<UnitStateRelation>();
		private List<UnitStateEnum> listState = new List<UnitStateEnum>();
		private UnitStateRelation[,] mapRelation;

		public UnitStateRelationManager()
		{
			mapRelation = GetRelationMap ();
		}

		protected virtual UnitStateRelation[,] GetRelationMap()
		{
			return new UnitStateRelation[,]{ 
				//Ground   		Float       	Climb			Free
				{USR(0),    	USR(1),   		USR(1),			USR(2) }, //Groud
				{USR(1),    	USR(0),     	USR(1),			USR(2) }, //Float
				{USR(1),   		USR(1),     	USR(0),			USR(2) }, //Climb
				{USR(2),   		USR(2),     	USR(2),			USR(0) }, //Free
			};
		}

		protected UnitStateRelation USR(int type,params object[] param)
		{
			return new UnitStateRelation (type, param);
		}

		public void HandleEnterState(UnitBase unit,UnitStateEnum enterState)
		{
			listRelation.Clear();
			listState.Clear();
			int propState = unit.PropComponent.PropState;
			//求出当前列
			int row = (int)enterState;
			//求出列表
			UnitStateValue.GetListState(propState, ref listState);
			for (int i = 0; i < listState.Count; i++)
			{
				listRelation.Add(mapRelation[(int)listState[i],row]);
			}

			int count = listRelation.Count;
			for (int i = 0; i < count; i++)
			{
				UnitStateRelationHandlerBase handler = UnitStateRelationHandlerFactory.GetHandle(listRelation[i].Type);
				if(!handler.IsCanHandleRelation(unit, listState[i], enterState, listRelation[i].Param))return;
			}
			for (int i = 0; i < count; i++)
			{
				UnitStateRelationHandlerBase handler = UnitStateRelationHandlerFactory.GetHandle(listRelation[i].Type);
				handler.HandleRelation(unit, listState[i], enterState, listRelation[i].Param);
			}
		}
	}
}

