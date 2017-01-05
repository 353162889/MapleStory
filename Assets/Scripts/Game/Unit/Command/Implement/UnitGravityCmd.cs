using System;
using Framework;

namespace Game
{
	public class UnitGravityCmd : UnitCommandBase
	{
		private bool _enableGravity;
		public override UnitCommandType CmdType
		{
			get
			{
				return UnitCommandType.Gravity;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,bool enableGravity)
		{
			base.Init(exeType);
			_enableGravity = enableGravity;
		}

		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
			UnitMoverComponent moveComponent = (UnitMoverComponent)_unit.GetUnitComponent(UnitComponentType.Mover);
			if (moveComponent != null)
			{
				moveComponent.EnableGravity (_enableGravity);
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				this.OnExecuteDone (CmdExecuteState.Fail);
			}

		}
	}
}

