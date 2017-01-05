using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class UnitPlayerModel : Singleton<UnitPlayerModel>
	{
		private Dictionary<int,UnitPlayerMO> _mapPlayerMO;
		private int _id;
		public override void Init ()
		{
			_id = 0;
			_mapPlayerMO = new Dictionary<int, UnitPlayerMO> ();
		}

		public bool AddPlayerMO(UnitPlayerMO playerMO)
		{
			if (!_mapPlayerMO.ContainsKey (playerMO.ID))
			{
				_mapPlayerMO.Add (playerMO.ID, playerMO);
				return true;
			}
			return false;
		}

		public bool RemovePlayerMO(UnitPlayerMO playerMO)
		{
			return _mapPlayerMO.Remove (playerMO.ID);
		}

		public void ClearPlayers()
		{
			_mapPlayerMO.Clear ();
		}

		public UnitPlayerMO CreatePlayerMO(Vector3 pos,List<string> listAvatar)
		{
			UnitPlayerMO playerMO = new UnitPlayerMO ();
			playerMO.ID = _id++;
			playerMO.DefineId = 0;
			playerMO.InitPos = pos;
			playerMO.listAvatar = listAvatar;
			return playerMO;
		}
	}
}

