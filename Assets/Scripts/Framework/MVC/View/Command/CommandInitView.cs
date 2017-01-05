using System;
using System.Collections.Generic;

namespace Framework
{
	public class CommandInitView : CommandBaseView
	{
		private MultiResourceLoader _resLoader;
		public override void Execute (ICommandContext context)
		{
			base.Execute (context);
            if(this.viewController.State == ViewState.Initing)
            {
                this.OnExecuteDone(CmdExecuteState.Success);
                return;
            }
			this.viewController.UpdateState (ViewState.Initing);

			List<string> dependResources = this.viewController.DependResources ();
			_resLoader = new MultiResourceLoader ();
			_resLoader.LoadList (dependResources, OnComplete, null, ResourceType.DirectObject);
		}

		private void OnComplete(MultiResourceLoader loader)
		{
			List<Resource> listResources = _resLoader.GetResources ();
			this.viewController.SetResources (listResources);
			this.viewController.InitUI ();
			this.OnExecuteDone (CmdExecuteState.Success);
		}

        public override void OnDestroy()
        {
            if (_resLoader != null)
            {
                _resLoader.Clear();
                _resLoader = null;
            }
            base.OnDestroy ();
		}

		public override CommandViewType CmdType {
			get {
				return CommandViewType.Init;
			}
		}
	}
}

