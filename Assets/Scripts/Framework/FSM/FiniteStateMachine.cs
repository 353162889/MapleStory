using System;
using System.Collections.Generic;

namespace Framework
{
	public enum FSOperateStatus
	{
		Success,
		Fail,
		InTransition,
		Executing,
	}
	public class FiniteStateMachine
	{
		private Dictionary<string,FiniteState> _mapState;
		private Dictionary<string,List<FiniteStateTransition>> _mapFromTransitions;
		private Dictionary<string,FiniteStateTransition> _mapFromAndToTransition;

		private bool _isPause;
		private bool _isTransition;
		private FiniteState _currentState;
		private FiniteState _toState;

		public FiniteStateMachine()
		{
			_mapState = new Dictionary<string, FiniteState> ();
			_mapFromTransitions = new Dictionary<string, List<FiniteStateTransition>> ();
			_mapFromAndToTransition = new Dictionary<string, FiniteStateTransition> ();
			_isPause = true;
			_isTransition = false;
			_toState = null;
			_currentState = null;
		}

		public void StartRun()
		{
			_isPause = false;
		}

		public FSOperateStatus AddState(FiniteState state)
		{
			if (!_mapState.ContainsKey (state.StateName))
			{
				_mapState.Add (state.StateName, state);
				return FSOperateStatus.Success;
			}
			return FSOperateStatus.Fail;
		}

		public FSOperateStatus RemoveState(string stateName)
		{
			if (_mapState.ContainsKey (stateName))
			{
				if (IsCurrentState (stateName))
				{
					return FSOperateStatus.Executing;
				}
				FiniteState state = _mapState [stateName];
				_mapState.Remove (stateName);
				state.OnDestroy ();
				return FSOperateStatus.Success;
			}
			return FSOperateStatus.Fail;
		}

		public bool IsCurrentState(string stateName)
		{
			return _currentState != null && _currentState.StateName == stateName;
		}

		public FSOperateStatus AddTransition(string fromState,string toState,IFiniteStateCondition condition)
		{
			string transitionName = fromState + "$" + toState;
			if (!_mapFromAndToTransition.ContainsKey (transitionName))
			{
				FiniteStateTransition transition = new FiniteStateTransition (fromState, toState, condition);
				_mapFromAndToTransition.Add (transitionName, transition);
				List<FiniteStateTransition> list;
				_mapFromTransitions.TryGetValue (fromState, out list);
				if (list == null)
				{
					list = new List<FiniteStateTransition> ();
					_mapFromTransitions.Add (fromState,list);
				}
				list.Add (transition);
			}
			_mapFromAndToTransition [transitionName].ChangeCondition (condition);
			return FSOperateStatus.Success;
		}

		public FSOperateStatus RemoveTransition(string fromState,string toState)
		{
			string transitionName = fromState + "$" + toState;
			if (_mapFromAndToTransition.ContainsKey (transitionName))
			{
				FiniteStateTransition transition = _mapFromAndToTransition [transitionName];
				_mapFromAndToTransition.Remove (transitionName);
				List<FiniteStateTransition> list = _mapFromTransitions [fromState];
				list.Remove (transition);
				transition.OnDestroy ();
				return FSOperateStatus.Success;
			}
			return FSOperateStatus.Fail;
		}

		public void OnTick(float dt)
		{
			if (!_isPause)
			{
				if (!_isTransition)
				{
					string toState = TryGetNextTransition ();
					if (string.IsNullOrEmpty (toState))
					{
						if (_currentState != null)
						{
							_currentState.OnTick (dt);
						}
					}
					else
					{
						EnterState (toState);
					}
				}
			}
		}

		public void Pause()
		{
			if (!_isPause)
			{
				_isPause = true;
				if (_currentState != null)
				{
					_currentState.OnPause ();
				}
			}
		}

		public void Resume()
		{
			if (_isPause)
			{
				_isPause = false;
				if (_currentState != null)
				{
					_currentState.OnResume ();
				}
			}
		}

