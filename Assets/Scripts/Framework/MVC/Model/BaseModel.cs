using System;

namespace Framework
{
	public class BaseModel<T> : Singleton<T> where T : BaseModel<T>,new()
	{
		public virtual void InitModel()
		{
		}
	}
}

