using System;
using UnityEngine;

public class ParamsTest : MonoBehaviour
{
	void Start()
	{
		object o = new object[]{ 1,"a"};
		Test (o);
	}

	void Test(params object[] param)
	{
//		Debug.Log (param.Length);
		object[] p = param;
		Test2(p);
	}

	void Test2(params object[] param)
	{
		Debug.Log (param.Length);
	}
}

