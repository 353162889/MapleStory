using System;

namespace Framework
{
	public class FiniteState
	{
		protected event Action<FiniteState> OnEnterFinish;
		protected event Action<FiniteState> OnExitFinish;

		public string StateName{ get; private set;}

		public bool IsInState{ get; private set;}

		public FiniteState(string stateName)
		{
			this.StateName = stateName;
			IsInState = false;
		}

		public virtual void StateIn()
		{
			this.OnEnter ();
		}

		public virtual void StateOut()
		{
			this.OnExit ();
		}

		protected virtual void OnEnter()
		{
			IsInState = true;
			if (OnEnterFinish != null)
			{
				OnEnterFinish.Invoke (this);
			}
		}

		protected virtual void OnExit()
		{
			IsInState = false;
			if (OnExitFinish != null)
			{
				OnExitFinish.Invoke (this);
			}
		}

		public virtual void OnPause()
		{
		}

		public virtual void OnResume()
		{
		}

		public virtual void OnTick(float dt)
		{
		}

		public void AddEnterListener(Action<FiniteState> action)
		{
			OnEnterFinish += action;
		}

		public void RemoveEnterListener(Action<FiniteState> action)
		{
			if (OnEnterFinish != null)
			{
				OnEnterFinish -= action;
			}
		}

		public void AddExitListener(Action<FiniteState> action)
		{
			OnExitFinish += action;
		}

		public void RemoveExitListener(Action<FiniteState> action)
		{
			if (OnExitFinish != null)
			{
				OnExitFinish -= action;
			}
		}

		public virtual void OnDestroy()
		{
			OnEnterFinish = null;
			OnExitFinish = null;
			IsInState = false;
		}

	}
}

