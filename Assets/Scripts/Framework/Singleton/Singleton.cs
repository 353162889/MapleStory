using System;
using System.Reflection;

namespace Framework
{
	public class Singleton<T> where T : Singleton<T>,new()
	{
		private static T _instance;

		public static T Instance
		{
			get{ 
				if (_instance == null)
				{
					_instance = Activator.CreateInstance<T> ();
					_instance.Init ();
				}
				return _instance;
			}
		}

		public virtual void Init()
		{
		}

		public virtual void Dispose()
		{
			_instance = null;
		}
	}
}

