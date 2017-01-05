using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
	public class WWWResLoader : BaseResLoader
	{
		public override void Load (Resource res)
		{
			StartCoroutine (LoadWWWResource (res));
		}

		IEnumerator LoadWWWResource(Resource res)
		{
            string url = ResourcePathUtil.GetInResPath(res.path, res.resType, res.storageType);
            CLog.Log("LoadWWWResource:" + url);
            using (WWW www = new WWW (url))
			{
				yield return www;
				res.isDone = true;
				if (www.error.IsEmpty ())
				{
					res.SetWWWObject (www);
				}
				else
				{
					res.errorTxt = www.error;
                    CLog.LogError ("Load resource [" + url + "] fail!");
				}
			}
			OnDone (res);
		}
	}
}

