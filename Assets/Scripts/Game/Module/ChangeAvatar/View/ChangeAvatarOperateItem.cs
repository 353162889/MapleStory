using System;
using UnityEngine;
using Framework;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Game
{
	public class ChangeAvatarOperateItem : MonoBehaviour
	{
		private Button _btnLeft;
		private Button _btnRight;
		private Text _txtDesc;
		private List<string> _listAvatar;
		private int _index;
		private Action _onChange;
		private string _name;
		public void Init (string name, List<string> listAvatar, Action OnChange)
		{
			this._name = name;
			this._listAvatar = listAvatar;
			this._onChange = OnChange;
			_btnLeft = this.gameObject.FindChildComponentRecursive<Button> ("BtnLeft");
			_btnRight = this.gameObject.FindChildComponentRecursive<Button> ("BtnRight");
			_txtDesc = this.gameObject.FindChildComponentRecursive<Text> ("TxtLabel");
			_btnLeft.onClick.AddListener (OnClickLeft);
			_btnRight.onClick.AddListener (OnClickRight);
			UpdateText (0);
		}

		public string GetSelectedValue()
		{
			return _listAvatar[_index];
		}

		private void UpdateText(int index)
		{
			_index = index;
			_txtDesc.text = _listAvatar[_index] + "_" + _name;
		}

		private void OnClickLeft()
		{
			UpdateIndex (-1);
		}

		private void OnClickRight()
		{
			UpdateIndex (1);
		}

		public void UpdateIndex(int offset)
		{
			int old = _index;
			_index+=offset;
			if (_index < 0)
				_index += _listAvatar.Count;
			_index = _index % _listAvatar.Count;
			if (_index != old && _onChange != null)
			{
				_onChange.Invoke ();
			}
			UpdateText (_index);
		}

		public void Dispose()
		{
			OnDestroy ();
		}

		void OnDestroy()
		{
			if (_btnLeft != null)
			{
				_btnLeft.onClick.RemoveAllListeners ();
				_btnLeft = null;
			}
			if (_btnRight != null)
			{
				_btnRight.onClick.RemoveAllListeners ();
				_btnRight = null;
			}
			_onChange = null;
			_listAvatar = null;

		}
	}
}

