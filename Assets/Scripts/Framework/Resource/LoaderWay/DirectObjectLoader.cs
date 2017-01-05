using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
	public class DirectObjectLoader : BaseResLoader
	{
		public override void Load (Resource res)
		{
			StartCoroutine (LoadDirectResource (res));	
		}

		IEnumerator LoadDirectResource(Resource res)
		{
			string loadPath = ResourcePathUtil.GetInResPath(res.path,res.resType,res.storageType);
            CLog.Log("LoadDirectResource:" + loadPath);
			ResourceRequest request = Resources.LoadAsync (loadPath);
			yield return request;
			res.isDone = true;
			if (request.asset == null)
			{
				res.errorTxt = "Load resource [" + loadPath + "] fail!";
                CLog.LogError (res.errorTxt);
			}
			else
			{
				res.SetDirectObject (request.asset);
			}
			OnDone (res);
		}
	}
}

