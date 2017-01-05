using System;
using UnityEngine;
using Framework;
using Game;
using System.Collections.Generic;

public class PlayerTest : MonoBehaviour
{
	private UnitPlayer player;
	void Start()
	{
		UpdateScheduler.CreateInstance (this.gameObject);
		FixedUpdateScheduler.CreateInstance (this.gameObject);
		ResourceManager.CreateInstance (this.gameObject);
		List<string> listAvatar = new List<string> (){ 
			"00002000","00012000","00020000","00030000"
		};
		UnitPlayerMO playerMO =  UnitPlayerModel.Instance.CreatePlayerMO (Vector3.zero,listAvatar);
		bool succ = UnitPlayerModel.Instance.AddPlayerMO (playerMO);
		if (succ)
		{
			player = UnitPlayerManager.Instance.AddPlayer (playerMO);
		}

		UnitPlayerAvatar avatar = UnitPlayerAvatar.Create<UnitPlayerAvatar> (listAvatar);
		avatar.transform.position = new Vector3 (100,0,0);
		avatar.gameObject.SetLayerRecursive (5);
		GameObject.Find ("UI").AddChildToParent (avatar.gameObject,"Test_Player");
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 100, 50), "[action]stand2"))
		{
			player.GetUnitComponent<UnitAnimatorComponent> ().PlayAnim ("stand2");
		}
		if (GUI.Button (new Rect (100, 0, 100, 50), "[action]walk2"))
		{
			player.GetUnitComponent<UnitAnimatorComponent> ().PlayAnim ("walk2");
		}
		if (GUI.Button (new Rect (200, 0, 100, 50), "[face]default"))
		{
			player.GetUnitComponent<UnitAnimatorComponent> ().PlayFace ("default");
		}
		if (GUI.Button (new Rect (300, 0, 100, 50), "[face]smile"))
		{
			player.GetUnitComponent<UnitAnimatorComponent> ().PlayFace ("smile");
		}
	}
}

