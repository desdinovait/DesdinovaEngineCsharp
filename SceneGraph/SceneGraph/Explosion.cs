using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneGraph
{
    public class Explosion : EngineSoundObject
    {
        public Explosion(Scene parentScene):base(parentScene)
        {
            IsCreated = true;
        }

        public override void Play()
        {
            throw new NotImplementedException();
        }
        public override void Pause()
        {
            throw new NotImplementedException();
        }
        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
