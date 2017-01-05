using System;
using UnityEngine;
using Framework;

public class FSMTest  : MonoBehaviour
{
	private FiniteStateMachine machine;
	void Start()
	{
		UpdateScheduler.CreateInstance (this.gameObject);
		machine = new FiniteStateMachine ();
		machine.AddState (new TestFiniteState ("StateA",KeyCode.A));
		machine.AddState (new TestFiniteState ("StateB",KeyCode.B));
		machine.AddState (new TestFiniteState ("StateC",KeyCode.C));
		machine.AddState (new TestFiniteState ("StateD",KeyCode.D));

		machine.AddTransition ("StateA", "StateB",new TestFiniteStateCondition());
		machine.AddTransition ("StateB", "StateC",new TestFiniteStateCondition());
		machine.AddTransition ("StateC", "StateD",new TestFiniteStateCondition());
		UpdateScheduler.Instance.AddScheduler (OnTick, 0);
		machine.StartRun ();
//		UpdateScheduler.Instance.AddScheduler (OnDestroy1, 20f);

	}

	private void OnTick(float dt)
	{
		machine.OnTick (dt);
	}

	private void OnDestroy1(float dt)
	{
		machine.OnDestroy ();
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 100, 50), "进入状态"))
		{
			machine.EnterState ("StateA");
		}
		if (GUI.Button (new Rect (100, 0, 100, 50), "暂停状态"))
		{
			machine.Pause ();
		}
		if (GUI.Button (new Rect (200, 0, 100, 50), "回复状态"))
		{
			machine.Resume ();
		}
	}
}

public class TestFiniteStateCondition : IFiniteStateCondition
{
	public void Reset()
	{
	}
	public bool IsInCondition(FiniteState curState,string toStateName)
	{
		TestFiniteState testState = (TestFiniteState)curState;
		if (Input.GetKeyDown (testState.keyCode))
		{
			Debug.Log ("keyDown:" + testState.keyCode);
			return true;
		}
		return false;
	}

	public void Dispose()
	{
	}
}

public class TestFiniteState : FiniteState
{
	public KeyCode keyCode;
	public TestFiniteState (string name,KeyCode keyCode) : base (name)
	{
		this.keyCode = keyCode;
	}

	public override void StateIn ()
	{
		Debug.Log ("StateIn:"+this.StateName);
		UpdateScheduler.Instance.AddScheduler (OnDelayStateIn,2f,1);
	}

	private void OnDelayStateIn(float dt)
	{
		base.StateIn ();
	}

	public override void StateOut ()
	{
		Debug.Log ("StateOut:"+this.StateName);
		UpdateScheduler.Instance.AddScheduler (OnDelayStateOut,3f,1);
	}

	private void OnDelayStateOut(float dt)
	{
		base.StateOut ();
	}

	public override void OnTick (float dt)
	{
		base.OnTick (dt);
	}

	protected override void OnEnter ()
	{
		Debug.Log ("OnEnter:"+this.StateName);
		base.OnEnter ();
	}

	protected override void OnExit ()
	{
		Debug.Log ("OnExit:"+this.StateName);
		base.OnExit ();
	}

	public override void OnPause ()
	{
		Debug.Log ("OnPause:"+this.StateName);
		base.OnPause ();
	}

	public override void OnResume ()
	{
		Debug.Log ("OnResume:"+this.StateName);
		base.OnResume ();
	}

	public override void OnDestroy ()
	{
		Debug.Log ("OnDestroy:"+this.StateName);
		base.OnDestroy ();
	}

}

