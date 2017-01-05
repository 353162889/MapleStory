using System;
using UnityEngine;
using Framework;
using System.Collections.Generic;

public class CommandTest : MonoBehaviour
{
	private CommandDynamicSequence dynamicSequence;
	void Start()
	{
//		CommandSequence sequence = new CommandSequence ();
//		Command1 t1 = new Command1 (1);
//		Command1 t2 = new Command1 (2);
//		Command1 t3 = new Command1 (3);
//		Command1 t4 = new Command1 (4);
//		sequence.AddSubCommand (t1);
//		sequence.AddSubCommand (t2);
//		sequence.AddSubCommand (t3);
//		sequence.AddSubCommand (t4);
//		Debug.Log ("StartExecute");
//		t2.OnDone += T2_OnDone;
//		sequence.OnDone += Sequence_OnDone;
//		sequence.Execute ();

		UpdateScheduler.CreateInstance (this.gameObject);
//
//		CommandDynamicSequence dynamicSequence = new CommandDynamicSequence ();
//		int i = 0;
//		UpdateScheduler.Instance.AddScheduler ((float dt) => {
//			i++;
//			dynamicSequence.AddSubCommand (new Command1 (i,UnityEngine.Random.Range(0.1f,0.5f)));
//		}, 1f, 20);


//		CommandDynamicSequence dynamicSequence = new CommandDynamicSequence ();
//		dynamicSequence.AddSubCommand (new Command2 (1,dynamicSequence));

//		CommandDynamicSequence dynamicSequence = new CommandDynamicSequence ();
//		Command3 cmd3 = new Command3 (1,dynamicSequence);
//		cmd3.OnDone += Cmd3_OnDone;
//		dynamicSequence.AddSubCommand (cmd3);

//		dynamicSequence = new CommandDynamicSequence ();
//		for (int i = 0; i < 20; i++)
//		{
//			dynamicSequence.AddSubCommand (new Command4 (i));
//		}

//		LinkedList<int> list = new LinkedList<int> ();
//		list.AddLast (1);
//		LinkedListNode<int> node = list.First;
//		Debug.Log (node.Next);

		CommandImmediately commandImmediately = new CommandImmediately ();
		commandImmediately.On_Done += OnCommandImmediately_OnDone;
		commandImmediately.On_ChildDone += OnCommandImmediately_OnChildDone;
		commandImmediately.AddSubCommand (new Command5 (1));
		commandImmediately.AddSubCommand (new Command5 (2));
		commandImmediately.AddSubCommand (new Command5 (3));
		commandImmediately.AddSubCommand (new Command5 (4));

	}

//	void OnGUI()
//	{
//		if (GUI.Button (new Rect (0, 0, 100, 50), "跳过"))
//		{
//			dynamicSequence.SkipExecuteChild ();
//		}
//	}

	void Cmd3_OnDone (CommandBase obj)
	{
		Command3 cmd = (Command3)obj;
		cmd.sequence.OnDestroy ();
	}

	void OnCommandImmediately_OnDone(CommandBase obj)
	{
		Debug.Log ("OnCommandImmediately_OnDone:"+obj+",state:"+obj.State);
	}

	void OnCommandImmediately_OnChildDone(CommandBase obj)
	{
		Debug.Log ("OnCommandImmediately_OnChildDone:"+ obj+",state:"+obj.State);
	}

//	void T2_OnDone (CommandBase obj)
//	{
//		Debug.Log ("T2_OnDone");
//	}
//
//	void Sequence_OnDone (CommandBase obj)
//	{
//		Debug.Log ("Sequence_OnDone");
//	}
}

public class Command1 : CommandBase
{
	public int index;
	public float time;
	public Command1(int index,float time)
	{
		this.index = index;
		this.time = time;
	}

	public override void Execute (ICommandContext context)
	{
		base.Execute (context);
		Debug.Log ("Execute:"+this.index);
		UpdateScheduler.Instance.AddScheduler (OnDelay, time, 1);
	}

	private void OnDelay(float dt)
	{
		this.OnExecuteDone (CmdExecuteState.Success);
	}

	public override void OnDestroy ()
	{
		base.OnDestroy ();
		Debug.Log ("OnDestroy:"+this.index);
	}

}

public class Command2 : CommandBase
{
	public int index;
	public CommandDynamicSequence sequence;
	public Command2(int index,CommandDynamicSequence sequence)
	{
		this.index = index;
		this.sequence = sequence;
	}

	public override void Execute (ICommandContext context)
	{
		base.Execute (context);
		Debug.Log ("[Command2]Execute:"+this.index);
		if (index < 20)
		{
			sequence.AddSubCommand (new Command2 (this.index + 1, sequence));
		}
		this.OnExecuteDone (CmdExecuteState.Success);
	}

	public override void OnDestroy ()
	{
		sequence = null;
		base.OnDestroy ();
		Debug.Log ("[Command2]OnDestroy:"+this.index);
	}
}

public class Command3 : CommandBase
{
	public int index;
	public CommandDynamicSequence sequence;
	public Command3(int index,CommandDynamicSequence sequence)
	{
		this.index = index;
		this.sequence = sequence;
	}

	public override void Execute (ICommandContext context)
	{
		base.Execute (context);
		Debug.Log ("[Command3]Execute:"+this.index);
		this.OnExecuteDone (CmdExecuteState.Success);
	}

	public override void OnDestroy ()
	{
		sequence = null;
		base.OnDestroy ();
		Debug.Log ("[Command3]OnDestroy:"+this.index);
	}
}

public class Command4 : CommandBase
{
	public int index;
	public Command4(int index)
	{
		this.index = index;
	}

	public override void Execute (ICommandContext context)
	{
		base.Execute (context);
		UpdateScheduler.Instance.AddScheduler (OnDelay, 0.5f, 1);
	}

	private void OnDelay(float dt)
	{
		Debug.Log ("[Command4]Execute:"+this.index);
		this.OnExecuteDone (CmdExecuteState.Success);
	}

	public override void OnDestroy ()
	{
		UpdateScheduler.Instance.RemoveScheduler (OnDelay);
		base.OnDestroy ();
		Debug.Log ("[Command4]OnDestroy:"+this.index);
	}
}

public class Command5 : CommandBase
{
	public int index;
	public Command5(int index)
	{
		this.index = index;
	}

	public override void Execute (ICommandContext context)
	{
		base.Execute (context);
		Debug.Log ("[Command5]Execute:" + this.index);
		UpdateScheduler.Instance.AddScheduler (OnDelay, UnityEngine.Random.Range ((float)index, index + 1), 1);
	}

	private void OnDelay(float dt)
	{
		Debug.Log ("[Command5]ExecuteFinish:"+this.index);
		if (this.index == 3)
		{
			this.OnExecuteDone (CmdExecuteState.Fail);	
		}
		else
		{
			this.OnExecuteDone (CmdExecuteState.Success);
		}
	}

	public override void OnDestroy ()
	{
		UpdateScheduler.Instance.RemoveScheduler (OnDelay);
		base.OnDestroy ();
		Debug.Log ("[Command5]OnDestroy:"+this.index);
	}
}
	

