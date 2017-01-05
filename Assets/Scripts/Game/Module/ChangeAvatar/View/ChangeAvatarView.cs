using System;
using UnityEngine;
using Framework;
using System.Collections.Generic;

namespace Game
{
	public class ChangeAvatarView : BaseView
	{
		private static List<string> ListHair = new List<string>{"00030000","00030001" };
		private static List<string> ListHead = new List<string>{ "00012000","00012001"};
		private static List<string> ListBody = new List<string>{ "00002000","00002001"};
		private static List<string> ListFace = new List<string>{ "00020000","00020001"};
		private static List<string> ListAction = new List<string>{ "stand2","walk2","jump"};
		private static List<string> ListFaceAction = new List<string>{"default","smile" };
		private GameObject _playerParent;
		private ChangeAvatarOperateItem _hairOperate;
		private ChangeAvatarOperateItem _headOperate;
		private ChangeAvatarOperateItem _bodyOperate;
		private ChangeAvatarOperateItem _faceOperate;

		private ChangeAvatarOperateItem _actionOperate;
		private ChangeAvatarOperateItem _faceActionOperate;

		private UnitPlayerAvatar _unitPlayer;

		private List<string> _listAvatar;
		public ChangeAvatarView(GameObject go,BaseViewController viewController)
			:base(go,viewController)
		{
		}

		public override void InitUI ()
		{
			_playerParent = MainGO.FindChildRecursive ("PlayerView");
			GameObject hairGO = MainGO.FindChildRecursive ("OperateHair");
			_hairOperate = hairGO.AddComponentOnce<ChangeAvatarOperateItem> ();
			GameObject HeadGO = MainGO.FindChildRecursive ("OperateHead");
			_headOperate = HeadGO.AddComponentOnce<ChangeAvatarOperateItem> ();
			GameObject BodyGO = MainGO.FindChildRecursive ("OperateBody");
			_bodyOperate = BodyGO.AddComponentOnce<ChangeAvatarOperateItem> ();
			GameObject FaceGO = MainGO.FindChildRecursive ("OperateFace");
			_faceOperate = FaceGO.AddComponentOnce<ChangeAvatarOperateItem> ();

			GameObject ActionGO = MainGO.FindChildRecursive ("OperateAction");
			_actionOperate = ActionGO.AddComponentOnce<ChangeAvatarOperateItem> ();

			GameObject FaceActionGO = MainGO.FindChildRecursive ("OperateFaceAction");
			_faceActionOperate = FaceActionGO.AddComponentOnce<ChangeAvatarOperateItem> ();
			_listAvatar = new List<string> ();
			base.InitUI ();
		}

		public override void OnEnterFinish ()
		{
			_hairOperate.Init ("Hair", ListHair, OnRefresh);
			_headOperate.Init ("Head", ListHead, OnRefresh);
			_bodyOperate.Init ("Body", ListBody, OnRefresh);
			_faceOperate.Init ("Face", ListFace, OnRefresh);
			_actionOperate.Init ("Action",ListAction,OnChangeAction);
			_faceActionOperate.Init ("FaceAction", ListFaceAction, OnChangeFaceAction);
			OnRefresh ();
			OnChangeAction ();
			OnChangeFaceAction ();
			base.OnEnterFinish ();
		}

		private void OnRefresh()
		{
			_listAvatar.Clear ();
			_listAvatar.Add (_hairOperate.GetSelectedValue ());
			_listAvatar.Add (_headOperate.GetSelectedValue ());
			_listAvatar.Add (_bodyOperate.GetSelectedValue ());
			_listAvatar.Add (_faceOperate.GetSelectedValue ());

			if (_unitPlayer == null)
			{
				_unitPlayer = UnitPlayerAvatar.Create<UnitPlayerAvatar> (_listAvatar);
				_playerParent.AddChildToParent (_unitPlayer.gameObject, "Player");
                _unitPlayer.transform.localScale = new Vector3(100,100,1);
				_unitPlayer.gameObject.SetLayerRecursive (5);
			}
			else
			{
				UnitAnimatorComponent component = _unitPlayer.GetUnitComponent<UnitAnimatorComponent> ();
				component.RefreshFashions (_listAvatar);
			}
		}

		private void OnChangeAction()
		{
			string action = _actionOperate.GetSelectedValue ();
			UnitPlayAnimCmd cmd = UnitCommandPool.Instance.GetCommand<UnitPlayAnimCmd> (UnitCommandType.PlayAnim);
			cmd.InitData (UnitCommandExecuteType.Immediately, action);
			_unitPlayer.AcceptCommand (cmd);
//			UnitAnimatorComponent component = _unitPlayer.GetUnitComponent<UnitAnimatorComponent> ();
//			component.PlayAnim (action);
		}

		private void OnChangeFaceAction()
		{
			string faceAction = _faceActionOperate.GetSelectedValue ();
			UnitPlayFaceCmd cmd = UnitCommandPool.Instance.GetCommand<UnitPlayFaceCmd> (UnitCommandType.PlayFace);
			cmd.InitData (UnitCommandExecuteType.Immediately, faceAction);
			_unitPlayer.AcceptCommand (cmd);
//			UnitAnimatorComponent component = _unitPlayer.GetUnitComponent<UnitAnimatorComponent> ();
//			component.PlayFace (faceAction);
		}

		public override void OnExitFinish ()
		{
			_hairOperate.Dispose ();
			_headOperate.Dispose ();
			_bodyOperate.Dispose ();
			_faceOperate.Dispose ();
			_actionOperate.Dispose ();
			_faceActionOperate.Dispose ();
			base.OnExitFinish ();
		}
	}
}

