using System;
using Framework;
using UnityEngine;

public class TouchDispatcherTest : MonoBehaviour
{
	void Start()
	{
		TouchDispatcher.CreateInstance (this.gameObject);
		TouchDispatcher.Instance.OnBeginTouchListener += TouchDispatcher_Instance_OnBeginTouchListener;
		TouchDispatcher.Instance.OnMoveTouchListener += TouchDispatcher_Instance_OnMoveTouchListener;
		TouchDispatcher.Instance.OnEndTouchListener += TouchDispatcher_Instance_OnEndTouchListener;
	}

	void TouchDispatcher_Instance_OnEndTouchListener (TouchParam obj)
	{
		Debug.Log ("OnEndTouch:pos="+obj.pos.ToString());
	}

	void TouchDispatcher_Instance_OnMoveTouchListener (TouchParam obj)
	{
		Debug.Log ("OnMoveTouch:pos="+obj.pos.ToString()+",deltaPos="+obj.deltaPos.ToString());
	}

	void TouchDispatcher_Instance_OnBeginTouchListener (TouchParam obj)
	{
		Debug.Log ("OnBeginTouch:pos="+obj.pos.ToString());
	}
}

