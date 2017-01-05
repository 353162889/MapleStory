using System;
using UnityEngine;

namespace Framework
{
	public class TouchParam
	{
		public Vector2 pos = Vector2.zero;
		public Vector2 deltaPos = Vector2.zero;

		public void Reset()
		{
			pos.x = 0;
			pos.y = 0;
			deltaPos.x = 0;
			deltaPos.y = 0;
		}

	}
}

