using System;
using Framework;

namespace Game
{
    public enum FSMStateInputType
    {
        Move,
        Action
    }

    public enum UnitFSMConditionType
	{
		GroundCheck = 0,		//是否在地面的检测
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
		private FiniteStateMachine _machine;
		private UnitBase _unit;
		public UnitFSMStateMachine (UnitBase unit)
		{
			_unit = unit;
			_machine = new FiniteStateMachine ();
			//添加状态
			string[] states = UnitFSMStateName.FSMStateArr;
			for (int i = 0; i < states.Length; i++)
			{
				UnitFSMStateBase fsmState = UnitFSMStateFactory.GetState (_unit, states [i]);
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
				{null,    			UFSMCP(0,true),  	null}, //Ground_Free
				{UFSMCP(0,false),   null,     			null}, //Float_Free
				{null,   			null, 				null}, //Climb_Free
			};
		}

		protected UnitFSMConditionParam UFSMCP(int type,params object[] param)
		{
			return new UnitFSMConditionParam (type,param);
		}

        public void AcceptInput(FSMStateInputType type,object[] param)
        {
            if(_machine != null && _machine.CurrentState != null)
            {
                ((UnitFSMStateBase)_machine.CurrentState).AcceptInput(type, param);
            }
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

