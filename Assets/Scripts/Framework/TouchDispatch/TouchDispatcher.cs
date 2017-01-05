using System;
using UnityEngine;

namespace Framework
{
	public class TouchDispatcher : SingletonMonoBehaviour<TouchDispatcher>
	{
		public event Action<TouchParam> OnBeginTouchListener;
		public event Action<TouchParam> OnMoveTouchListener;
		public event Action<TouchParam> OnEndTouchListener;

		private ITouchDecetor _touchDecetor;
		private TouchParam _touchParam;

		protected override void Init ()
		{
			#if UNITY_EDITOR
			_touchDecetor = new PCTouchDecetor();
			#else
			CLog.LogError("not find touchDecetor");
			#endif
			_touchParam = new TouchParam ();
		}

		//可以在update中更新，将当前脚本执行顺序调前
		void Update()
		{
			if (_touchDecetor != null)
			{
				_touchDecetor.Update ();
			}
		}

		public void OnBeginTouch(Vector2 pos)
		{
			if (OnBeginTouchListener != null)
			{
				_touchParam.Reset ();
				_touchParam.pos = pos;
				OnBeginTouchListener.Invoke (_touchParam);
			}
		}

		public void OnMoveTouch(Vector2 pos,Vector2 deltaPos)
		{
			if (OnMoveTouchListener != null)
			{
				_touchParam.Reset ();
				_touchParam.pos = pos;
				_touchParam.deltaPos = deltaPos;
				OnMoveTouchListener.Invoke (_touchParam);
			}
		}

		public void OnEndTouch(Vector2 pos)
		{
			if (OnEndTouchListener != null)
			{
				_touchParam.Reset ();
				_touchParam.pos = pos;
				OnEndTouchListener.Invoke (_touchParam);
			}
		}
	}
}

