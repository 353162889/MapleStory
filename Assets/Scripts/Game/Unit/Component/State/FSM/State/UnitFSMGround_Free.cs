using System;
using UnityEngine;

namespace Game
{
	public class UnitFSMGround_Free : UnitFSMStateBase
	{
		public UnitFSMGround_Free(string stateName,UnitBase unit)
			:base(stateName,unit)
		{
		}

		protected override void OnInput (UnitInputParam inputParam)
		{
			if (inputParam.InputType == UnitInputType.Move)
			{
				UnitInputParamMove param = (UnitInputParamMove)inputParam;
				MoveDirection (param.X, 0f);
				if (param.X != 0)
				{
					DoAction (UnitActionBaseDef.Walk);
					UnitFace face = param.X > 0 ? UnitFace.Right : UnitFace.Left;
					ChangeFace (face);
				}
				else
				{
					DoAction (UnitActionBaseDef.Stand);
				}
			}
			else if (inputParam.InputType == UnitInputType.Action)
			{
				UnitInputParamAction param = (UnitInputParamAction)inputParam;
				if (param.ActionName == UnitActionBaseDef.Jump && param.Effective)
				{
					Jump ();
				}
			}
		}

		protected override void OnEnter ()
		{
			base.OnEnter ();
			EnableGravity (false);
            DoAction(UnitActionBaseDef.Stand);
            CheckCacheInputInEnter (UnitInputType.Move);
		}

		public override void OnTick (float dt)
		{
			base.OnTick (dt);
		    if (Mathf.Abs(_unit.PropComponent.Speed.y) < 0.0001f)
		    {
                UpdateMapHeight();
            }
			
		}
	}
}

