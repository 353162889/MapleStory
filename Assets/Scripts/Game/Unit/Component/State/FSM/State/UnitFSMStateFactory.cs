using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
	public class UnitFSMStateFactory
	{
		private static Dictionary<string, Type> mapFSMState = new Dictionary<string, Type> {
			{UnitFSMStateName.Ground_Free, 	typeof(UnitFSMGround_Free)},
			{UnitFSMStateName.Float_Free, 	typeof(UnitFSMFloat_Free)},
			{UnitFSMStateName.Climb_Free, 	typeof(UnitFSMClimb_Free)},
		};

		public static UnitFSMStateBase GetState(UnitBase unit, string fsmStateName)
		{
			Type type;
			mapFSMState.TryGetValue (fsmStateName, out type);
			if(type == null)
			{
				CLog.LogError("can not config handler with UnitFSMStateName: " + fsmStateName +" !");
			}
			UnitFSMStateBase fsmState = (UnitFSMStateBase)Activator.CreateInstance (type,new object[]{fsmStateName,unit});
			return fsmState;
		}
	}
}

