using System;
using UnityEngine;

namespace Game
{
	public class UnitFSMCondGroundCheck : UnitFSMConditionBase
	{
		private static float OffsetUnderGround = -0.09f;//跟地图的每个碰撞格子的高度有关
		private bool _isGround;
		public UnitFSMCondGroundCheck (UnitBase unit)
			:base(unit)
		{
		}

		public override void InitParam (object[] param)
		{
			base.InitParam (param);
			_isGround = (bool)param [0];
		}

		public override bool IsInCondition (Framework.FiniteState curState, string toStateName)
		{
			return _isGround == IsGround ();
		}

		public bool IsGround()
		{
			Vector3 pos = this._unit.transform.position;
			bool walkable = SceneModel.Instance.IsWalkable(pos.x, pos.y + OffsetUnderGround);
			//如果速度方向往下或为0，并且当前位置不是可行走区域，那么说明已经落地了（并且当前检测点与当前人物所在空间的点不在同一高度）
			Vector2 speed = this._unit.PropComponent.Speed;
			bool isDown = (speed.y > 0) ? false : true;
			return isDown && !walkable;
		}
	}
}

