using System;
using UnityEngine;

namespace Framework
{
	public class BaseResLoader : MonoBehaviour
	{
		public event Action<Resource> OnResourceDone;

		public virtual void Load(Resource res)
		{
		}

		protected void OnDone(Resource res)
		{
			if (OnResourceDone != null)
			{
				OnResourceDone.Invoke (res);
			}
		}

	}
}

