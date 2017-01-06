using System;
using Framework;

namespace Game
{
	public abstract class UnitInputParam : IPoolable
	{
		public abstract UnitInputType InputType{ get;}

		public virtual bool Replaced(UnitInputParam inputParam)
		{
			return false;
		}

		public virtual void Reset()
		{
		}
	}
}

