using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class UnitFSMStateBase : FiniteState
    {
        //人物检测高度数据时的偏移高度（人物中心点的高度加上当前偏移高度才是检测高度）
        private static float OffsetOnGround = 0.3f;

        protected UnitBase _unit;
		public UnitFSMStateBase (string stateName,UnitBase unit)
			:base(stateName)
		{
			this._unit = unit;
		}

		public void OnAcceptInput(int type,params object[] arg)
        {
			OnInput((UnitInputParam)arg[0]);
        }

		/// <summary>
		/// 关注在进入改状态时，有没有其额外的输入（例如移动）
		/// </summary>
		/// <param name="type">Type.</param>
		public void CheckCacheInputInEnter(UnitInputType type)
		{
			UnitInputComponent inputComponent = (UnitInputComponent)_unit.GetUnitComponent (UnitComponentType.Input);
			if (inputComponent != null)
			{
				List<UnitInputParam> listParam = inputComponent.GetCacheInput (type);
				if (listParam != null)
				{
					for (int i = 0; i < listParam.Count; i++)
					{
						OnInput (listParam [i]);
					}
				}
			}
		}

		protected virtual void OnInput(UnitInputParam inputParam)
        {
        }

        protected override void OnEnter ()
        {
            CLog.Log("EnterState:"+this.StateName);
			int beforeState = _unit.PropComponent.PropState;
			int afterState = UnitFSMStateName.GetPropState (this.StateName);
			this._unit.PropComponent.UpdateProperty (UnitProperty.State, afterState);
			if (beforeState != afterState)
			{
				this._unit.DispatchEvent(UnitStateEvent.OnStateChange, beforeState, afterState);
			}
			this._unit.AddListener (UnitInputEvent.OnAcceptInput, OnAcceptInput);
			base.OnEnter ();
		}

	    protected override void OnExit ()
		{
			this._unit.RemoveListener (UnitInputEvent.OnAcceptInput, OnAcceptInput);
			base.OnExit ();
		}

		protected void EnableGravity(bool enable)
		{
			UnitGravityCmd cmd = UnitCommandPool.Instance.GetCommand<UnitGravityCmd> (UnitCommandType.Gravity);
			cmd.InitData (UnitCommandExecuteType.Immediately, enable);
			_unit.AcceptCommand (cmd);
		}

		/// <summary>
		/// 做动作，这里现在只播动画
		/// </summary>
		/// <param name="actionName">Action name.</param>
		protected void DoAction(string actionName)
		{
			UnitPlayAnimCmd cmd = UnitCommandPool.Instance.GetCommand<UnitPlayAnimCmd> (UnitCommandType.PlayAnim);
			cmd.InitData (UnitCommandExecuteType.Immediately, actionName);
			_unit.AcceptCommand (cmd);
		}

		protected void ChangeFace(UnitFace face)
		{
			UnitFace curFace = _unit.PropComponent.UnitFace;
			if (curFace != face)
			{
				UnitChangeFaceCmd cmd = UnitCommandPool.Instance.GetCommand<UnitChangeFaceCmd> (UnitCommandType.ChangeFace);
				cmd.InitData (UnitCommandExecuteType.Immediately, face);
				_unit.AcceptCommand (cmd);
			}
		}

		protected void MoveDirection(float x,float y)
		{
			UnitMoveDirectionCmd cmd = UnitCommandPool.Instance.GetCommand<UnitMoveDirectionCmd> (UnitCommandType.MoveDirection);
			cmd.InitData (UnitCommandExecuteType.Immediately,x,y);
			_unit.AcceptCommand (cmd);
		}

		protected void Jump()
		{
			UnitJumpCmd cmd = UnitCommandPool.Instance.GetCommand<UnitJumpCmd> (UnitCommandType.Jump);
			cmd.InitData (UnitCommandExecuteType.Immediately);
			_unit.AcceptCommand (cmd);
		}

		protected void UpdateMapHeight()
		{
			Vector3 curPos = _unit.transform.position;
			curPos.y = SceneModel.Instance.GetCurMapHeight (curPos.x, curPos.y + OffsetOnGround);
			_unit.transform.position = curPos;
		}

		public override void OnDestroy ()
		{
			if (_unit != null)
			{
				this._unit.RemoveListener (UnitInputEvent.OnAcceptInput, OnAcceptInput);
			}
			this._unit = null;
			base.OnDestroy ();
		}
	}
}

