using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Framework;

namespace Game
{
    public class UnitMoverComponent : UnitComponentBase
    {
        //默认行走速度（左右）
        private static float DefaultHorizontalSpeed = 1f;
        //默认爬行速度
        private static float DefaultVerticalSpeed = 0.5f;
        //默认最大的上升或下降速度大小
        private static float DefaultMaxVerticalSpeed = 6f;
        //默认重力加速度
        private static float DefaultGravity = 9.8f;

        private static float DefaultJumpSpeed = 5f;

//		//人物检测高度数据时的偏移高度（人物中心点的高度加上当前偏移高度才是检测高度）
//		private static float OffsetOnGround = 0.3f;

        protected float _horizontalSpeed;
        protected float _verticalSpeed;
        protected float _gravity;
        protected float _jumpSpeed;
		private Vector2 _oldSpeed;

        private bool _enableGravity;

        public UnitMoverComponent(UnitBase unitBase) : base(unitBase)
        {
        }

        public override UnitComponentType ComponentType
        {
            get
            {
                return UnitComponentType.Mover;
            }
        }

        public override void Init()
        {
			this._unit.PropComponent.InitProperty(UnitProperty.Speed, new Vector2(0f,0f));
            Vector3 pos = _unit.transform.position;
            Vector2 lastPos = new Vector2(pos.x,pos.y);
            this._unit.PropComponent.InitProperty(UnitProperty.LastPos, lastPos);
            this._horizontalSpeed = DefaultHorizontalSpeed;
            this._verticalSpeed = DefaultVerticalSpeed;
            this._gravity = DefaultGravity;
            this._jumpSpeed = DefaultJumpSpeed;
			_oldSpeed = Vector2.zero;

            //默认没有重力，但是在update开始时刷新，（不能在初始化的时候那State组件的状态，可能不准，因为一些顺序问题）
            this._enableGravity = false;
        }

        //改变移动方向
        public void ChangeDirection(float x,float y)
        {
			Vector2 speed = _unit.PropComponent.Speed;
			_oldSpeed.x = speed.x;
			_oldSpeed.y = speed.y;
            if(Mathf.Approximately(x,0))
            {
				speed.x = 0;
            }
            else
            {
				speed.x = Mathf.Sign(x) * DefaultHorizontalSpeed;
            }
            if (Mathf.Approximately(y, 0))
            {
				speed.y = 0f;
            }
            else
            {
				speed.y = Mathf.Sign(y) * DefaultVerticalSpeed;
            }
            if (Mathf.Abs(_oldSpeed.x - speed.x) > 0.0001f || Mathf.Abs(_oldSpeed.y - speed.y) > 0.0001f)
            {
                _unit.PropComponent.UpdateProperty(UnitProperty.Speed, speed);
            }
        }

        public void Jump()
        {
            Vector2 speed = _unit.PropComponent.Speed;
            speed.y = _jumpSpeed;
            _unit.PropComponent.UpdateProperty(UnitProperty.Speed, speed);
        }

		public void EnableGravity(bool enable)
		{
			this._enableGravity = enable;
		    if (!_enableGravity)
		    {
                Vector2 speed = _unit.PropComponent.Speed;
                ChangeDirection(speed.x,0f);
		    }
		}

        public override void Update(float dt)
        {
            if(this._unit != null)
            {
				Vector2 speed = _unit.PropComponent.Speed;
				_oldSpeed.x = speed.x;
				_oldSpeed.y = speed.y;

                if(_enableGravity)
                {
					//更新当前速度的垂直速度
					speed.y = speed.y - _gravity * dt;
					if(Mathf.Abs(speed.y) > DefaultMaxVerticalSpeed)
					{
						speed.y = Mathf.Sign(speed.y) * DefaultMaxVerticalSpeed;
					}
                }

                //更新当前角色的位置
                Vector3 curPos = _unit.transform.position;
                Vector2 lastPos = _unit.PropComponent.LastPos;
                lastPos.x = curPos.x;
                lastPos.y = curPos.y;
                _unit.PropComponent.UpdateProperty(UnitProperty.LastPos, lastPos);
                curPos.x = curPos.x + speed.x * dt;
				curPos.y = curPos.y + speed.y * dt;

				//将速度更新回去
				if ( Mathf.Abs(_oldSpeed.x - speed.x) > 0.0001f || Mathf.Abs(_oldSpeed.y - speed.y) > 0.0001f)
				{
					_unit.PropComponent.UpdateProperty (UnitProperty.Speed, speed);
				}
				//int propState = _unit.PropComponent.PropState;
//				//这个时候状态还来不及改变（将人物直接设置到高度后，检测的时候又变成isGround状态了）
//				bool isGround = UnitStateValue.HasState (propState, UnitStateEnum.Ground);
				//如果是Ground，高度设置成当前地图的高度
//				if (false)
//				{
//					curPos.y = SceneModel.Instance.GetCurMapHeight (curPos.x, curPos.y + OffsetOnGround);
//				}

				//如果下一个位置已经超出了人物可行走的场景位置，那么人物只在场景边缘
				curPos.x = SceneModel.Instance.GetX (curPos.x);
				curPos.y = SceneModel.Instance.GetY (curPos.y);
                _unit.transform.position = curPos;
            }
        }
    }
}
