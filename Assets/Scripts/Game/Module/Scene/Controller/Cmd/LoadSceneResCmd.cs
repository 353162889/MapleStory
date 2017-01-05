using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{
    public class LoadSceneResCmd : SceneCommandBase
    {
        private MultiResourceLoader _loader;
        public override void Execute(ICommandContext context)
        {
            base.Execute(context);
            CLog.Log("LoadSceneResCmd:start");
            SceneModel.Instance.Clear();
            SceneCO sceneCO = this._sceneContext.sceneCO;
            _loader = new MultiResourceLoader();
            _loader.LoadList(new List<string> {sceneCO.ResPath,sceneCO.MapPath }, OnComplete);
        }

        private void OnComplete(MultiResourceLoader obj)
        {
            SceneCO sceneCO = this._sceneContext.sceneCO;
            Resource res = _loader.TryGetRes(sceneCO.ResPath);
            GameObject prefab = res.GetAsset<GameObject>();
            GameObject go = GameObject.Instantiate(prefab);
            SceneController.Instance.SceneRoot.AddChildToParent(go, prefab.name);

            res = _loader.TryGetRes(sceneCO.MapPath);
            string content = res.GetText();
            SceneMapData sceneMapData = StringUtil.Deserialize<SceneMapData>(content);
            SceneMapMO sceneMapMO = new SceneMapMO(sceneMapData);
            SceneModel.Instance.SceneCO = sceneCO;
            SceneModel.Instance.SceneMapMO = sceneMapMO;
            CLog.Log("LoadSceneResCmd:done");
            this.OnExecuteDone(CmdExecuteState.Success);
        }

        public override void OnDestroy()
        {
            if (_loader != null)
            {
                _loader.Clear();
                _loader = null;
            }
            base.OnDestroy();
        }
    }
}
