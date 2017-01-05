using System;
using UnityEngine;

namespace Framework
{
	public class FixedUpdateScheduler : Scheduler<FixedUpdateScheduler>
	{
		void FixedUpdate()
		{
			this.OnTick (Time.fixedDeltaTime);
		}
	}
}

