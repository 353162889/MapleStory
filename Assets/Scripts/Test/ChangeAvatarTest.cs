using System;
using UnityEngine;
using Framework;
using Game;

public class ChangeAvatarTest : MonoBehaviour
{
	ChangeAvatarViewController viewController;
	void Start()
	{
		UpdateScheduler.CreateInstance (this.gameObject);
		FixedUpdateScheduler.CreateInstance (this.gameObject);
		ResourceManager.CreateInstance (this.gameObject);
		viewController = new ChangeAvatarViewController (0);
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (0,0,100,50), "打开"))
		{
			viewController.Open ();
		}
		if (GUI.Button (new Rect (100,0,100,50), "关闭"))
		{
			viewController.Close ();
		}
	}

	void OnDestroy()
	{
		if (viewController != null)
		{
			viewController.Destroy ();
		}
	}
}

