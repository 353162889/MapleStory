using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class SceneController : BaseController<SceneController>
    {
        public GameObject SceneRoot { get; private set; }

        public override void InitController()
        {
            SceneRoot = new GameObject();
            SceneRoot.name = "SceneRoot";
            SceneRoot.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(SceneRoot);
        }

        private CommandSequence _sequence;
        public bool EnterScene(SceneCO sceneCO)
        {
            //如果正在进入场景，不允许进入场景
            if (_sequence != null)
            {
                return false;
            }
			DebugScheduler.Instance.RemoveScheduler (DebugSchedulerType.LateUpdate,OnDebugDraw);
			DebugScheduler.Instance.RemoveScheduler (DebugSchedulerType.Gizmos, OnGizmosDraw);
            SceneModel.Instance.State = SceneState.SceneLoading;
            //切场景时禁止用户输入
            GameInputController.Instance.ForbidInput(GameInputForbidKey.SceneLoad);
            _sequence = new CommandSequence();
            _sequence.On_Done += OnEnterSceneDone;
            _sequence.AddSubCommand(new LeaveSceneCmd());
            _sequence.AddSubCommand(new LoadSceneResCmd());
            _sequence.AddSubCommand(new JoinSceneCmd());
            _sequence.Execute(new SceneContext(sceneCO));
            return true;
        }

        private void OnEnterSceneDone(CommandBase obj)
        {
            _sequence = null;
            SceneModel.Instance.State = SceneState.InScene;
            GameInputController.Instance.ResumeInput(GameInputForbidKey.SceneLoad);
            this.DispatchEvent(SceneEvent.OnSceneEnterFinish);
			//渲染当前地形数据
			DebugScheduler.Instance.AddScheduler (DebugSchedulerType.LateUpdate,OnDebugDraw,0f);
			DebugScheduler.Instance.AddScheduler (DebugSchedulerType.Gizmos,OnGizmosDraw,0f);
        }

		//画网格
		private void OnDebugDraw(float dt)
		{
			Mesh mesh = SceneModel.Instance.SceneMapMO.GetMapMesh ();
			Vector3 pos = new Vector3 (SceneModel.Instance.SceneMapMO.MapOriginX,SceneModel.Instance.SceneMapMO.MapOriginY,0);
			Material mat = PreloadController.Instance.GetPrefab<Material> (PreloadConfig.GraphicsMeshMaterial);
			Graphics.DrawMesh (mesh, pos, Quaternion.identity,mat, LayerMask.NameToLayer("Default"));
		}

		//画边框
		private void OnGizmosDraw(float dt)
		{
			Vector3 v1 = new Vector3 (SceneModel.Instance.SceneMapMO.MapOriginX,SceneModel.Instance.SceneMapMO.MapOriginY,0);
			Vector3 v2 = new Vector3 (SceneModel.Instance.SceneMapMO.MapOriginX +  SceneModel.Instance.SceneMapMO.MapWidth,SceneModel.Instance.SceneMapMO.MapOriginY,0);
			Vector3 v3 = new Vector3 (SceneModel.Instance.SceneMapMO.MapOriginX +  SceneModel.Instance.SceneMapMO.MapWidth,SceneModel.Instance.SceneMapMO.MapOriginY + SceneModel.Instance.SceneMapMO.MapHeight,0);
			Vector3 v4 = new Vector3 (SceneModel.Instance.SceneMapMO.MapOriginX,SceneModel.Instance.SceneMapMO.MapOriginY + SceneModel.Instance.SceneMapMO.MapHeight,0);
			Color old =  Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawLine (v1, v2);
			Gizmos.DrawLine (v2, v3);
			Gizmos.DrawLine (v3, v4);
			Gizmos.DrawLine (v4, v1);
			Gizmos.color = old;
		}
    }
}
