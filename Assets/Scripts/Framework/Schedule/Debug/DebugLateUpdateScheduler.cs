using System;
using UnityEngine;

namespace Framework
{
	public class DebugLateUpdateScheduler : Scheduler<DebugLateUpdateScheduler>
	{
		public bool IsDebugMode = false;
		void LateUpdate()
		{
			if(IsDebugMode)
			{
				this.OnTick (Time.deltaTime);
			}
		}
	}
}

