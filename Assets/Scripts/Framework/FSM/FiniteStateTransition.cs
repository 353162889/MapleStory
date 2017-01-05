using System;

namespace Framework
{
	public class FiniteStateTransition
	{
		public string FromState{ get; private set;}
		public string ToState{ get; private set;}

		private IFiniteStateCondition _condition;

		public FiniteStateTransition (string fromState,string toState,IFiniteStateCondition condition)
		{
			this.FromState = fromState;
			this.ToState = toState;
			ChangeCondition (condition);
		}

		public void ChangeCondition(IFiniteStateCondition condition)
		{
			this._condition = condition;
		}

		public bool IsInCondition(FiniteState curState)
		{
			if (_condition != null)
			{
				_condition.Reset ();
				return _condition.IsInCondition (curState,ToState);
			}
			return false;
		}

		public void OnDestroy()
		{
			if (_condition != null)
			{
				_condition.Dispose ();
				_condition = null;
			}
		}
	}
}

