using System;
using Framework;
using UnityEngine;

public class SchedulerTest : MonoBehaviour
{
	private float updateTime = 0;
	private float fixedUpdateTime = 0;
	void Start()
	{
		UpdateScheduler.CreateInstance (this.gameObject);
		FixedUpdateScheduler.CreateInstance (this.gameObject);
		UpdateScheduler.Instance.AddScheduler (OnUpdateTime,0.5f,5);
//		FixedUpdateScheduler.Instance.AddScheduler (OnFixedUpdateTime,1.3f,10);
	}

	private void OnUpdateTime(float dt)
	{
		updateTime += dt;
		Debug.Log ("[OnUpdateTime]Time:"+updateTime+",dt:"+dt);
		if (updateTime > 1f)
		{
			UpdateScheduler.Instance.RemoveScheduler (OnUpdateTime);
		}
	}

	private void OnFixedUpdateTime(float dt)
	{
		fixedUpdateTime += dt;
		Debug.Log ("[OnFixedUpdateTime]Time:"+fixedUpdateTime+",dt:"+dt);
//		if (fixedUpdateTime > 10)
//		{
//			FixedUpdateScheduler.Instance.RemoveScheduler (OnFixedUpdateTime);
//		}

	}
}

