using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
	public class UnitStateRelationHandlerFactory
	{
		private static Dictionary<UnitStateRelationType, UnitStateRelationHandlerBase> mapRelationHandle = new Dictionary<UnitStateRelationType, UnitStateRelationHandlerBase> {
			{UnitStateRelationType.Reject,              new UnitStateRelationReject() },
			{UnitStateRelationType.AddAndRemoveSelf,    new UnitStateRelationAddAndRemoveSelf() }
		};

		public static UnitStateRelationHandlerBase GetHandle(UnitStateRelationType type)
		{
			UnitStateRelationHandlerBase handler;
			mapRelationHandle.TryGetValue(type, out handler);
			if(handler == null)
			{
				CLog.LogError("can not config handler with UnitStateRelationType: " + type +" !");
			}
			return handler;
		}
	}
}

