using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SceneGraph
{
    public class Ship : XModel
    {
        private Explosion sound1;
        private Explosion sound2;

        public Ship(Scene parentScene): base(parentScene)
        {
            try
            {
                BoundingBox = new BoundingBox(new Vector3(-20, -20, -20), new Vector3(20, 20, 20));

                sound1 = new Explosion(parentScene);
                sound2 = new Explosion(parentScene);

                parentScene.AddObject(sound1);
                parentScene.AddObject(sound2);

                IsCreated = true;
            }
            catch
            {
                IsCreated = false;
            }
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
