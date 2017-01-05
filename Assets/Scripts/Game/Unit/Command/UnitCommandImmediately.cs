using System;
using Framework;

namespace Game
{
	public class UnitCommandImmediately : CommandImmediately
	{
		public UnitCommandImmediately(bool childFailStop = true,bool isAutoDestroy = true)
			:base(childFailStop,isAutoDestroy)
		{
		}

		protected override void OnChildDestroy (CommandBase command)
		{
			base.OnChildDestroy (command);
			//回收子命令
			UnitCommandPool.Instance.SaveObject((UnitCommandBase)command);
		}
	}
}

