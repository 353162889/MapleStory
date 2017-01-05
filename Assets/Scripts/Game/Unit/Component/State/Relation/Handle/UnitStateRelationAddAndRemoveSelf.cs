using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class UnitStateRelationAddAndRemoveSelf : UnitStateRelationHandlerBase
    {
        public override void HandleRelation(UnitBase unit, UnitStateEnum existState, UnitStateEnum enterState, object[] param)
        {
            RemovePropState(unit, existState);
            AddPropState(unit, enterState);
        }
    }
}
