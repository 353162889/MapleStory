using System;
using System.Collections.Generic;

namespace Framework
{
	public class CommandOpenView : CommandBaseView
	{
		private object[] _param;
		private CommandSequence _animPlayer;

		public CommandOpenView()
		{
			_animPlayer = new CommandSequence ();
		}

		public void Init(BaseViewController viewController,params object[] param)
		{
            //这里只能用base.Init，不然会重复调用
			base.Init (viewController);
			this._param = param;
		}

		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
            if (this.viewController.State == ViewState.Opening || this.viewController.State == ViewState.Open)
            {
                this.OnExecuteDone(CmdExecuteState.Fail);
                return;
            }
            this.viewController.UpdateState (ViewState.Opening);
			this.viewController.Enter (_param);
			List<BaseViewAnim> listAnim = this.viewController.BuildOpenAnims ();
			if (listAnim == null || listAnim.Count == 0)
			{
				this.viewController.OnOpenAnimDone ();
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
			this.viewController.OnOpenAnimDone ();
            //这里销毁是先CommandBaseView先销毁，然后_animPlayer销毁
            this.OnExecuteDone (CmdExecuteState.Success);
		}

        protected override void OnExecuteFinish()
        {
            if(this.State == CmdExecuteState.Success)
            { 
                this.viewController.UpdateState(ViewState.Open);
            }
        }

        public override void OnDestroy ()
		{
			this._param = null;
			_animPlayer.OnDestroy ();
			base.OnDestroy ();
		}

		public override CommandViewType CmdType {
			get {
				return CommandViewType.Open;
			}
		}
	}
}

