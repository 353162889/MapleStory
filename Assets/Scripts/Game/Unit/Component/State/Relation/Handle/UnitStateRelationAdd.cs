using System;

namespace Game
{
	public class UnitStateRelationAdd : UnitStateRelationHandlerBase
	{
		public override void HandleRelation (UnitBase unit, UnitStateEnum existState, UnitStateEnum enterState, object[] param)
		{
			AddPropState (unit, enterState);
		}
	}
}

