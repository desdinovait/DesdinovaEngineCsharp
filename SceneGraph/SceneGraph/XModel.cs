using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SceneGraph
{
    public class XModel : Engine3DObject
    {
        public XModel(Scene parentScene): base(parentScene)
        {
            BoundingBox = new BoundingBox(new Vector3(-20, -20, -20), new Vector3(20, 20, 20));
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
