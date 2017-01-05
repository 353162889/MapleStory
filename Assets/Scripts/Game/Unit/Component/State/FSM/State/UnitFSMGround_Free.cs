using System;

namespace Game
{
	public class UnitFSMGround_Free : UnitFSMStateBase
	{
		public UnitFSMGround_Free(string stateName,UnitBase unit)
			:base(stateName, unit)
		{
		}

		protected override void OnEnter ()
		{
			base.OnEnter ();
			EnableGravity (false);
		}

		protected override void OnExit ()
		{
			EnableGravity (false);
			base.OnExit ();
		}
	}
}

