using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class BaseView
	{
		public GameObject MainGO{get;private set;}
		protected List<BaseView> listSubView;
		protected BaseViewController viewController;

		public BaseView(GameObject go,BaseViewController viewController)
		{
			this.MainGO = go;
			this.viewController = viewController;
			listSubView = BuildSubView ();
		}

		public virtual List<BaseView> BuildSubView()
		{
			return null;
		}

		public virtual void InitUI()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].InitUI ();
				}
			}
		}

		public virtual void DestroyUI()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].DestroyUI ();
				}
			}
		}

		public virtual void BindEvent()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].BindEvent ();
				}
			}
		}

		public virtual void UnbindEvent()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].UnbindEvent ();
				}
			}
		}

		public virtual void OnEnter(params object[] param)
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].OnEnter (param);
				}
			}
		}

		public virtual void OnEnterFinish()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].OnEnterFinish ();
				}
			}
		}

		public virtual void OnExit()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].OnExit ();
				}
			}
		}

		public virtual void OnExitFinish()
		{
			if (listSubView != null)
			{
				for (int i = 0; i < listSubView.Count; i++)
				{
					listSubView [i].OnExitFinish ();
				}
			}
		}

	}
}

