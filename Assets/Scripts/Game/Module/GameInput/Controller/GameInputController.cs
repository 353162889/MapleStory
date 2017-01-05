using System;
using Framework;
using UnityEngine;

namespace Game
{
	public class GameInputController : BaseController<GameInputController>
	{
        public event Action<Vector2> OnInputMove { add { _gameInput.OnInputMove += value; }remove { _gameInput.OnInputMove -= value; } }
		public event Action<VirtualKey> OnKeyClick { add { _gameInput.OnKeyClick += value; }remove { _gameInput.OnKeyClick -= value; } }
        private GameInputBase _gameInput;

		public override void InitController ()
		{

#if UNITY_EDITOR || UNITY_EDITOR_WIN
            _gameInput = new KeyBoardGameInput();
#else
              CLog.LogError("can not support GameInputController");
#endif
            _gameInput.Init();
            //初始进入游戏时，默认静止输入，第一次进入场景时放开
            ForbidInput(GameInputForbidKey.NotInGame);
            SceneController.Instance.AddListener(SceneEvent.OnSceneEnterFinish, OnFirstSceneEnter);
        }

        private void OnFirstSceneEnter(int eventID, object[] param)
        {
            SceneController.Instance.RemoveListener(SceneEvent.OnSceneEnterFinish, OnFirstSceneEnter);
            ResumeInput(GameInputForbidKey.NotInGame);
        }

        public void ForbidInput(GameInputForbidKey key)
        {
            _gameInput.ForbidInput(key);
        }

        public void ResumeInput(GameInputForbidKey key)
        {
            _gameInput.ResumeInput(key);
        }

        public void ClearForbid()
        {
            _gameInput.ClearForbid();
        }
    }
}

