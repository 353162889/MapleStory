using System;
using System.Collections.Generic;
using Framework;
using System.Reflection;

namespace Game
{
	public class UnitFSMConditionFactory
	{
		private static Dictionary<UnitFSMConditionType, Type> mapCondition = new Dictionary<UnitFSMConditionType, Type> {
			{UnitFSMConditionType.GroundCheck,typeof(UnitFSMCondGroundCheck)},
            {UnitFSMConditionType.LandingCheck,typeof(UnitFSMCondLandingCheck)},
            {UnitFSMConditionType.RiseCheck,typeof(UnitFSMCondRiseCheck)},
        };

		public static UnitFSMConditionBase GetCondition(UnitBase unit,UnitFSMConditionType conditionType,object[] param)
		{
			Type type;
			mapCondition.TryGetValue (conditionType, out type);
			if(type == null)
			{
				CLog.LogError("can not config handler with UnitFSMConditionType: " + conditionType +" !");
			}
			UnitFSMConditionBase condition = (UnitFSMConditionBase)Activator.CreateInstance (type,new object[]{unit});
			condition.InitParam (param);
			return condition;
		}
	}
}

