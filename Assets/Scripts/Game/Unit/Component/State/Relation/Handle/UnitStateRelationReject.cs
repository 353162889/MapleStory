using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class UnitStateRelationReject : UnitStateRelationHandlerBase
    {
		public override bool IsCanHandleRelation (UnitBase unit, UnitStateEnum existState, UnitStateEnum enterState, object[] param)
		{
			return false;
		}
       
    }
}
