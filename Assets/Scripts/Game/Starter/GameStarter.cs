using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Framework;

namespace Game
{
    public class GameStarter : MonoBehaviour
    {
        private void Start()
        {
           
            UpdateScheduler.CreateInstance(this.gameObject);
            FixedUpdateScheduler.CreateInstance(this.gameObject);
			LateUpdateScheduler.CreateInstance (this.gameObject);
			DebugScheduler.CreateInstance (this.gameObject);
            ResourceManager.CreateInstance(this.gameObject);
            //脚本的执行顺序应该调到默认之间
            KeyBoardDispatcher.CreateInstance(this.gameObject);
            //脚本的执行顺序应该调到默认之间
            TouchDispatcher.CreateInstance(this.gameObject);

            InitConfig();
            InitController();
            InitModel();

			PreloadController.Instance.AddListener (PreloadEvent.OnPreloadFinish, OnPreloadFinish);
			PreloadController.Instance.StartPreload ();
        }

		private void OnPreloadFinish(int notify,object arg)
		{
			SceneCO sceneCO = new SceneCO("测试场景", new Vector3(3.026f, 8.56f, 0), "Scene/S_Test/S_Test.prefab", "Scene/S_Test/S_Test_MapData.byte");
			SceneController.Instance.EnterScene(sceneCO);
		}

        private void InitConfig()
        {
        }

        private void InitController()
        {
            SceneController.Instance.InitController();
            GameInputController.Instance.InitController();
			PreloadController.Instance.InitController ();
			CameraController.Instance.InitController ();
			UnitOperateController.Instance.InitController ();
        }

        private void InitModel()
        {
            SceneModel.Instance.InitModel();
        }
    }
}
