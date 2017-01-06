using System;
using System.Collections.Generic;

namespace Game
{
	public class UnitPlayer : UnitBase
	{
		public UnitPlayerMO PlayerMO{ get; private set;}

		protected override void InitData (params object[] param)
		{
			this.PlayerMO = (UnitPlayerMO)param[0];
			this.ID = PlayerMO.ID;
            this.gameObject.transform.position = PlayerMO.InitPos;
		}

		protected override void InitComponent ()
		{
            base.InitComponent();
            _componentContainer.RegisterComponent(new UnitStateComponent(this));
            _componentContainer.RegisterComponent(new UnitMoverComponent(this));
            _componentContainer.RegisterComponent (new UnitPlayerAnimatorComponent (this));
			_componentContainer.RegisterComponent (new UnitInputComponent (this));
		}

		protected override void InitFinish ()
		{
			UnitPlayerAnimatorComponent component = GetUnitComponent<UnitPlayerAnimatorComponent> ();
			component.RefreshFashions (PlayerMO.listAvatar);
		}
	}
}

