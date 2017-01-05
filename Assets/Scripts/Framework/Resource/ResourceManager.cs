using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	
	public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
	{
		public delegate void ResourceHandler(Resource res);

		private Dictionary<Resource,List<ResourceHandler>> _succCallbacks;
		private Dictionary<Resource,List<ResourceHandler>> _failCallbacks;
		private Dictionary<string,Resource> _mapRes;

		private ResourceLoader _resLoader;

		protected override void Init ()
		{
			_succCallbacks = new Dictionary<Resource, List<ResourceHandler>> ();
			_failCallbacks = new Dictionary<Resource, List<ResourceHandler>> ();
			_mapRes = new Dictionary<string, Resource>();
			_resLoader = ResourceLoader.GetResLoader (this.gameObject);
			_resLoader.OnDone += OnResourceDone;
		}

        public void ReleaseUnUseRes()
        {
            List<Resource> list = new List<Resource>();
            foreach (var item in _mapRes)
            {
                if(item.Value.refCount <= 0)
                {
                    list.Add(item.Value);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                _mapRes.Remove(list[i].path);
                list[i].DestroyResource();
            }
            GC.Collect();
        }

		public Resource GetResource(string path,ResourceHandler onSucc = null,ResourceHandler onFail = null,
			ResourceType resType = ResourceType.DirectObject,ResourceStorageType storageType = ResourceStorageType.UnKnow)
		{
			if (path.IsEmpty ())
			{
                CLog.LogError ("[GetResource]ResName can not is null!");
				return null;
			}
			Resource res;
			_mapRes.TryGetValue (path, out res);
			if (res != null)
			{
				if (res.isDone)
				{
					if (res.isSucc && onSucc != null)
					{
						ResourceHandler tempOnSucc = onSucc;
						onSucc = null;
						tempOnSucc.Invoke (res);
						tempOnSucc = null;
					}
				}
				else
				{
					AddListener (res, onSucc, onFail);
				}
				return res;
			}
			res = new Resource ();
			res.path = path;
			res.resType = resType;
			res.storageType = storageType;

			//获取到当前资源的依赖资源(可能没法保证顺序，所以拿的时候需要保证所有依赖资源都已经加载好)
			List<string> listDependResPath = GetDependResPath();
			if (listDependResPath != null && listDependResPath.Count > 0)
			{
				List<Resource> listDependRes = new List<Resource> ();
				for (int i = 0; i < listDependResPath.Count; i++)
				{
					//加载依赖资源
					Resource dependRes = GetResource(listDependResPath[i]);
					listDependRes.Add (dependRes);
				}
				res.SetDependsRes (listDependRes);
			}

			//真正加载当前资源
			_mapRes.Add(res.path,res);
			res.Retain ();
			AddListener(res,onSucc,onFail);
			_resLoader.Load(res);
			return res;	
		}

		private void OnResourceDone (Resource res)
		{
			if (res.isSucc)
			{
				List<ResourceHandler> succList;
				_succCallbacks.TryGetValue (res, out succList);
				_succCallbacks.Remove (res);
				if (succList != null)
				{
					for (int i = 0; i < succList.Count; i++)
					{
						succList [i].Invoke (res);
					}
					succList.Clear ();
				}
				res.Release ();
			}
			else
			{
				//失败的话，将资源先移除掉
				bool removeSucc = _mapRes.Remove (res.path);
				List<ResourceHandler> failList;
				_failCallbacks.TryGetValue (res, out failList);
				_failCallbacks.Remove (res);
				if (failList != null)
				{
					for (int i = 0; i < failList.Count; i++)
					{
						failList [i].Invoke (res);
					}
					failList.Clear ();
				}
				if(removeSucc)
				{
					res.Release ();
					res.DestroyResource ();
				}
			}
		}

		private List<string> GetDependResPath()
		{
			return null;
		}

		private void AddListener(Resource res,ResourceHandler onSucc = null,ResourceHandler onFail = null)
		{
			
			if (onSucc != null)
			{
				List<ResourceHandler> succList;
				_succCallbacks.TryGetValue(res,out succList);
				if (succList == null)
				{
					succList = new List<ResourceHandler> ();
					_succCallbacks.Add (res, succList);
				}
				if (!succList.Contains (onSucc))
				{
					succList.Add (onSucc);
				}
			}
			if (onFail != null)
			{
				List<ResourceHandler> failList;
				_failCallbacks.TryGetValue(res,out failList);
				if (failList == null)
				{
					failList = new List<ResourceHandler> ();
					_failCallbacks.Add (res, failList);
				}
				if (!failList.Contains (onFail))
				{
					failList.Add (onFail);
				}
			}
		}

		public void RemoveListener(Resource res,ResourceHandler onSucc = null,ResourceHandler onFail = null)
		{
			if (onSucc != null)
			{
				List<ResourceHandler> succList;
				_succCallbacks.TryGetValue (res, out succList);
				if (succList != null)
				{
					succList.Remove (onSucc);
				}
			}
			if (onFail != null)
			{
				List<ResourceHandler> failList;
				_failCallbacks.TryGetValue (res, out failList);
				if (failList != null)
				{
					failList.Remove (onFail);
				}
			}
		}

		public void RemoveListener(string path,ResourceHandler onSucc = null,ResourceHandler onFail = null)
		{
			Resource res;
			_mapRes.TryGetValue (path, out res);
			if (res != null)
			{
				RemoveListener (res,onSucc,onFail);
			}
		}

		public void RemoveAllListener(Resource res)
		{
			_succCallbacks.Remove (res);
			_failCallbacks.Remove (res);
		}

		public void RemoveAllListener(string path)
		{
			Resource res;
			_mapRes.TryGetValue (path, out res);
			if (res != null)
			{
				RemoveAllListener (res);
			}
		}

		public void RemoveWaitLoadingRes(Resource res)
		{
			if(_resLoader.RemoveWaitingLoadingRes (res))
			{
				if (_mapRes.Remove (res.path))
				{
					res.Release ();
				}
			}
		}

		public override void OnDestroy ()
		{
			_resLoader.OnDone -= OnResourceDone;
			base.OnDestroy ();
		}
	}
}

