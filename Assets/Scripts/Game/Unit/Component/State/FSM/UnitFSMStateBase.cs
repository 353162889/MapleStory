using System;
using Framework;

namespace Game
{
   
	public class UnitFSMStateBase : FiniteState
	{
		protected UnitBase _unit;
		public UnitFSMStateBase (string stateName,UnitBase unit)
			:base(stateName)
		{
			this._unit = unit;
		}

        public void AcceptInput(FSMStateInputType type,object[] param)
        {
            if(IsInState)
            {
                OnInput(type, param);
            }
        }

        protected virtual void OnInput(FSMStateInputType inputType, object[] param)
        {
        }

        protected override void OnEnter ()
		{
			int beforeState = _unit.PropComponent.PropState;
			int afterState = UnitFSMStateName.GetPropState (this.StateName);
			this._unit.PropComponent.UpdateProperty (UnitProperty.State, afterState);
			if (beforeState != afterState)
			{
				this._unit.DispatchEvent(UnitStateEvent.OnStateChange, beforeState, afterState);
			}
			base.OnEnter ();
		}

		protected void EnableGravity(bool enable)
		{
			UnitGravityCmd cmd = UnitCommandPool.Instance.GetCommand<UnitGravityCmd> (UnitCommandType.Gravity);
			cmd.InitData (UnitCommandExecuteType.Immediately, enable);
			_unit.AcceptCommand (cmd);
		}

		public override void OnDestroy ()
		{
			this._unit = null;
			base.OnDestroy ();
		}
	}
}

