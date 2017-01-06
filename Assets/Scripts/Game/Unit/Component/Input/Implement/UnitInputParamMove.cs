using System;
using Framework;
using UnityEngine;

namespace Game
{
	public class UnitInputParamMove : UnitInputParam
	{
		public float X{ get; private set;}
		public float Y{get;private set;}

		public override UnitInputType InputType {
			get {
				return UnitInputType.Move;
			}
		}

		public void InitData(float x,float y)
		{
			this.X = x;
			this.Y = y;
		}

		public override bool Replaced (UnitInputParam inputParam)
		{
			return inputParam.InputType == this.InputType;
		}

		public override void Reset ()
		{
			this.X = 0;
			this.Y = 0;
		}

	}
}

