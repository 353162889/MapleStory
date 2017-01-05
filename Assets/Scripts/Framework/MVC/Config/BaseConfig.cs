using System;

namespace Framework
{
	public class BaseConfig<T> : Singleton<T> where T : BaseConfig<T>,new()
	{
	}
}

