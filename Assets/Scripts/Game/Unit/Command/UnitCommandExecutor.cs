using System;
using Framework;

namespace Game
{
	public class UnitCommandExecutor
	{
		private UnitCommandDynamicSequence _cmdSequence;
		private UnitCommandImmediately _cmdImmediately;
		public UnitCommandExecutor()
		{
			_cmdSequence = new UnitCommandDynamicSequence (false, false);
			_cmdImmediately = new UnitCommandImmediately (false,false);
		}

		public void ExecuteCommand(UnitCommandBase command)
		{
			if (command.ExeType == UnitCommandExecuteType.Immediately)
			{
				_cmdImmediately.AddSubCommand (command);
			}
			else
			{
				_cmdSequence.AddSubCommand (command);
			}
		}

		public void Dispose()
		{
			_cmdSequence.OnDestroy ();
			_cmdImmediately.OnDestroy ();
		}
	}
}

