using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game;
using Framework;

public class SceneTest : MonoBehaviour
{
    private void Start()
    {
        SceneController.Instance.InitController();
        SceneModel.Instance.InitModel();
        UpdateScheduler.CreateInstance(this.gameObject);
        FixedUpdateScheduler.CreateInstance(this.gameObject);
        ResourceManager.CreateInstance(this.gameObject);

        SceneCO sceneCO = new SceneCO("测试场景", new Vector3(3.026f,1.56f,0), "Scene/S_Test/S_Test.prefab", "Scene/S_Test/S_Test_MapData.byte");
        SceneController.Instance.AddListener(SceneEvent.OnSceneEnterFinish, OnSceneEnterFinish);
        SceneController.Instance.EnterScene(sceneCO);
        //Debug.Log(Resources.Load("Scene/S_Test/S_Test_MapData"));
    }

    private void OnSceneEnterFinish(int eventID, object[] param)
    {
        Debug.Log("OnSceneEnterFinish");
    }
}
