using System;
using UnityEngine;

namespace Framework
{
	public class DebugGizmosScheduler : Scheduler<DebugGizmosScheduler>
	{
		public bool IsDebugMode = false;

		void OnDrawGizmos()
		{
			if (IsDebugMode)
			{
				this.OnTick (Time.deltaTime);
			}
		}
	}
}

