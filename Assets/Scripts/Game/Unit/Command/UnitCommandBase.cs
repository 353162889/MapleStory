using System;
using Framework;

namespace Game
{
	public abstract class UnitCommandBase : CommandBase
	{
		protected UnitBase _unit;

		public abstract UnitCommandType CmdType{ get;}

		public UnitCommandExecuteType ExeType{ get; private set;}

		//子类还是不要和这个一样的名字，有坑来着，如果子类用了this.Init（并且有params参数），就...掉坑里了
		protected void Init(UnitCommandExecuteType exeType)
		{
			this.ExeType = exeType;
		}

		public void BindUnit(UnitBase unit)
		{
			this._unit = unit;
		}

		public override void Reset ()
		{
			_unit = null;
			ExeType = UnitCommandExecuteType.Immediately;
			base.Reset ();
		}
	}
}

