using System;
using Framework;

namespace Game
{
	public class UnitInputCmd : UnitCommandBase
	{
		private UnitInputParam _inputParam;
		public override UnitCommandType CmdType {
			get {
				return UnitCommandType.Input;
			}
		}

		public void InitData(UnitCommandExecuteType exeType,UnitInputParam inputParam)
		{
			base.Init (exeType);
			this._inputParam = inputParam;
		}

		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
			UnitInputComponent inputComponent = (UnitInputComponent)_unit.GetUnitComponent (UnitComponentType.Input);
			if (inputComponent != null)
			{
				inputComponent.AcceptInput (_inputParam);
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				UnitInputParamPool.Instance.SaveObject (_inputParam);
				this.OnExecuteDone (CmdExecuteState.Fail);
			}
		}

		public override void Reset ()
		{
			_inputParam = null;
			base.Reset ();
		}
	}
}

