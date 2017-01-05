using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class ChangeAvatarViewController : BaseViewController
	{
		public ChangeAvatarViewController(int autoDestroyTime)
			:base(autoDestroyTime)
		{
		}

		protected override List<BaseView> BuildViews ()
		{
			return new List<BaseView>{ 
				new ChangeAvatarView(MainGO,this)
			};
		}

		public override List<string> DependResources ()
		{
			return new List<string>{ "View/ChangeAvatarView.prefab"};
		}

		protected override UnityEngine.GameObject Parent {
			get {
				return GameObject.Find ("UI");
			}
		}
	}
}

