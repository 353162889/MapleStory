using System;
using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// 具有动画数据的对象才能有这个
	/// </summary>
	public class UnitAnimatorComponent : UnitComponentBase
	{
        private static string RootNodeName = "root";
        private static Dictionary<string, string> PlayerPartTreeMap = new Dictionary<string, string>{
            {"hand","map_navel"},
            {"arm","map_navel"},
            {"body",RootNodeName},
            {"map_navel","body"},
            {"map_neck","body"},
            {"head","map_neck"},
            {"ear","map_neck"},
            {"face","map_brow"},
            { "map_brow","head"},
            //{ "map_earOverHead","head" },
            //{ "map_earBelowHead","head"},
            { "hair","map_brow"}
        };

        private static Dictionary<string, int> SpriteOrderMap = new Dictionary<string, int>{
            {"body",1001},
            {"arm",1002},
            {"backAccessoryOverHead",1003},
            {"head",1004},
            {"accessoryOverHair",1005},
            {"handOverHair",1006},
            { "face",1007},
            { "hair",1008},
            { "armOverHair",1009}
        };
        private static string DefaultAction = "stand2";
        private static string DefalutFace = "smile";
        
		private MultiResourceLoader _resLoader;
        private Dictionary<string, UnitPartNode> _mapPartNodes;

        private UnitActionAnimator _actionAnimator;
        private UnitActionAnimator _faceAnimator;

		private List<string> _listAvatar;

		private Action<bool> _actionFinish;
		private Action<bool> _faceFinish;

        private UnitFace _unitFace;

		public override UnitComponentType ComponentType {
			get {
				return UnitComponentType.Animator;
			}
		}

		public UnitAnimatorComponent(UnitBase unitBase)
			:base(unitBase)
		{
		}

		public override void Init ()
		{
            _mapPartNodes = new Dictionary<string, UnitPartNode>();
            _actionAnimator = new UnitActionAnimator("body");
            _faceAnimator = new UnitActionAnimator("face");
            this._unit.PropComponent.InitProperty(UnitProperty.ActionName, _actionAnimator.CurAction);
            this._unit.PropComponent.InitProperty(UnitProperty.FaceName, _faceAnimator.CurAction);
            this._unit.PropComponent.InitProperty(UnitProperty.UnitFace, UnitFace.Right);
			_actionAnimator.OnActionFinish += _actionAnimator_OnActionFinish;
			_faceAnimator.OnActionFinish += _faceAnimator_OnActionFinish;
            _resLoader = new MultiResourceLoader ();
		}

		void _faceAnimator_OnActionFinish (string actionName)
		{
			if (_actionFinish != null)
			{
				Action<bool> temp = _actionFinish;
				_actionFinish = null;
				temp.Invoke (true);
			}
		}

		void _actionAnimator_OnActionFinish (string actionName)
		{
			if (_faceFinish != null)
			{
				Action<bool> temp = _faceFinish;
				_faceFinish = null;
				temp.Invoke (true);
			}
		}

		public void RefreshFashions(List<string> listAvatar)
		{
			if (listAvatar == null || listAvatar.Count == 0)
				return;
			_listAvatar = listAvatar;
			_resLoader.Clear();
            List<string> resPaths = new List<string>();
			for (int i = 0; i < _listAvatar.Count; i++)
            {
				AvatarCO avatarCO = AvatarConfig.Instance.GetAvatarCO(_listAvatar[i]);
				resPaths.Add(avatarCO.ResPath);
            }
			_resLoader.LoadList (resPaths, OnComplete);
		}


        /// <summary>
        /// 改变unit的面向
        /// </summary>
        /// <param name="face"></param>
        public void ChangeUnitFace(UnitFace face)
        {
            this._unit.PropComponent.UpdateProperty(UnitProperty.UnitFace, face);
            UpdateUnitFace(face);
        }

        private void UpdateUnitFace(UnitFace face)
        {
            UnitPartNode node;
            _mapPartNodes.TryGetValue(RootNodeName, out node);
            if (node != null)
            {
                node.GO.transform.localScale = new Vector3((int)face, 1f, 1f);
            }
        }

		/// <summary>
		/// 播放动作
		/// </summary>
		/// <param name="faceName">动作名</param>
		/// <param name="OnFaceFinish">动作是否播完，如果播完了，会回调OnFaceFinish函数，bool参数是 如果正常播完，为true，如果是播放其他动作打断的，那么为false</param>
		public void PlayAnim(string actionName,Action<bool> OnActionFinish = null)
        {
			if (_actionAnimator != null)
			{
				if (_actionFinish != null)
				{
					Action<bool> temp = _actionFinish;
					_actionFinish = null;
					temp.Invoke (false);
				}
				_actionFinish = OnActionFinish;
                PlayAnimInner(actionName);
			}
			else
			{
				CLog.Log ("UnitAnimatorComponent play anim fail,because _actionAnimator is null",CLogColor.Yellow);
			}
        }

        private void PlayAnimInner(string actionName)
        {
            if(_actionAnimator != null)
            {
                _actionAnimator.PlayAnim(actionName);
                this._unit.PropComponent.UpdateProperty(UnitProperty.ActionName, _actionAnimator.CurAction);
            }
        }

		/// <summary>
		/// 播放表情
		/// </summary>
		/// <param name="faceName">表情名</param>
		/// <param name="OnFaceFinish">表情是否播完，如果播完了，会回调OnFaceFinish函数，bool参数是 如果正常播完，为true，如果是播放其他动作打断的，那么为false</param>
		public void PlayFace(string faceName,Action<bool> OnFaceFinish = null)
        {
            if(_faceAnimator != null)
            {
				if (_faceFinish != null)
				{
					Action<bool> temp = _faceFinish;
					_faceFinish = null;
					temp.Invoke (false);
				}
				_faceFinish = OnFaceFinish;
                PlayFaceInner(faceName);
            }
			else
			{
				CLog.Log ("UnitAnimatorComponent play face fail,because _actionAnimator is null",CLogColor.Yellow);
			}
        }

        private void PlayFaceInner(string faceName)
        {
            if (_faceAnimator != null)
            {
                _faceAnimator.PlayAnim(faceName);
                this._unit.PropComponent.UpdateProperty(UnitProperty.FaceName, _faceAnimator.CurAction);
            }
        }
        //创建身体树结构
        private void CreateTreeStruct(GameObject parent)
        {
            foreach (var item in _mapPartNodes)
            {
                item.Value.Dispose();
            }
            _mapPartNodes.Clear();
            UnitPartNode root = new UnitPartNode("root", false);
			root.GO.SetLayerRecursive (parent.layer);
            parent.AddChildToParent(root.GO);
            _mapPartNodes.Add("root", root);
            foreach (var item in PlayerPartTreeMap)
            {
                UnitPartNode node;
                _mapPartNodes.TryGetValue(item.Key, out node);
                if (node == null)
                {
                    bool isBodyPart = !item.Key.StartsWith("map_");
                    node = new UnitPartNode(item.Key, isBodyPart);
                    _mapPartNodes.Add(item.Key, node);
                }
                UnitPartNode parentNode;
                _mapPartNodes.TryGetValue(item.Value, out parentNode);
                if (parentNode == null)
                {
                    bool isBodyPart = !item.Value.StartsWith("map_");
                    parentNode = new UnitPartNode(item.Value, isBodyPart);
                    _mapPartNodes.Add(item.Value, parentNode);
                }
                node.SetParent(parentNode);
            }
        }

        private void OnComplete(MultiResourceLoader loader)
		{
			_actionAnimator.ClearActionRes ();
            _faceAnimator.ClearActionRes();
			for (int j = 0; j < _listAvatar.Count; j++)
			{
				AvatarCO avatarCO = AvatarConfig.Instance.GetAvatarCO(_listAvatar[j]);
				Resource res = loader.TryGetRes (avatarCO.ResPath);
				ActionResScriptObj scriptObj = res.GetAsset<ActionResScriptObj> ();
				if (avatarCO.avatarType == AvatarPartType.Face)
				{
					_faceAnimator.AddActionRes (scriptObj);
				}
				else
				{
					_actionAnimator.AddActionRes (scriptObj);
				}
			}

            CreateTreeStruct(_unit.gameObject);
            UnitFace unitFace = this._unit.PropComponent.GetProperty<UnitFace>(UnitProperty.UnitFace);
            UpdateUnitFace(unitFace);
            _actionAnimator.UpdateUnitPartNodeMap(_mapPartNodes);
            _actionAnimator.UpdateMapSpriteOrder(SpriteOrderMap);
            _faceAnimator.UpdateUnitPartNodeMap(_mapPartNodes);
            _faceAnimator.UpdateMapSpriteOrder(SpriteOrderMap);
			if (_actionAnimator.CurAction.IsEmpty ())
			{
                PlayAnimInner(DefaultAction);
			}
			else
			{
                PlayAnimInner(_actionAnimator.CurAction);
			}
			if (_faceAnimator.CurAction.IsEmpty ())
			{
                PlayFaceInner(DefalutFace);
			}
			else
			{
                PlayFaceInner(_faceAnimator.CurAction);
			}
		}

		public override void Update (float dt)
		{
			if (_actionAnimator != null)
			{
				_actionAnimator.Update (dt);
			}
            if(_faceAnimator != null)
            {
                _faceAnimator.Update(dt);
            }
		}

		public override void Dispose ()
		{
			if (_resLoader != null)
			{
				_resLoader.Clear ();
				_resLoader = null;
			}
			if (_actionAnimator != null)
			{
				_actionAnimator.Dispose ();
				_actionAnimator = null;
			}
            if (_faceAnimator != null)
            {
                _faceAnimator.Dispose();
                _faceAnimator = null;
            }
            foreach (var item in _mapPartNodes)
            {
                item.Value.Dispose();
            }
            _mapPartNodes.Clear();
			if (_listAvatar != null)
			{
				_listAvatar.Clear ();
				_listAvatar = null;
			}
			base.Dispose ();
		}
	}

    public class UnitPartNode
    {
        public string Name { get; private set; }
        public GameObject GO { get; private set; }
        public SpriteRenderer render { get; private set; }
        public UnitPartNode Parent { get; private set; }
        public List<UnitPartNode> Children { get; private set; }

        public UnitPartNode(string name, bool isBodyPart)
        {
            this.Name = name;
            this.GO = new GameObject();
            this.GO.name = this.Name;
            if (isBodyPart)
            {
                render = this.GO.AddComponentOnce<SpriteRenderer>();
            }
            Children = new List<UnitPartNode>();
        }

        public void SetParent(UnitPartNode parent)
        {
            if (parent != null)
            {
                this.Parent = parent;
                parent.GO.AddChildToParent(this.GO, Name);
				this.GO.SetLayerRecursive (this.Parent.GO.layer);
                this.GO.transform.position = Vector3.zero;
                this.Parent.AddChild(this);
            }
        }

        public void AddChild(UnitPartNode child)
        {
            Children.Add(child);
        }

        public void Dispose()
        {
            GameObject.DestroyObject(GO);
            Parent = null;
            Children.Clear();
        }
    }
}

