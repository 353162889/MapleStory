using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

namespace Game
{
    public class SceneCommandBase : CommandBase
    {
        protected SceneContext _sceneContext;
        public override void Execute(ICommandContext context)
        {
            base.Execute(context);
            _sceneContext = (SceneContext)context;
        }

        public override void OnDestroy()
        {
            this._sceneContext = null;
            base.OnDestroy();
        }
    }
}
