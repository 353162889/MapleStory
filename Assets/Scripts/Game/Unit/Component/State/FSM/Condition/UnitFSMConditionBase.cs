using System;
using Framework;

namespace Game
{
	public class UnitFSMConditionBase : IFiniteStateCondition
	{
		protected UnitBase _unit;
		protected object[] _param;

		public UnitFSMConditionBase(UnitBase unit)
		{
			this._unit = unit;
		}

		public virtual void InitParam(object[] param)
		{
			this._param = param;
		}

		public virtual void Reset()
		{
		}

		public virtual bool IsInCondition(FiniteState curState,string toStateName)
		{
			return false;
		}

		public virtual void Dispose()
		{
			this._unit = null;
			_param = null;
		}
	}
}

