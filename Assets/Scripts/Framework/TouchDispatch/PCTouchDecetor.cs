using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework
{
	public class PCTouchDecetor : ITouchDecetor
	{
		private static int MouseLeftKey = 0;
		private static string MouseXAxis = "Mouse X";
		private static string MouseYAxis = "Mouse Y";
		private bool _isTouchDown = false;
		public void Update()
		{
			bool isTouchOnUI = (EventSystem.current !=null )? EventSystem.current.IsPointerOverGameObject () : false;
			if (!isTouchOnUI && Input.GetMouseButtonDown(MouseLeftKey))
			{
				Vector2 pos = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);
				TouchDispatcher.Instance.OnBeginTouch (pos);
				_isTouchDown = true;
			}
			if (_isTouchDown && Input.GetMouseButton(MouseLeftKey))
			{
				float xDelta =  Input.GetAxis (MouseXAxis);
				float yDelta = Input.GetAxis (MouseYAxis);
				if (xDelta != 0f || yDelta != 0f)
				{
					Vector2 pos = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);
					Vector2 deltaPos = new Vector2 (xDelta,yDelta);
					TouchDispatcher.Instance.OnMoveTouch (pos, deltaPos);
				}
			}
			if (_isTouchDown && Input.GetMouseButtonUp(MouseLeftKey))
			{
				Vector2 pos = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);
				TouchDispatcher.Instance.OnEndTouch (pos);
				_isTouchDown = false;
			}
		}
	}
}

