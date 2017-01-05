using System;
using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
	public enum ResourceType
	{
		AssetBundle,
		Bytes,
		AudioClip,
		Movie,
		Text,
		Texture,
		DirectObject	//代表暂时未知，为DirectionObj（这种模式表示加载Resource目录下的文件）
	}

	public enum ResourceStorageType
	{
		InResource,		//在Resource目录下
		OutResource,	//在Resource目录外，并不是远程
		Remote,			//远程资源
		UnKnow
	}

	public class Resource
	{
		public string path;
		public ResourceType resType;
		public ResourceStorageType storageType;

		public bool isDone{ get; set;}
		public string errorTxt{get;set;}
		public bool isSucc{ get{ return isDone && errorTxt.IsEmpty();}}

		private UnityEngine.Object _directObj;
		private UnityEngine.Object _wwwAssetObj;
		private string _txt;
		private byte[] _bytes;
		private AssetBundle _assetBundle;

		private List<Resource> _dependRes;

		public int refCount{ get; private set;}

		public Resource()
		{
			isDone = false;
			_directObj = null;
			_txt = null;
			_bytes = null;
			_assetBundle = null;
			refCount = 0;
		}

		public void SetDirectObject(UnityEngine.Object obj)
		{
			this._directObj = obj;
		}

		public void SetWWWObject(WWW www)
		{
			if (resType == ResourceType.AssetBundle)
			{
				_assetBundle = www.assetBundle;
			}
			else if (resType == ResourceType.Text)
			{
				_txt = www.text;
			}
			else if (resType == ResourceType.Bytes)
			{
				_bytes = www.bytes;
			}
			else if (resType == ResourceType.AudioClip)
			{
				_wwwAssetObj = www.audioClip;
			}
			else if (resType == ResourceType.Movie)
			{
				_wwwAssetObj = www.movie;
			}
			else if (resType == ResourceType.Texture)
			{
				_wwwAssetObj = www.texture;
			}
		}

		public string GetText()
		{
			if (resType == ResourceType.Text && !_txt.IsEmpty ())
			{
				return _txt;
			}
			else
			{
				if (_directObj != null && _directObj is TextAsset)
				{
					return ((TextAsset)_directObj).text;
				}
				return null;
			}
		}

		public byte[] GetBytes()
		{
			if (resType == ResourceType.Bytes && _bytes != null)
			{
				return _bytes;
			}
			else
			{
				if (_directObj != null && _directObj is TextAsset)
				{
					return ((TextAsset)_directObj).bytes;
				}
				return null;
			}
		}

		public UnityEngine.Object GetAsset(string name=null)
		{
			UnityEngine.Object asset = null;
			if (resType == ResourceType.AssetBundle)
			{
				if (_assetBundle != null)
				{
					if (name.IsEmpty ())
					{
						asset = _assetBundle.mainAsset;
					}
					else
					{
						string realName = name.ToLower ();
						asset = _assetBundle.LoadAsset (realName);
					}
				}
			}
			else
			{
				if (_wwwAssetObj != null)
				{
					asset = _wwwAssetObj;
				}
				else
				{
					asset = _directObj;
				}
			}
			return asset;
		}

		public T GetAsset<T>(string name = null) where T : UnityEngine.Object
		{
			return GetAsset (name) as T;
		}

		public void SetDependsRes(List<Resource> list)
		{
			this._dependRes = list;
		}

		public void Retain()
		{
			++refCount;
            CLog.Log ("Retain:"+refCount);
		}

		public void Release()
		{
			if (refCount > 0)
			{
				--refCount;
			}
            CLog.Log ("Release:"+refCount);
		}

		public void DestroyResource()
		{
			_txt = null;
			_bytes = null;
			if (_assetBundle != null)
			{
				_assetBundle.Unload (false);
				_assetBundle = null;
			}
			if (_wwwAssetObj != null)
			{
				GameObject.Destroy (_wwwAssetObj);
				_wwwAssetObj = null;
			}
			if (_directObj != null)
			{
				_directObj = null;
			}
			_dependRes = null;
		}
	}
}

