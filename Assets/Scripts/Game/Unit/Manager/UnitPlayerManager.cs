using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class UnitPlayerManager : Singleton<UnitPlayerManager>
	{
        public UnitPlayer MainPlayer { get; set; }
		private Dictionary<int,UnitPlayer> _mapPlayer;
		private GameObject parent;
		public override void Init ()
		{
			_mapPlayer = new Dictionary<int, UnitPlayer> ();
			parent = new GameObject ();
			parent.name = "Players";
			parent.transform.position = Vector3.zero;
			GameObject.DontDestroyOnLoad (parent);
		}

		public UnitPlayer AddPlayer(UnitPlayerMO playerMO)
		{
			if (!_mapPlayer.ContainsKey (playerMO.ID))
			{
				UnitPlayer player = UnitPlayer.Create<UnitPlayer> (playerMO);
				player.gameObject.name = "Player_"+ player.ID;
				parent.AddChildToParent (player.gameObject);
				_mapPlayer.Add (player.ID, player);
				return player;
			}
			else
			{
				CLog.LogError ("[UnitPlayerManager] exist ID="+playerMO.ID +" player");
				return null;
			}
		}

		public bool RemovePlayer(int id)
		{
			UnitPlayer player = GetPlayer (id);
			if (player != null)
			{
				player.transform.parent = null;
				GameObjectUtil.Destroy (player.gameObject);
			}
			return _mapPlayer.Remove (id);
		}

		public UnitPlayer GetPlayer(int ID)
		{
			UnitPlayer player;
			_mapPlayer.TryGetValue (ID, out player);
			return player;
		}

		public void ClearPlayers()
		{
            MainPlayer = null;
            foreach (var item in _mapPlayer)
			{
				GameObjectUtil.Destroy (item.Value.gameObject);
			}
			_mapPlayer.Clear ();
		}

	}
}

