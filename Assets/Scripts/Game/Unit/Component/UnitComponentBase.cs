using System;

namespace Game
{
	public abstract class UnitComponentBase
	{
		public abstract UnitComponentType ComponentType{ get;}
		protected UnitBase _unit;
		public UnitComponentState State{ get; set;}

		public UnitComponentBase(UnitBase unitBase)
		{
			this._unit = unitBase;
			State = UnitComponentState.Init;
		}

		public virtual void Init()
		{
		}

		public virtual void Dispose()
		{
			State = UnitComponentState.Init;
			_unit = null;
		}

		public virtual void Update(float dt)
		{
		}

		public virtual void FixedUpdate(float dt)
		{
		}
	}

	public enum UnitComponentState
	{
		Init,
		ToAdd,
		ToRemove,
		Doing
	}
}

