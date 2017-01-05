using System;

namespace Framework
{
	public class CommandDestroyView : CommandBaseView
	{
		protected int _delayTime;
		public void Init(BaseViewController viewController,int delayTime)
		{
			base.Init (viewController);
			this._delayTime = delayTime;
		}

		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
            if(this.viewController.State == ViewState.Destroying)
            {
                this.OnExecuteDone(CmdExecuteState.Success);
                return;
            }
			this.viewController.UpdateState (ViewState.Destroying);
			if (_delayTime == 0)
			{
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				UpdateScheduler.Instance.AddScheduler (OnDelay, (float)_delayTime, 1);
			}
		}

		private void OnDelay(float dt)
		{
			this.OnExecuteDone (CmdExecuteState.Success);
		}

		public override void OnDestroy ()
		{
			_delayTime = 0;
			UpdateScheduler.Instance.RemoveScheduler (OnDelay);
			base.OnDestroy ();
		}

		public override CommandViewType CmdType {
			get {
				return CommandViewType.Destroy;
			}
		}
	}
}

