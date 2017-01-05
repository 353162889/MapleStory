using UnityEngine;
using System.Collections;
using Framework;
using System.Collections.Generic;

public class EventDispatcherTest: MonoBehaviour 
{
	EventDispatcher eventDispatcher;
	void Start()
	{
		eventDispatcher = new EventDispatcher();
		eventDispatcher.AddListener (1, OnReceive);
		eventDispatcher.DispatchEvent (1,1);
		eventDispatcher.DispatchEvent (1,1);
	}

	private void OnReceive(int eventID,params object[] arg)
	{
		eventDispatcher.RemoveListener (1, OnReceive);
		eventDispatcher.AddListener (1, OnOtherReceive);
		Debug.Log ("[OnReceive]EventID:"+eventID+",param:"+arg.Length);
	}

	private void OnOtherReceive(int eventID,params object[] arg)
	{
		Debug.Log ("[OnOtherReceive]EventID:"+eventID+",param:"+arg.Length);
	}
}

