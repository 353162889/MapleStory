using System;

namespace Game
{
	public class UnitChangeFaceCmd : UnitCommandBase
	{
		private UnitFace _face;
		public override UnitCommandType CmdType {
			get {
				return UnitCommandType.ChangeFace;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,UnitFace face)
		{
			base.Init (exeType);
			this._face = face;
		}

		public override void Execute (Framework.ICommandContext context)
		{
			base.Execute (context);
			UnitAnimatorComponent animatorComponent = (UnitAnimatorComponent)_unit.GetUnitComponent (UnitComponentType.Animator);
			if (animatorComponent != null)
			{
				animatorComponent.ChangeUnitFace (_face);
				this.OnExecuteDone (Framework.CmdExecuteState.Success);
			}
			else
			{
				this.OnExecuteDone (Framework.CmdExecuteState.Fail);
			}
		}

		public override void Reset ()
		{
			_face = UnitFace.Right;
			base.Reset ();
		}
	}
}

