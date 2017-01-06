using System;
using Framework;

namespace Game
{
	public class UnitJumpCmd : UnitCommandBase
	{
		public override UnitCommandType CmdType
		{
			get
			{
				return UnitCommandType.Jump;
			}
		}

		public void InitData(UnitCommandExecuteType exeType)
		{
			base.Init(exeType);
		}

		public override void Execute (Framework.ICommandContext context)
		{
			base.Execute (context);
			UnitMoverComponent moverComponent = (UnitMoverComponent)_unit.GetUnitComponent (UnitComponentType.Mover);
			if (moverComponent != null)
			{
				moverComponent.Jump();
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				this.OnExecuteDone (CmdExecuteState.Fail);
			}
		}
	}
}

