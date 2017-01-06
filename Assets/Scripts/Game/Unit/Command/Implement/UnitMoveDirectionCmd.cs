using System;

namespace Game
{
	public class UnitMoveDirectionCmd : UnitCommandBase
	{
		public float X{ get; private set;}
		public float Y{ get; private set;}
		public override UnitCommandType CmdType {
			get {
				return UnitCommandType.MoveDirection;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,float x,float y)
		{
			base.Init (exeType);
			this.X = x;
			this.Y = y;
		}

		public override void Execute (Framework.ICommandContext context)
		{
			base.Execute (context);
			UnitMoverComponent moverComponent = (UnitMoverComponent)_unit.GetUnitComponent (UnitComponentType.Mover);
			if (moverComponent != null)
			{
				moverComponent.ChangeDirection (X, Y);
				this.OnExecuteDone (Framework.CmdExecuteState.Success);
			}
			else
			{
				this.OnExecuteDone (Framework.CmdExecuteState.Fail);
			}
		}

		public override void Reset ()
		{
			this.X = 0;
			this.Y = 0;
			base.Reset ();
		}
	}
}

