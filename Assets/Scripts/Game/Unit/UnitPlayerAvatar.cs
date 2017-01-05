using System;
using System.Collections.Generic;

namespace Game
{
	public class UnitPlayerAvatar : UnitBase
	{
		public List<string> listAvatar{ get; private set;}

		protected override void InitData (params object[] param)
		{
			this.listAvatar = (List<string>)param[0];
		}

		protected override void InitComponent ()
		{
            base.InitComponent();
			_componentContainer.RegisterComponent (new UnitAnimatorComponent (this));
		}

		protected override void InitFinish ()
		{
			UnitAnimatorComponent component = GetUnitComponent<UnitAnimatorComponent> ();
			component.RefreshFashions (listAvatar);
		}
	}
}

