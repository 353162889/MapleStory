using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using UnityEngine;

namespace Game
{ 
    public class JoinSceneCmd : SceneCommandBase
    {
        public override void Execute(ICommandContext context)
        {
            base.Execute(context);
            CLog.Log("JoinSceneCmd:start");
            List<string> listAvatar = new List<string>(){
            "00002000","00012000","00020000","00030000"
             };
            UnitPlayerMO playerMO = UnitPlayerModel.Instance.CreatePlayerMO(SceneModel.Instance.SceneCO.BirthPoint, listAvatar);
            UnitPlayerModel.Instance.AddPlayerMO(playerMO);
            UnitPlayer player = UnitPlayerManager.Instance.AddPlayer(playerMO);
            UnitPlayerManager.Instance.MainPlayer = player;
			CameraController.Instance.SetFocusUnit (player);
			UnitOperateController.Instance.SetOperateUnit (player);
            SceneController.Instance.DispatchEvent(SceneEvent.OnSceneLoaded);
            CLog.Log("JoinSceneCmd:done");
            this.OnExecuteDone(CmdExecuteState.Success);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
