using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneGraph
{
    public class Sentinel : EngineTriggerObject
    {

        public Sentinel(Scene parentScene): base(parentScene)
        {
            IsCreated = true;
        }
    }
}
