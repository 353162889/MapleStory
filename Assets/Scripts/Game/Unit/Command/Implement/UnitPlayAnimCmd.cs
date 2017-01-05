using System;
using Framework;
using UnityEngine;

namespace Game
{
	public class UnitPlayAnimCmd : UnitCommandBase
	{
		private string _animName;
		public override UnitCommandType CmdType {
			get {
				return UnitCommandType.PlayAnim;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,string animName)
		{
			base.Init (exeType);
			this._animName = animName;
		}

		public override void Execute (Framework.ICommandContext context)
		{
			base.Execute (context);
			UnitAnimatorComponent animComponent = this._unit.GetUnitComponent<UnitAnimatorComponent> ();
			if (animComponent != null)
			{
				animComponent.PlayAnim (_animName);
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				CLog.Log ("can not find UnitAnimatorComponent in " + this._unit + ",ID:" + _unit.ID,CLogColor.Yellow);
				this.OnExecuteDone (CmdExecuteState.Fail);
			}
		}

		public override void Reset ()
		{
			this._animName = null;
			base.Reset ();
		}
	}
}

