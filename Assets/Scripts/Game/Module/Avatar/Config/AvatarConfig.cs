using System;
using Framework;
using System.Collections.Generic;

namespace Game
{
	public class AvatarConfig : BaseConfig<AvatarConfig>
	{
		public Dictionary<string,AvatarCO> _mapAvatar;

		public override void Init ()
		{
			_mapAvatar = new Dictionary<string, AvatarCO> ();
		}

		public AvatarCO GetAvatarCO(string ID)
		{
			AvatarCO avatarCO;
			_mapAvatar.TryGetValue(ID,out avatarCO);
			if (avatarCO == null)
			{
				avatarCO = new AvatarCO ();
				avatarCO.Parse (ID);
				_mapAvatar.Add (avatarCO.ID,avatarCO);
			}
			return avatarCO;
		}
	}
}

