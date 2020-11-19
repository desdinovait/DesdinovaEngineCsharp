using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SceneGraph
{
    public class Panel : Engine2DObject
    {
        public Panel(Scene parentScene) : base(parentScene)
        {
            IsCreated = true;
        }

        public override void Update(GameTime gametime, Camera camera)
        {
            base.Update(gametime, camera);
        }

        public override void Draw(LightEffect lightEffect)
        {
            base.Draw(lightEffect);
        }
    }
}
