using System;

namespace Game
{
	public class UnitFSMFloat_Free : UnitFSMStateBase
	{
		public UnitFSMFloat_Free(string stateName,UnitBase unit)
			:base(stateName, unit)
		{
		}

		protected override void OnEnter ()
		{
			base.OnEnter ();
			EnableGravity (true);
		}

		protected override void OnExit ()
		{
			EnableGravity (false);
			base.OnExit ();
		}
	}
}

