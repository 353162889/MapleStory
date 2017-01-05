using System;
using UnityEngine;
using Framework;

namespace Game
{
	public class UnitBase : MonoBehaviour
	{
		public int ID{ get; protected set;}

        private UnitPropertyComponent _propComponent;
        public UnitPropertyComponent PropComponent { get {
                if(_propComponent == null)
                {
                    _propComponent = (UnitPropertyComponent)GetUnitComponent(UnitComponentType.Property);
                }
                return _propComponent;
            } }

		public static T Create<T>(params object[] param) where T : UnitBase
		{
			GameObject go = new GameObject ();
			T t = go.AddComponentOnce<T> ();
			t.Init (param);
			return t;
		}

		protected UnitComponentContainer _componentContainer;
		protected UnitCommandExecutor _commandExecutor;
        protected EventDispatcher _eventDispatcher;

        private void Init(params object[] param)
		{
			_componentContainer = new UnitComponentContainer ();
			_commandExecutor = new UnitCommandExecutor ();
            _eventDispatcher = new EventDispatcher();
			InitData (param);
			InitComponent ();
			_componentContainer.Init ();
			InitFinish ();
			if (this.gameObject.activeSelf)
			{
				UpdateScheduler.Instance.AddScheduler (OnUpdate, 0);
				FixedUpdateScheduler.Instance.AddScheduler (OnFixedUpdate, 0);
			}
		}

		void OnEnable()
		{
			UpdateScheduler.Instance.AddScheduler (OnUpdate, 0);
			FixedUpdateScheduler.Instance.AddScheduler (OnFixedUpdate, 0);
		}

		void OnDisable()
		{
			if (UpdateScheduler.Instance != null)
			{
				UpdateScheduler.Instance.RemoveScheduler (OnUpdate);
			}
			if (FixedUpdateScheduler.Instance != null)
			{
				FixedUpdateScheduler.Instance.RemoveScheduler (OnFixedUpdate);
			}
		}

		protected virtual void InitData(params object[] param)
		{
		}

		protected virtual void InitComponent()
		{
            //这个每个都是需要出事化
            _componentContainer.RegisterComponent(new UnitPropertyComponent(this));
		}

		protected virtual void InitFinish()
		{
		}

        public void AddListener(int eventID, EventDispatchHandler handler)
        {
            _eventDispatcher.AddListener(eventID, handler);
        }

        public void RemoveListener(int eventID, EventDispatchHandler handler)
        {
            _eventDispatcher.RemoveListener(eventID, handler);
        }

        public void DispatchEvent(int eventID, params object[] param)
        {
            _eventDispatcher.DispatchEvent(eventID, param);
        }

        public void AcceptCommand(UnitCommandBase command)
		{
			command.BindUnit (this);
			_commandExecutor.ExecuteCommand (command);
		}

		public void RegisterComponent(UnitComponentBase component)
		{
			if (_componentContainer.RegisterComponent (component))
			{
				component.Init ();
                this.DispatchEvent(UnitComponentEvent.OnComponentRegister,component.ComponentType);
			}
		}

		public void UnRegisterComponent(UnitComponentType componentType)
		{
			UnitComponentBase component = _componentContainer.UnRegisterComponent (componentType);	
			if (component != null)
			{
				component.Dispose ();
                this.DispatchEvent(UnitComponentEvent.OnComponentUnRegister,componentType);
			}
		}

		public UnitComponentBase GetUnitComponent(UnitComponentType componentType)
		{
			return _componentContainer.GetComponent (componentType);
		}

		public T GetUnitComponent<T>(UnitComponentType componentType) where T : UnitComponentBase
		{
			UnitComponentBase component = GetUnitComponent (componentType);
			if (component != null)
			{
				return (T)component;
			}
			return null;
		}

		public T GetUnitComponent<T>() where T : UnitComponentBase
		{
			return _componentContainer.GetComponent<T> ();
		}

		protected virtual void OnUpdate(float dt)
		{
			_componentContainer.Update (dt);
		}

		protected virtual void OnFixedUpdate(float dt)
		{
			_componentContainer.FixedUpdate (dt);
		}

		//有可能对象都没有调用awake方法，所以，在移除时需要手动调用
		public virtual void Dispose()
		{
			if (UpdateScheduler.Instance != null)
			{
				UpdateScheduler.Instance.RemoveScheduler (OnUpdate);
			}
			if (FixedUpdateScheduler.Instance != null)
			{
				FixedUpdateScheduler.Instance.RemoveScheduler (OnFixedUpdate);
			}
			if (_componentContainer != null)
			{
				_componentContainer.Dispose ();
				_componentContainer = null;
			}
			if (_commandExecutor != null)
			{
				_commandExecutor.Dispose ();
				_commandExecutor = null;
			}
            if(_eventDispatcher != null)
            {
                _eventDispatcher.Dispose();
                _eventDispatcher = null;
            }
		}

		void OnDestroy()
		{
			//当前对象销毁时，默认调用dispose方法
			Dispose ();
		}
	}
}

