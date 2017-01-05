using System;
using UnityEngine;

namespace Framework
{
	public class LateUpdateScheduler : Scheduler<LateUpdateScheduler>
	{
		void LateUpdate()
		{
			this.OnTick (Time.deltaTime);
		}
	}
}

