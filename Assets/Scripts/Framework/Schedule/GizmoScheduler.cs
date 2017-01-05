using System;
using UnityEngine;

namespace Framework
{
	public class GizmoScheduler : Scheduler<GizmoScheduler>
	{
		public bool enableGizmos = false;
		void Start()
		{
		}
		void OnDrawGizmos()
		{
			if (enableGizmos)
			{
				this.OnTick (Time.deltaTime);
			}
		}
	}
}

