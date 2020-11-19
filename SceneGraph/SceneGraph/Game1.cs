using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SceneGraph
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Core
    {
        //Oggetti
        Scene1 scene1;
        Scene1 scene2;
        Scene1 scene3;

        public Game1()
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Scene
            scene1 = new Scene1("Scene1", base.GraphicsDevice, base.Services);
            //scene2 = new Scene2("Scene2", base.GraphicsDevice, base.Services);
            //scene3 = new Scene3("Scene3", base.GraphicsDevice, base.Services);

            //Imposta la scena corrente
            base.CurrentScene = scene1;
        }


    }
}
