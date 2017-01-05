using System;

namespace Game
{
	public class UnitPlayerAnimatorComponent : UnitAnimatorComponent
	{
		public UnitPlayerAnimatorComponent (UnitBase unit)
			:base(unit)
		{
		}

		public override UnitComponentType ComponentType {
			get {
				return UnitComponentType.Animator;
			}
		}
	}
}

