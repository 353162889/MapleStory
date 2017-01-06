using System;

namespace Game
{
	public class UnitInputParamAction : UnitInputParam
	{
		public string ActionName{get;private set;}
		public bool Effective{ get; private set;}

		public override UnitInputType InputType {
			get {
				return UnitInputType.Action;
			}
		}

		public void InitData(string actionName,bool effective)
		{
			this.ActionName = actionName;
			this.Effective = effective;
		}

		public override bool Replaced (UnitInputParam inputParam)
		{
			if(inputParam.InputType == this.InputType)
			{
				return this.ActionName == ((UnitInputParamAction)inputParam).ActionName;
			}
			return false;
		}

		public override void Reset ()
		{
			this.ActionName = null;
			this.Effective = false;
		}
	}
}

