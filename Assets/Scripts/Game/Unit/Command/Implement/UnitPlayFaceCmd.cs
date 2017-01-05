using System;
using Framework;

namespace Game
{
	public class UnitPlayFaceCmd : UnitCommandBase
	{
		private string _faceName;

		public override UnitCommandType CmdType {
			get {
				return UnitCommandType.PlayFace;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,string faceName)
		{
			base.Init (exeType);
			this._faceName = faceName;
		}

		public override void Execute (Framework.ICommandContext context)
		{
			base.Execute (context);
			UnitAnimatorComponent animComponent = this._unit.GetUnitComponent<UnitAnimatorComponent> ();
			if (animComponent != null)
			{
				animComponent.PlayFace (_faceName);
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
			this._faceName = null;
			base.Reset ();
		}
	}
}

