using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Game
{
	public class UnitActionAnimator
	{
		public event Action<string> OnActionFinish;
		public string CurAction{get;private set;}
		private List<ActionResScriptObj> _listActionRes;
		private List<ActionResInfo> _curListActionInfo;
		private List<int> _curListActionFrameIndex;
		private Dictionary<string,UnitPartNode> _mapPartNodes;
        private Dictionary<string, int> _mapSpriteOrder;
		private float _curActionDoTime;
		private int _curMainActionPartNameIndex;
		private string _mainActionPartName;
		private bool _isMainActionLastFrameFinish;

		/// <summary>
		/// 动画器
		/// </summary>
		/// <param name="mainActionPart">当前动画器以身体的什么部位为主要帧（用来判断最后一帧）</param>
		public UnitActionAnimator(string mainActionPart)
		{
			this._mainActionPartName = mainActionPart;
			_curActionDoTime = 0;
			_curMainActionPartNameIndex = -1;
			_curListActionInfo = new List<ActionResInfo> ();
			_curListActionFrameIndex = new List<int> ();
			_listActionRes = new List<ActionResScriptObj> ();
		}

		public void Dispose()
		{
			this._listActionRes.Clear();
			this._mapPartNodes = null;
            this._mapSpriteOrder = null;
			this._curListActionInfo.Clear ();
			this._curListActionFrameIndex.Clear ();
			this._curMainActionPartNameIndex = -1;
			this.OnActionFinish = null;
		}

		public void ClearActionRes()
		{
			this._listActionRes.Clear ();
		}

		public void AddActionRes(ActionResScriptObj ActionResScriptObj)
		{
			this._listActionRes.Add (ActionResScriptObj);
		}

        public void UpdateUnitPartNodeMap(Dictionary<string,UnitPartNode> map)
        {
            this._mapPartNodes = map;
        }

        public void UpdateMapSpriteOrder(Dictionary<string,int> map)
        {
            this._mapSpriteOrder = map;
        }

		public void PlayAnim(string actionName = null)
		{
            if(!actionName.IsEmpty())
            { 
                CurAction = actionName;
            }
			if (_listActionRes.Count == 0 || CurAction.IsEmpty ())
			{
				//如果资源没有加载完，直接回调当前动作完成
				if (OnActionFinish != null && !CurAction.IsEmpty())
				{
					OnActionFinish.Invoke (CurAction);
				}
				return;
			}
			_curListActionInfo.Clear ();
			_curListActionFrameIndex.Clear ();
			_curMainActionPartNameIndex = -1;
			_curActionDoTime = 0f;
			for (int i = 0; i < _listActionRes.Count; i++)
			{
				ActionResInfo actionInfo = _listActionRes [i].actionInfos [actionName];
				_curListActionInfo.Add (actionInfo);
				_curListActionFrameIndex.Add (-1);
				//找出主要帧的身体部分
				if (actionInfo.frameInfos.Length > 0 && actionInfo.frameInfos [0].frameResInfos.ContainsKey (_mainActionPartName))
				{
					_curMainActionPartNameIndex = i;
				}
			}
		}
		//导出图的origin是中心点，offset是当前图与父节点的偏移量，spriteResInfo.maps中是节点与当前图原点的偏移量
		public void Update(float dt)
		{
			_isMainActionLastFrameFinish = false;
			if (_curListActionInfo.Count == 0 || _mapPartNodes == null || _mapSpriteOrder == null)
				return;
			for (int i = 0; i < _curListActionInfo.Count; i++)
			{
				ActionResInfo actionResInfo = _curListActionInfo [i];	
				if (actionResInfo.frameInfos != null)
				{
//					float totalFrameTime = 0;
//					for (int j = 0; j < actionResInfo.frameInfos.Length; j++) {
//						totalFrameTime += (actionResInfo.frameInfos [j].delay);
//					}
//					int curFrame = 0;
//					if (totalFrameTime > 0)
//					{
//						float spaceTime = _curActionDoTime % totalFrameTime;
//						float tempTime = 0;
//						for (curFrame = 0; curFrame < actionResInfo.frameInfos.Length; curFrame++)
//						{
//							tempTime += (actionResInfo.frameInfos [curFrame].delay);
//							if (spaceTime < tempTime)
//							{
//								break;
//							}
//						}
//					}
					int curFrame = GetFrame(actionResInfo,_curActionDoTime);
					//预测主动作下一帧是否结束
					if (_curMainActionPartNameIndex == i)
					{
						//如果只有一帧
						if (actionResInfo.frameInfos.Length == 1)
						{
							if (actionResInfo.frameInfos [0].delay > 0)
							{
								float spaceTime = _curActionDoTime % actionResInfo.frameInfos [0].delay;
								if (spaceTime + dt > actionResInfo.frameInfos [0].delay)
								{
									_isMainActionLastFrameFinish = true;
								}
							}
							else
							{
								_isMainActionLastFrameFinish = true;
							}
						}
						else
						{
							int actionFrame = _curListActionFrameIndex [i];
							int actionNextFrame = GetFrame (actionResInfo, _curActionDoTime + dt);
							if (actionFrame != actionNextFrame)
							{
								_isMainActionLastFrameFinish = true;
							}
						}
					}
					if (_curListActionFrameIndex [i] != curFrame)
					{
						_curListActionFrameIndex [i] = curFrame;
						FrameResInfo frameResInfo = actionResInfo.frameInfos [curFrame];
						int spriteCount = frameResInfo.frameResInfos.ListKeys.Count;
						for (int j = 0; j < spriteCount; j++)
						{
							string bodyPartName = frameResInfo.frameResInfos.ListKeys [j];
							SpriteResInfo spriteInfo = frameResInfo.frameResInfos.ListValues [j];
							UnitPartNode playerPartNode;
							_mapPartNodes.TryGetValue (bodyPartName, out playerPartNode);
							if (playerPartNode != null && playerPartNode.render != null)
							{
								playerPartNode.GO.transform.localPosition = new Vector3 (spriteInfo.offset.x,spriteInfo.offset.y,0f);
								playerPartNode.render.sprite = spriteInfo.sprite;
                                int order = 0;
                                _mapSpriteOrder.TryGetValue(spriteInfo.z, out order);
                                if (order == 0)
                                {
                                    CLog.LogError("Sprite order " + spriteInfo.z + " not exist!");
                                }
                                playerPartNode.render.sortingOrder = order;
                            }
							int spriteMapCount = spriteInfo.maps.ListKeys.Count;
							for (int k = 0; k < spriteMapCount; k++)
							{
								string mapName = spriteInfo.maps.ListKeys [k];
								Vector2 mapValue = spriteInfo.maps.ListValues [k];
								UnitPartNode playerMapPartNode;
								_mapPartNodes.TryGetValue (mapName, out playerMapPartNode);
								if (playerMapPartNode != null)
								{
									playerMapPartNode.GO.transform.localPosition = new Vector3 (mapValue.x, mapValue.y, 0f);
								}
							}
						}
					}
				}
			}
			_curActionDoTime += dt;
			if (_isMainActionLastFrameFinish && OnActionFinish != null)
			{
				OnActionFinish.Invoke (CurAction);
			}
		}

		private int GetFrame(ActionResInfo actionResInfo,float actionDoTime)
		{
			float totalFrameTime = 0;
			for (int j = 0; j < actionResInfo.frameInfos.Length; j++) {
				totalFrameTime += (actionResInfo.frameInfos [j].delay);
			}
			int curFrame = 0;
			if (totalFrameTime > 0)
			{
				float spaceTime = actionDoTime % totalFrameTime;
				float tempTime = 0;
				for (curFrame = 0; curFrame < actionResInfo.frameInfos.Length; curFrame++)
				{
					tempTime += (actionResInfo.frameInfos [curFrame].delay);
					if (spaceTime < tempTime)
					{
						break;
					}
				}
			}
			return curFrame;
		}

	}

	
}

