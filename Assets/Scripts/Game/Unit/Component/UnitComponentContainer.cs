using System;
using System.Collections.Generic;
using Framework;

namespace Game
{
	public class UnitComponentContainer
	{
		private List<UnitComponentBase> _listComponent;
		private List<UnitComponentBase> _pendingListComponent;
		private bool _isPending;
		public UnitComponentContainer()
		{
			_listComponent = new List<UnitComponentBase> ();
			_pendingListComponent = new List<UnitComponentBase> ();
			_isPending = false;
		}

		//UnitBase注册完组件后统一初始化
		public void Init()
		{
			int count = _listComponent.Count;
			for (int i = 0; i < count; i++)
			{
				_listComponent [i].Init ();
				_listComponent [i].State = UnitComponentState.Doing;
			}
		}

		public bool RegisterComponent(UnitComponentBase component)
		{
			if (HasComponent (component.ComponentType))
				return false;
			if (_isPending)
			{
				component.State = UnitComponentState.ToAdd;
				_pendingListComponent.Add (component);
				return false;
			}
			else
			{
				component.State = UnitComponentState.Doing;
				_listComponent.Add (component);
			}
			return true;
		}

		public UnitComponentBase UnRegisterComponent(UnitComponentType componentType)
		{
			UnitComponentBase component = GetComponent (componentType);
			if (component != null)
			{
				return UnRegisterComponent(component);
			}
			return null;
		}

		public UnitComponentBase UnRegisterComponent(UnitComponentBase component)
		{
			if (_isPending)
			{
				component.State = UnitComponentState.ToRemove;
				_pendingListComponent.Add (component);
				return null;
			}
			else
			{
				return RemoveComponent (component);
			}
		}

		private UnitComponentBase RemoveComponent(UnitComponentBase component)
		{
			for (int i = _listComponent.Count - 1; i >=0 ; i--)
			{
				if (component.ComponentType == component.ComponentType)
				{
					component.State = UnitComponentState.ToRemove;
					_listComponent.RemoveAt (i);
					return component;
				}
			}
			return null;
		}

		private bool HasComponent(UnitComponentType componentType)
		{
			bool result = false;
			for (int i = 0; i < _listComponent.Count; i++)
			{
				if (_listComponent [i].ComponentType == componentType)
				{
					result = true;
					break;
				}
			}
			if (_isPending)
			{
				int count = result ? 1 : 0;
				for (int i = 0; i < _pendingListComponent.Count; i++)
				{
					if (_pendingListComponent [i].ComponentType == componentType)
					{
						if (_pendingListComponent [i].State == UnitComponentState.ToAdd)
						{
							count++;
						}
						else if (_pendingListComponent [i].State == UnitComponentState.ToRemove)
						{
							count--;
						}
					}
				}
				result = count > 0;
			}
			return result;
		}

		public UnitComponentBase GetComponent(UnitComponentType componentType)
		{
			for (int i = 0; i < _listComponent.Count; i++)
			{
				if (_listComponent [i].ComponentType == componentType)
				{
					return _listComponent [i];
				}
			}
			for (int i = 0; i < _pendingListComponent.Count; i++)
			{
				if (_pendingListComponent [i].ComponentType == componentType)
				{
					return _pendingListComponent[i];
				}
			}
			return null;
		}

		public T GetComponent<T>(UnitComponentType componentType) where T : UnitComponentBase
		{
			UnitComponentBase component = GetComponent (componentType);
			if (component != null)
			{
				return (T)component;
			}
			return null;
		}

		public T GetComponent<T>() where T : UnitComponentBase
		{
			for (int i = 0; i < _listComponent.Count; i++)
			{
				if (_listComponent [i] is T)
				{
					return (T)_listComponent [i];
				}
			}
			for (int i = 0; i < _pendingListComponent.Count; i++)
			{
				if (_pendingListComponent [i] is T)
				{
					return (T)_pendingListComponent[i];
				}
			}
			return null;
		}

		public void FixedUpdate(float dt)
		{
			_isPending = true;
			int count = _listComponent.Count;
			for (int i = 0; i < count; i++)
			{
				if (_listComponent [i].State == UnitComponentState.Doing)
				{
					//防止整个游戏卡死
					try
					{
						_listComponent [i].FixedUpdate (dt);
					}
					catch(Exception ex)
					{
						CLog.LogError (ex.Message+"\n"+ex.StackTrace);
					}
				}
			}
			_isPending = false;
		}

		//因为FixedUpdate函数是在Update函数之前调用的，所以统一出移除或添加在update里处理
		public void Update(float dt)
		{
			_isPending = true;
			int count = _listComponent.Count;
			for (int i = 0; i < count; i++)
			{
				//销毁掉的component默认是Init状态，所以也是不会执行的
				if (_listComponent [i].State == UnitComponentState.Doing)
				{
					//防止整个游戏卡死
					try
					{
						_listComponent [i].Update (dt);
					}
					catch(Exception ex)
					{
						CLog.LogError (ex.Message+"\n"+ex.StackTrace);
					}
				}
			}
			_isPending = false;
			for (int i = 0; i < _pendingListComponent.Count; i++)
			{
				UnitComponentBase component = _pendingListComponent[i];
				if (component.State == UnitComponentState.ToAdd)
				{
                    RegisterComponent(component);
                    component.Init();
				}
				else if (component.State == UnitComponentState.ToRemove)
				{
					UnitComponentBase removedComponent = RemoveComponent (component);
					removedComponent.Dispose ();
				}
			}
			_pendingListComponent.Clear ();
		}

		public void Dispose()
		{
			int count = _listComponent.Count;
			for (int i = 0; i < count; i++)
			{
				_listComponent [i].Dispose ();
			}
			for (int i = 0; i < _pendingListComponent.Count; i++)
			{
				_pendingListComponent [i].Dispose ();
			}
			_listComponent.Clear ();
			_pendingListComponent.Clear ();
		}
	}
}