		/// <summary>
		/// 判断当前是否可以进入某个状体（外部使用）
		/// </summary>
		/// <returns><c>true</c> if this instance can enter state the specified stateName; otherwise, <c>false</c>.</returns>
		/// <param name="stateName">State name.</param>
		public bool CanEnterState(string stateName)
		{
			if (_isTransition)
			{
				return false;
			}
			FiniteState state;
			this._mapState.TryGetValue (stateName, out state);
			if (state == null)
			{
				return false;
			}
			if (_currentState != null)
			{
				return true;
			}
			string transitionName = _currentState.StateName + "$" + state.StateName;
			FiniteStateTransition transition;
			_mapFromAndToTransition.TryGetValue (transitionName, out transition);
			if (transition == null)
			{
				return false;
			}
			return transition.IsInCondition(_currentState);
		}

		protected string TryGetNextTransition()
		{
			if (_currentState == null)
				return null;
			List<FiniteStateTransition> list;
			_mapFromTransitions.TryGetValue (_currentState.StateName, out list);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list [i].IsInCondition (_currentState))
					{
						return list [i].ToState;
					}
				}
			}
			return null;
		}

		public FSOperateStatus EnterState(string stateName)
		{
			FiniteState state;
			this._mapState.TryGetValue (stateName, out state);
			return EnterState (state);
		}

		/// <summary>
		/// 进入某状态，不需要判断当前过渡条件
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="state">State.</param>
		protected FSOperateStatus EnterState(FiniteState state)
		{
			if (state == null)
			{
				return FSOperateStatus.Fail;
			}
			if (_isTransition)
			{
				return FSOperateStatus.InTransition;
			}
			Translate (state);
			return FSOperateStatus.Success;
		}

		protected void Translate(FiniteState state)
		{
			_isTransition = true;
			_toState = state;
			if (_currentState != null)
			{
				AddStateExitListener (_currentState.StateName, OnExitDone);
				_currentState.StateOut ();
			}
			else
			{
				OnExitDone (null);
			}
		}

		protected void OnExitDone(FiniteState state)
		{
			if (state != null)
			{
				RemoveStateExitListener (state.StateName, OnExitDone);
			}
			if (_toState != null)
			{
				AddStateEnterListener (_toState.StateName, OnEnterDone);
				_currentState = _toState;
				_toState = null;
				_currentState.StateIn ();
			}
			else
			{
				_currentState = null;
				OnEnterDone (null);
			}
		}

		protected void OnEnterDone(FiniteState state)
		{
			if (state != null)
			{
				RemoveStateEnterListener (state.StateName, OnEnterDone);
			}
			_toState = null;
			_isTransition = false;
		}

		public void AddStateEnterListener(string stateName, Action<FiniteState> action)
		{
			FiniteState state;
			_mapState.TryGetValue (stateName, out state);
			if (state != null)
			{
				state.AddEnterListener (action);
			}
		}

		public void RemoveStateEnterListener(string stateName, Action<FiniteState> action)
		{
			FiniteState state;
			_mapState.TryGetValue (stateName, out state);
			if (state != null)
			{
				state.RemoveEnterListener (action);
			}
		}

		public void AddStateExitListener(string stateName, Action<FiniteState> action)
		{
			FiniteState state;
			_mapState.TryGetValue (stateName, out state);
			if (state != null)
			{
				state.AddExitListener (action);
			}
		}

		public void RemoveStateExitListener(string stateName, Action<FiniteState> action)
		{
			FiniteState state;
			_mapState.TryGetValue (stateName, out state);
			if (state != null)
			{
				state.RemoveExitListener (action);
			}
		}

		public void OnDestroy()
		{
			foreach (var item in _mapState)
			{
				item.Value.OnDestroy ();
			}
			_mapState.Clear ();
			foreach (var item in _mapFromAndToTransition)
			{
				item.Value.OnDestroy ();
			}
			_mapFromAndToTransition.Clear ();
			_mapFromTransitions.Clear ();
			_currentState = null;
			_toState = null;
			_isPause = true;
			_isTransition = false;
		}
	}
}

