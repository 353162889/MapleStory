using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SceneContext : ICommandContext
    {
        public SceneCO sceneCO { get; private set; }

        public SceneContext(SceneCO sceneCO)
        {
            this.sceneCO = sceneCO;
        }

        public void Dispose()
        {
            sceneCO = null;
        }
    }
}
