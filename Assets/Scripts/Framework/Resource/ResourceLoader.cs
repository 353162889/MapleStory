using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
	public class ResourceLoader : MonoBehaviour
	{
		private static int MaxLoaderCount = 3;

		public event Action<Resource> OnDone;
		private DirectObjectLoader _directLoader;
		private WWWResLoader _wwwLoader;
		private LinkedList<Resource> _waitingList;
		private LinkedList<Resource> _loadingList;

		public static ResourceLoader GetResLoader(GameObject go)
		{
			ResourceLoader loader = go.AddComponentOnce<ResourceLoader> ();
			loader.Init ();
			return loader;
		}

		protected void Init()
		{
			_directLoader = this.gameObject.AddComponentOnce<DirectObjectLoader> ();
			_wwwLoader = this.gameObject.AddComponentOnce<WWWResLoader> ();
			_directLoader.OnResourceDone += OnResourceDone;
			_wwwLoader.OnResourceDone += OnResourceDone;

			_waitingList = new LinkedList<Resource> ();
			_loadingList = new LinkedList<Resource> ();
		}

		private void OnResourceDone(Resource res)
		{
			bool succ = _loadingList.Remove (res);
			if (succ)
			{
				if (OnDone != null)
				{
					OnDone.Invoke (res);
				}
			}
			LoadNext ();
		}

		public void Load(Resource res)
		{
			if (_waitingList.Contains (res) || _loadingList.Contains (res))
				return;
			_waitingList.AddLast (res);
			LoadNext ();
		}

		public bool RemoveWaitingLoadingRes(Resource res)
		{
			return _waitingList.Remove (res);
		}

		private void LoadNext()
		{
			while (_loadingList.Count < MaxLoaderCount && _waitingList.Count > 0)
			{
				Resource loadingRes = _waitingList.First.Value;
				_waitingList.RemoveFirst ();
				_loadingList.AddLast (loadingRes);
				if (loadingRes.resType == ResourceType.DirectObject)
				{
					_directLoader.Load (loadingRes);
				}
				else
				{
					_wwwLoader.Load (loadingRes);
				}
			}
		}

		void OnDestroy()
		{
			_directLoader.OnResourceDone -= OnResourceDone;
			_wwwLoader.OnResourceDone -= OnResourceDone;
		}
	}
}

