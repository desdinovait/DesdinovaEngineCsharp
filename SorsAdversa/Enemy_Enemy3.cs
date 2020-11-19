//Using di sistema
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Using DesdinovaEngineX
using DesdinovaModelPipeline;

namespace SorsAdversa
{
    public class Enemy_Enemy3 : Enemy
    {

        public Enemy_Enemy3(ContentManager contentManager, Scene parentScene) : base(parentScene)
        {
            if (base.Initialize("Content\\Model\\Enemy\\Enemy3", contentManager))
            {
                base.Name = "Enemy_Enemy3";
            }
        }
    }
}
