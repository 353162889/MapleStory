using System;
using UnityEngine;

namespace Framework
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		private static T _instance;

		public static T CreateInstance(GameObject go)
		{
			if (_instance == null)
			{
				_instance = go.AddComponent<T> ();
				GameObject.DontDestroyOnLoad (go);
				_instance.Init ();
			}
			return _instance;
		}

		protected virtual void Init()
		{
		}

		public virtual void OnDestroy()
		{
			if (_instance != null)
			{
				GameObject.Destroy (_instance);
				_instance = null;
			}
		}

		public static T Instance
		{
			get{
				return _instance;
			}
		}
	}
}

