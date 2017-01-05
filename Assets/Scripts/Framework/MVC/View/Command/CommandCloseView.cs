using System;
using System.Collections.Generic;

namespace Framework
{
	public class CommandCloseView : CommandBaseView
	{
		private CommandSequence _animPlayer;
		public override CommandViewType CmdType {
			get {
				return CommandViewType.Close;
			}
		}

		public CommandCloseView()
		{
			_animPlayer = new CommandSequence ();
		}

		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
            if (this.viewController.State == ViewState.Closing || this.viewController.State == ViewState.Close)
            {
                this.OnExecuteDone(CmdExecuteState.Fail);
                return;
            }
			this.viewController.UpdateState (ViewState.Closing);
			this.viewController.Exit ();
			List<BaseViewAnim> listAnim = this.viewController.BuildCloseAnims ();
			if (listAnim == null || listAnim.Count == 0)
			{
				this.viewController.OnCloseAnimDone ();
				this.OnExecuteDone (CmdExecuteState.Success);
			}
			else
			{
				for (int i = 0; i < listAnim.Count; i++)
				{
					_animPlayer.AddSubCommand (listAnim[i]);
				}
				_animPlayer.On_Done += AfterPlayAnim;
				_animPlayer.Execute ();
			}
		}

		private void AfterPlayAnim(CommandBase command)
		{
			this.viewController.OnCloseAnimDone ();
			//这里销毁是先CommandBaseView先销毁，然后_animPlayer销毁
			this.OnExecuteDone (CmdExecuteState.Success);
		}

		protected override void OnExecuteFinish ()
		{
            if (this.State == CmdExecuteState.Success)
            {
                this.viewController.UpdateState(ViewState.Close);
            }
		}

		public override void OnDestroy ()
		{
			_animPlayer.OnDestroy ();
			base.OnDestroy ();
		}

	}
}

