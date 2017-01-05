using System;

namespace Framework
{
	public abstract class CommandBaseView : CommandBase
	{
		protected BaseViewController viewController;

		public void Init(BaseViewController viewController)
		{
			this.viewController = viewController;
		}

		public override void OnDestroy ()
		{
			this.viewController = null;
			base.OnDestroy ();
		}

		public abstract CommandViewType CmdType{ get;}
	}
}

