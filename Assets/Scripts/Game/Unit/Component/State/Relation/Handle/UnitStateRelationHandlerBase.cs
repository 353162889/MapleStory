using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public abstract class UnitStateRelationHandlerBase
    {
        public virtual void HandleRelation(UnitBase unit, UnitStateEnum existState, UnitStateEnum enterState, object[] param)
		{
		}

		public virtual bool IsCanHandleRelation(UnitBase unit, UnitStateEnum existState, UnitStateEnum enterState, object[] param)
		{
			return true;
		}
        /// <summary>
        /// 添加UnitState
        /// </summary>
        /// <param name="state">UnitState中的状态</param>
        protected void AddPropState(UnitBase unit,UnitStateEnum state)
        {
            int propState = unit.PropComponent.PropState;
            int resultState = (propState | (1 << (byte)state));
            unit.PropComponent.UpdateProperty(UnitProperty.State, resultState);
        }

        /// <summary>
        /// 移除UnitState
        /// </summary>
        /// <param name="state">UnitState中的状态</param>
		protected void RemovePropState(UnitBase unit, UnitStateEnum state)
        {
            int propState = unit.PropComponent.PropState;
            int resultState = (propState & (~(1 << (byte)state)));
            unit.PropComponent.UpdateProperty(UnitProperty.State, resultState);
        }
    }
}
