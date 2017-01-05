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
			GameInputController.Instance.OnKeyClick += OnKeyClick;
		}

		void OnKeyClick (VirtualKey obj)
		{
			if (_unit == null)
				return;
        }

		void OnInputMove (Vector2 obj)
		{
			if (_unit == null)
				return;	
		}

		public void SetOperateUnit(UnitBase unit)
		{
			this._unit = unit;
		}
	}
}

