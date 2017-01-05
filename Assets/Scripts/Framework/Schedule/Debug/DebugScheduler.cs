using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class DebugScheduler : SingletonMonoBehaviour<DebugScheduler>
	{
		private bool _isDebugMode;
		public bool IsDebugMode{
			get{ 
				return _isDebugMode;
			}
			set
			{ 
				_isDebugMode = value;
				DebugLateUpdateScheduler.Instance.IsDebugMode = value;
				DebugGizmosScheduler.Instance.IsDebugMode = value;
			}
		}

		protected override void Init ()
		{
			DebugGizmosScheduler.CreateInstance (this.gameObject);
			DebugLateUpdateScheduler.CreateInstance (this.gameObject);
			IsDebugMode = false;
		}

		public bool AddScheduler(DebugSchedulerType type,SchedulerHandler handler,float delay,int times = 0)
		{
			if (type == DebugSchedulerType.Gizmos)
			{
				return DebugGizmosScheduler.Instance.AddScheduler (handler, delay, times);
			}
			else if (type == DebugSchedulerType.LateUpdate)
			{
				return DebugLateUpdateScheduler.Instance.AddScheduler (handler, delay, times);
			}
			return false;
		}

		public void RemoveScheduler(DebugSchedulerType type,SchedulerHandler handler)
		{
			if (type == DebugSchedulerType.Gizmos)
			{
				DebugGizmosScheduler.Instance.RemoveScheduler (handler);
			}
			else if (type == DebugSchedulerType.LateUpdate)
			{
				DebugLateUpdateScheduler.Instance.RemoveScheduler (handler);
			}
		}
	}

	public enum DebugSchedulerType
	{
		LateUpdate,
		Gizmos
	}
}

