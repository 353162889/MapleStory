using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
	public class UnitCommandPool : Singleton<UnitCommandPool>
	{
		private Dictionary<UnitCommandType,Queue<UnitCommandBase>> _mapPool;
		private Dictionary<UnitCommandType,int> _mapCapicity;
		private static int DefaultCapicity = 2;
		public override void Init ()
		{
			_mapPool = new Dictionary<UnitCommandType, Queue<UnitCommandBase>>(new UnitCommanTypeComparer());
			_mapCapicity = new Dictionary<UnitCommandType, int>(new UnitCommanTypeComparer()){ 
				{UnitCommandType.PlayAnim,5},
				{UnitCommandType.PlayFace,5},
				{UnitCommandType.Gravity,1}
			};
		}

		public T GetCommand<T>(UnitCommandType type,params object[] param) where T : UnitCommandBase
		{
			Queue<UnitCommandBase> queue = GetQueue (type);
			T obj;
			if (queue.Count > 0)
			{
				obj = (T)queue.Dequeue ();
			}
			else
			{
				obj = (T)Activator.CreateInstance (typeof(T), param);
			}
			return obj;
		}

		public void SaveObject(UnitCommandBase cmd)
		{
			cmd.Reset ();
			Queue<UnitCommandBase> queue = GetQueue (cmd.CmdType);
			int capicity = GetCapicity (cmd.CmdType);
			if (queue.Count < capicity)
			{
				queue.Enqueue (cmd);
			}
			else
			{
				CLog.Log ("<color='yellow'>"+ cmd +" over capicity:"+capicity+"</color>");
			}
		}

		private Queue<UnitCommandBase> GetQueue(UnitCommandType type)
		{
			Queue<UnitCommandBase> queue;
			_mapPool.TryGetValue (type, out queue);
			if (queue == null)
			{
				int capicity = GetCapicity (type);
				queue = new Queue<UnitCommandBase> (capicity);
				_mapPool.Add (type, queue);
			}
			return queue;
		}

		private int GetCapicity(UnitCommandType type)
		{
			int capicity = 0;
			_mapCapicity.TryGetValue (type, out capicity);
			if (capicity == 0)
				capicity = DefaultCapicity;
			return capicity;
		}

		public override void Dispose ()
		{
			_mapPool.Clear ();
			base.Dispose ();
		}

		public class UnitCommanTypeComparer : IEqualityComparer<UnitCommandType>
		{
			public bool Equals (UnitCommandType x, UnitCommandType y)
			{
				return (int)x == (int)y;
			}

			public int GetHashCode (UnitCommandType obj)
			{
				return (int)obj;
			}
		}
	}
}

