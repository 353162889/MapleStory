using System;
using UnityEngine;

namespace Framework
{
	public class UpdateScheduler : Scheduler<UpdateScheduler>
	{
		void Update()
		{
			this.OnTick (Time.deltaTime);
		}
	}
}

