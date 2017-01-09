using System;
using Framework;
using System.Collections.Generic;

namespace Game
{
    public enum UnitFSMConditionType
	{
		GroundCheck = 0,		//是否在地面的检测
        LandingCheck = 1,       //是否着陆检测
        RiseCheck = 2,          //是否上升检测
	}

	public class UnitFSMConditionParam
	{
		public UnitFSMConditionType Type{get;private set;}
		public object[] Param{ get; private set;}
		public UnitFSMConditionParam(int type,object[] param)
		{
			this.Type = (UnitFSMConditionType)type;
			this.Param = param;
		}
	}

	public class UnitFSMStateMachine
	{
		public UnitBase _unit{ get; private set;}
		private FiniteStateMachine _machine;

		public UnitFSMStateMachine (UnitBase unit)
		{
			this._unit = unit;
			_machine = new FiniteStateMachine ();
			//添加状态
			string[] states = UnitFSMStateName.FSMStateArr;
			for (int i = 0; i < states.Length; i++)
			{
				UnitFSMStateBase fsmState = UnitFSMStateFactory.GetState (_unit,states [i]);
				_machine.AddState (fsmState);
			}

			//添加状态过渡
			UnitFSMConditionParam[,] stateMap = GetFSMMap ();
			int width = stateMap.GetLength (0);
			int height = stateMap.GetLength (1);
			for (int i = 0; i < width; i++) {		//行
				for (int j = 0; j < height; j++)	//列
				{
					UnitFSMConditionParam param = stateMap[i,j];
					if(param != null)
					{
						UnitFSMConditionBase condition = UnitFSMConditionFactory.GetCondition (_unit, param.Type, param.Param);
						string fromState = states [j];
						string toState = states[i];
						_machine.AddTransition (fromState, toState, condition);
					}
				}
			}
		}

		protected virtual UnitFSMConditionParam[,] GetFSMMap()
		{
			//列状态进入行状态,例如第一行第二列是（Float_Free&Ground_Free） null表示没有过度（条件永远不满足）
			return new UnitFSMConditionParam[,]{ 
				//Ground_Free   	Float_Free       	Climb_Free
				{null,    			UFSMCP(1),  	    null}, //Ground_Free
				{UFSMCP(2),         null,     			null}, //Float_Free
				{null,   			null, 				null}, //Climb_Free
			};
		}

		protected UnitFSMConditionParam UFSMCP(int type,params object[] param)
		{
			return new UnitFSMConditionParam (type,param);
		}

		public void OnTick(float dt)
		{
			if (_machine != null)
			{
				_machine.OnTick (dt);
			}
		}

		public void InitState(string stateName)
		{
			_machine.EnterState (stateName);
			_machine.StartRun ();
		}

		public void OnDestroy()
		{
			if (_machine != null)
			{
				_machine.OnDestroy ();
				_machine = null;
			}

			_unit = null;
		}
	}
}

