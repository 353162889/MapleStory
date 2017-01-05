using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class KeyBoardDispatcher : SingletonMonoBehaviour<KeyBoardDispatcher>
	{
		public event Action<KeyCode> OnKeyDown;
		public event Action<KeyCode> OnKeyUp;

		private List<KeyCodeEntity> _listKeyCode;
        private List<KeyCodeEntity> _listPending;
        private bool _isPending;
        private ObjectPool<KeyCodeEntity> _pool;
		protected override void Init ()
		{
			_listKeyCode = new List<KeyCodeEntity> ();
            _listPending = new List<KeyCodeEntity>();
            ObjectPool<KeyCodeEntity>.Instance.Init(10);
            _pool = ObjectPool<KeyCodeEntity>.Instance;
            _isPending = false;
        }

		void Update()
		{
            _isPending = true; 
            for (int i = 0; i < _listKeyCode.Count; i++)
            {
                if (_listKeyCode[i].operate == KeyCodeOperate.ToAction)
                {
                    KeyCode keyCode = _listKeyCode[i].keyCode;
                    if (Input.GetKeyDown(keyCode))
                    {
                        if (OnKeyDown != null)
                        {
                            OnKeyDown.Invoke(keyCode);
                        }
                    }
                    if (Input.GetKeyUp(keyCode))
                    {
                        if (OnKeyUp != null)
                        {
                            OnKeyUp.Invoke(keyCode);
                        }
                    }
                }
            }
            _isPending = false;
            for (int i = 0; i < _listPending.Count; i++)
            {
                KeyCodeEntity keyCodeEntity = _listPending[i];
                if (keyCodeEntity.operate == KeyCodeOperate.ToAdd)
                {
                    RegisterRelationKey(keyCodeEntity.keyCode);
                }
                else if(keyCodeEntity.operate == KeyCodeOperate.ToRemove)
                {
                    UnRegisterRelationKey(keyCodeEntity.keyCode);
                }
                _pool.SaveObject(keyCodeEntity);
            }
            _listPending.Clear();
		}

        public void RegisterRelationKey(KeyCode keyCode)
        {
            if (HasKeyCode(keyCode)) return;
            KeyCodeEntity keyCodeEntity = _pool.GetObject();
            if (_isPending)
            {
                keyCodeEntity.Init(KeyCodeOperate.ToAdd, keyCode);
                _listPending.Add(keyCodeEntity);
            }
            else
            {
                keyCodeEntity.Init(KeyCodeOperate.ToAction, keyCode);
                _listKeyCode.Add(keyCodeEntity);
            }
        }

        public void UnRegisterRelationKey(KeyCode keyCode)
        {
            if (_isPending)
            {
                KeyCodeEntity keyCodeEntity = _pool.GetObject();
                keyCodeEntity.Init(KeyCodeOperate.ToRemove, keyCode);
                _listPending.Add(keyCodeEntity);
                for (int i = 0; i < _listKeyCode.Count; i++)
                {
                    if(_listKeyCode[i].keyCode == keyCode)
                    {
                        _listKeyCode[i].operate = KeyCodeOperate.ToRemove;
                    }
                }
            }
            else
            {
                RemoveKey(keyCode);
            }
        }

        private void RemoveKey(KeyCode keyCode)
        {
            for (int i = _listKeyCode.Count - 1; i >= 0; i--)
            {
                KeyCodeEntity keyCodeEntity = _listKeyCode[i];
                if(keyCodeEntity.keyCode == keyCode)
                {
                    _listKeyCode.RemoveAt(i);
                    _pool.SaveObject(keyCodeEntity);
                    break;
                }
            }
        }

        private bool HasKeyCode(KeyCode keyCode)
        {
            bool result =false;
            for (int i = 0; i < _listKeyCode.Count; i++)
            {
                if(_listKeyCode[i].keyCode == keyCode)
                {
                    result = true;
                    break;
                }
            }
            if(_isPending)
            {
                int count = result ? 1 : 0;
                for (int i = 0; i < _listPending.Count; i++)
                {
                    if(_listPending[i].operate == KeyCodeOperate.ToAdd)
                    {
                        count++;
                    }
                    else if(_listPending[i].operate == KeyCodeOperate.ToRemove)
                    {
                        count--;
                    }
                }
                result = count > 0 ? true : false;
            }
            return result;
        }
	}

    public class KeyCodeEntity : IPoolable
    {
        public KeyCodeOperate operate { get; set; }
        public KeyCode keyCode { get; private set; }

        public void Init(KeyCodeOperate operate,KeyCode keyCode)
        {
            this.operate = operate;
            this.keyCode = keyCode;
        }

        public void Reset()
        {
            operate = KeyCodeOperate.Init;
            keyCode = KeyCode.A;
        }
    }

    public enum KeyCodeOperate
    {
        Init,
        ToAdd,
        ToRemove,
        ToAction
    }
}

