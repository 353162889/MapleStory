using System;
using UnityEngine;
using Framework;

public class PoolTest : MonoBehaviour
{
	void Start()
	{
		ObjectPool<Test1>.Instance.Init (2);
		Test1 t = new Test1 ();
		t.Init (3);
//		Type tt = typeof(Test1);
//		SaveObj<Test1> (t);
//		Debug.Log (ObjectPool<Test1>.Instance.GetObject().i);
	}

	public void SaveObj<T>(T t) where T : TestBase
	{
		ObjectPool<T>.Instance.SaveObject (t);
	}

	public class TestBase : IPoolable
	{
		public int i;
		public void Init(int i)
		{
			this.i = i;
		}

		public void Reset()
		{
		}
	}

	public class Test1 : TestBase
	{
	}
	public class Test2 : TestBase
	{
	}
}

