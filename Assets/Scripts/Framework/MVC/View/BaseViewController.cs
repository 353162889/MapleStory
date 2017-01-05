using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public abstract class BaseViewController
	{
        public event Action<BaseViewController> OnFinishDestroy;
		protected List<BaseView> _listSubView;

		public GameObject MainGO{ get; private set;}

		public ViewState State{get;private set;}

		protected ViewOperateExecutor _executor;

		protected int _autoDestroyTime;

		protected List<Resource> listResources;

		protected abstract GameObject Parent{ get;}

		protected abstract List<BaseView> BuildViews ();

		public abstract List<string> DependResources ();

        /// <summary>
        /// 面板基类
        /// </summary>
        /// <param name="autoDestroyTime">自动销毁时间  关闭界面后   小于0不销毁，等于0立即销毁，大于0等autoDestroyTime时间销毁</param>
        public BaseViewController(int autoDestroyTime)
        {
            //找个时机dispose掉
            _executor = new ViewOperateExecutor(false,false);
            State = ViewState.None;
            this._autoDestroyTime = autoDestroyTime;
        }

        public void UpdateState(ViewState state)
		{
			this.State = state;
			CLog.Log ("[BaseViewController:"+this + "]state:"+State);
		}

		public void Open(params object[] param)
		{
			if (State == ViewState.None)
			{
				CommandInitView initCmd = ViewOperateExecutor.CreateCommand<CommandInitView> ();
				initCmd.Init (this);
				_executor.AddSubCommand (initCmd);
			}
            CommandOpenView openCmd = ViewOperateExecutor.CreateCommand<CommandOpenView>();
            openCmd.Init(this, param);
            _executor.AddSubCommand(openCmd);
        }

		public void Close()
		{
			CommandCloseView cmd = ViewOperateExecutor.CreateCommand<CommandCloseView> ();
			cmd.Init (this);
			_executor.AddSubCommand (cmd);
			if (_autoDestroyTime >= 0)
			{
				Destroy ();
			}
		}

		public void Destroy(bool immediately = false)
		{
			CommandDestroyView cmd = ViewOperateExecutor.CreateCommand<CommandDestroyView> ();
			int autoTime = immediately ? 0 : _autoDestroyTime;
			cmd.Init (this,autoTime);
			cmd.On_Done += OnDestroyDone;
			_executor.AddSubCommand (cmd);
		}

		private void OnDestroyDone(CommandBase command)
		{
            command.On_Done -= OnDestroyDone;
            if(OnFinishDestroy != null)
            {
                OnFinishDestroy.Invoke(this);
                OnFinishDestroy = null;
            }
			ReleaseResources ();
            if(_listSubView != null)
            {
                for (int i = 0; i < _listSubView.Count; i++)
                {
                    _listSubView[i].UnbindEvent();
                }
                for (int i = 0; i < _listSubView.Count; i++)
                {
                    _listSubView[i].DestroyUI();
                }
                _listSubView = null;
            }
            if(MainGO != null)
            {
                GameObjectUtil.Destroy(MainGO);
                MainGO = null;
            }
			_executor.OnDestroy ();
            this.State = ViewState.None;
            CLog.Log("[BaseViewController:"+this+"]OnDestroyDone");
		}

		public Resource GetResource(string name)
		{
			if (listResources != null)
			{
				for (int i = 0; i < listResources.Count; i++)
				{
					if(listResources[i].path == name)return listResources[i];
				}
			}
			return null;
		}

		public void SetResources(List<Resource> res)
		{
			ReleaseResources ();
			listResources = res;
			for (int i = 0; i < listResources.Count; i++)
			{
				listResources [i].Retain ();
			}
		}

		private void ReleaseResources()
		{
			if (listResources != null)
			{
				for (int i = 0; i < listResources.Count; i++)
				{
					listResources [i].Release ();
				}
			}
			listResources = null;
		}

		public void InitUI()
		{
			string mainResPath = DependResources () [0];
			Resource res = GetResource (mainResPath);
			GameObject prefab = res.GetAsset<GameObject> ();
			MainGO = GameObject.Instantiate (prefab);
			Parent.AddChildToParent (MainGO,prefab.name);
			MainGO.SetActive (false);
			_listSubView = BuildViews ();
			if (_listSubView != null)
			{
				for (int i = 0; i < _listSubView.Count; i++)
				{
					_listSubView [i].InitUI ();
				}
				for (int i = 0; i < _listSubView.Count; i++)
				{
					_listSubView [i].BindEvent ();
				}
			}
		}

		public void Enter(params object[] param)
		{
            MainGO.SetActive(true);
			for (int i = 0; i < _listSubView.Count; i++)
			{
				_listSubView [i].OnEnter (param);
			}
		}

		public virtual List<BaseViewAnim> BuildOpenAnims()
		{
			return null;
		}

		public virtual void OnOpenAnimDone()
		{
			for (int i = 0; i < _listSubView.Count; i++)
			{
				_listSubView [i].OnEnterFinish ();
			}
		}

		public void Exit()
		{
			for (int i = 0; i < _listSubView.Count; i++)
			{
				_listSubView [i].OnExit ();
			}
		}

		public virtual List<BaseViewAnim> BuildCloseAnims()
		{
			return null;
		}

		public virtual void OnCloseAnimDone()
		{
			for (int i = 0; i < _listSubView.Count; i++)
			{
				_listSubView [i].OnExitFinish ();
			}
            MainGO.SetActive(false);
        }
	}

	public enum ViewState
	{
		None,
		Initing,
		Opening,
		Open,
		Closing,
		Close,
		Destroying
	}
}

