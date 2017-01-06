using System;
using Framework;
using UnityEngine;

namespace Game
{
	public class UnitOperateController : BaseController<UnitOperateController>
	{
		private UnitBase _unit;
		public override void InitController ()
		{
			GameInputController.Instance.OnInputMove += OnInputMove;
			GameInputController.Instance.OnKeyDown += OnKeyDown;
			GameInputController.Instance.OnKeyUp += OnKeyUp;
		}

		void OnKeyDown (VirtualKey obj)
		{
			if (_unit == null)
				return;
			if (obj == VirtualKey.KeyJump)
			{
				UnitInputParamAction param = UnitInputParamPool.Instance.GetInputParam<UnitInputParamAction> (UnitInputType.Action);
				param.InitData (UnitActionBaseDef.Jump, true);
				UnitInputCmd inputCmd = UnitCommandPool.Instance.GetCommand<UnitInputCmd> (UnitCommandType.Input);
				inputCmd.InitData (UnitCommandExecuteType.Immediately, param);
				_unit.AcceptCommand (inputCmd);
			}
        }

		void OnKeyUp (VirtualKey obj)
		{
			if (_unit == null)
				return;
			if (obj == VirtualKey.KeyJump)
			{
				UnitInputParamAction param = UnitInputParamPool.Instance.GetInputParam<UnitInputParamAction> (UnitInputType.Action);
				param.InitData (UnitActionBaseDef.Jump, false);
				UnitInputCmd inputCmd = UnitCommandPool.Instance.GetCommand<UnitInputCmd> (UnitCommandType.Input);
				inputCmd.InitData (UnitCommandExecuteType.Immediately, param);
				_unit.AcceptCommand (inputCmd);
			}
		}

		void OnInputMove (Vector2 obj)
		{
			if (_unit == null)
				return;
			UnitInputParamMove param = UnitInputParamPool.Instance.GetInputParam<UnitInputParamMove> (UnitInputType.Move);
			param.InitData (obj.x,obj.y);
			UnitInputCmd inputCmd = UnitCommandPool.Instance.GetCommand<UnitInputCmd> (UnitCommandType.Input);
			inputCmd.InitData (UnitCommandExecuteType.Immediately, param);
			_unit.AcceptCommand (inputCmd);
		}

		public void SetOperateUnit(UnitBase unit)
		{
			this._unit = unit;
		}
	}
}

