using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
	public class UnitInputParamPool : Singleton<UnitInputParamPool>
	{
		private Dictionary<UnitInputType,Queue<UnitInputParam>> _mapPool;
		private Dictionary<UnitInputType,int> _mapCapicity;
		private static int DefaultCapicity = 2;
		public override void Init ()
		{
			_mapPool = new Dictionary<UnitInputType, Queue<UnitInputParam>>(new UnitInputTypeComparer());
			_mapCapicity = new Dictionary<UnitInputType, int>(new UnitInputTypeComparer()){ 
				{UnitInputType.Move,10},
				{UnitInputType.Action,10},
			};
		}

		public T GetInputParam<T>(UnitInputType type,params object[] param) where T : UnitInputParam
		{
			Queue<UnitInputParam> queue = GetQueue (type);
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

		public void SaveObject(UnitInputParam input)
		{
			input.Reset ();
			Queue<UnitInputParam> queue = GetQueue (input.InputType);
			int capicity = GetCapicity (input.InputType);
			if (queue.Count < capicity)
			{
				queue.Enqueue (input);
			}
			else
			{
				CLog.Log ("<color='yellow'>"+ input +" over capicity:"+capicity+"</color>");
			}
		}

		private Queue<UnitInputParam> GetQueue(UnitInputType type)
		{
			Queue<UnitInputParam> queue;
			_mapPool.TryGetValue (type, out queue);
			if (queue == null)
			{
				int capicity = GetCapicity (type);
				queue = new Queue<UnitInputParam> (capicity);
				_mapPool.Add (type, queue);
			}
			return queue;
		}

		private int GetCapicity(UnitInputType type)
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

		public class UnitInputTypeComparer : IEqualityComparer<UnitInputType>
		{
			public bool Equals (UnitInputType x, UnitInputType y)
			{
				return (int)x == (int)y;
			}

			public int GetHashCode (UnitInputType obj)
			{
				return (int)obj;
			}
		}
	}
}

