using System;

namespace Game
{
	public class UnitFSMClimb_Free : UnitFSMStateBase
	{
		public UnitFSMClimb_Free(string stateName,UnitBase unit)
			:base(stateName,unit)
		{
		}

		protected override void OnInput (UnitInputParam inputParam)
		{
			base.OnInput (inputParam);
		}
	}
}

