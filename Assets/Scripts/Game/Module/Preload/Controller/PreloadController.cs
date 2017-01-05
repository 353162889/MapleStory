using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class PreloadController : BaseController<PreloadController>
	{
		private MultiResourceLoader _multiResourceLoader;
		public override void InitController ()
		{
			_multiResourceLoader = new MultiResourceLoader ();
		}

		public void StartPreload()
		{
			List<string> resList = new List<string> ();
			resList.Add (PreloadConfig.GraphicsMeshMaterial);
			_multiResourceLoader.LoadList (resList,OnComplete);
		}

		private void OnComplete(MultiResourceLoader loader)
		{
			this.DispatchEvent (PreloadEvent.OnPreloadFinish);
		}

		public T GetAsset<T>(string path) where T : UnityEngine.Object
		{
			T prefab = GetPrefab<T> (path);
			return GameObject.Instantiate<T> (prefab);
		}

		public T GetPrefab<T>(string path)where T : UnityEngine.Object
		{
			Resource res = _multiResourceLoader.TryGetRes (path);
			return res.GetAsset<T> ();
		}
	}
}

