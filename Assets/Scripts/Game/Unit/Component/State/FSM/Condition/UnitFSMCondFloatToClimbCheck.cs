using System;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
	public class UnitFSMCondFloatToClimbCheck : UnitFSMConditionBase
	{
		private static float OffsetOnGround = 0.3f;
		public UnitFSMCondFloatToClimbCheck (UnitBase unit)
			:base(unit)
		{

		}

		public override bool IsInCondition (Framework.FiniteState curState, string toStateName)
		{
			UnitInputComponent inputComponent = (UnitInputComponent)_unit.GetUnitComponent (UnitComponentType.Input);
			if (inputComponent != null)
			{
				List<UnitInputParam> listParam = inputComponent.GetCacheInput (UnitInputType.Move);
				if (listParam != null && listParam.Count > 0)
				{
					UnitInputParamMove moveParam = (UnitInputParamMove)listParam [listParam.Count - 1];
					if (Mathf.Abs (moveParam.X) < 0.2f && Mathf.Abs(moveParam.Y) > 0.8f)//当x小于某个值时（其实就是和在数值方向的夹角差不多）
					{
						bool climbable = false;
						if (moveParam.Y > 0)
						{
							Vector3 pos = this._unit.transform.position;
							climbable = SceneModel.Instance.IsClimbable(pos.x, pos.y + OffsetOnGround);	
						}
						return climbable;
					}
				}
			}
			return false;
		}
	}
}

