using System;

namespace Framework
{
	public interface IFiniteStateCondition
	{
		void Reset();
		bool IsInCondition(FiniteState curState,string toStateName);
		void Dispose();
	}
}

