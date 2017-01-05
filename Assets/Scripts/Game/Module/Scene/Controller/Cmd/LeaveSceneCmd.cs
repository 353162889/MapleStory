using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class LeaveSceneCmd : SceneCommandBase
    {
        public override void Execute(ICommandContext context)
        {
            base.Execute(context);
            CLog.Log("LeaveSceneCmd:start");
			CameraController.Instance.SetFocusUnit (null);
			UnitOperateController.Instance.SetOperateUnit (null);
            UnitPlayerManager.Instance.ClearPlayers();
            SceneController.Instance.SceneRoot.DestroyChildren();
            //释放没有引用的资源
            ResourceManager.Instance.ReleaseUnUseRes();
            SceneController.Instance.DispatchEvent(SceneEvent.OnSceneUnloaded);
            CLog.Log("LeaveSceneCmd:done");
            this.OnExecuteDone(CmdExecuteState.Success);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
