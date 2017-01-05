using System;

namespace Framework
{
	public class BaseViewAnim : CommandBase
	{
		public sealed override void Execute (ICommandContext context)
		{
			base.Execute (context);
			OnPlay ();
		}

		public sealed override void OnDestroy ()
		{
			OnStop ();
			base.OnDestroy ();
		}

		public sealed override void Reset ()
		{
			base.Reset ();
		}

		protected sealed override void OnExecuteDone (CmdExecuteState state)
		{
			base.OnExecuteDone (state);
		}

		protected sealed override void OnExecuteFinish ()
		{
			base.OnExecuteFinish ();
		}

		protected virtual void OnPlay()
		{
		}

		protected virtual void OnStop()
		{
		}

		protected void FinishPlay()
		{
			this.OnExecuteDone (CmdExecuteState.Success);
		}
	}
}

