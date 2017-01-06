using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Framework;

namespace Game
{
    
    /// <summary>
    /// 管理unit的状态以及状态机
    /// </summary>
    public class UnitStateComponent : UnitComponentBase
    {
        public int PropState { get { return this._unit.PropComponent.PropState; } }

		protected UnitFSMStateMachine _stateMachine;

        public override UnitComponentType ComponentType
        {
            get
            {
                return UnitComponentType.State;
            }
        }

        public UnitStateComponent(UnitBase unitBase) : base(unitBase)
        {
        }

        public override void Init()
        {
			this._unit.PropComponent.InitProperty(UnitProperty.State, 0);
			_stateMachine = new UnitFSMStateMachine (_unit);
			_stateMachine.InitState (UnitFSMStateName.Float_Free);
        }

        public override void Update(float dt)
        {
			if (_stateMachine != null)
			{
				_stateMachine.OnTick (dt);
			}
        }

		public override void Dispose ()
		{
			if (_stateMachine != null)
			{
				_stateMachine.OnDestroy ();
				_stateMachine = null;
			}
			base.Dispose ();
		}
    }
}
