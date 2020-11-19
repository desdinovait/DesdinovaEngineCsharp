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
using DesdinovaModelPipeline.Helpers;
using System.Reflection;

namespace Benchmark
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Benchmark : Core
    {
        //Input
        Input input = new Input();

        //Scene
        public static Scene_Loading loading = null;
        public static Scene_Menu menu = null;
        public static Scene_1 scene1 = null;


        public Benchmark(CoreSettings deviceSettings)
            : base(deviceSettings)
        {
        }

        protected override void Initialize()
        {
            //Initialize della classe base (engine)
            base.Initialize();
            base.WindowTitle = Assembly.GetExecutingAssembly().GetName(false).Name.ToString() + " " + Assembly.GetExecutingAssembly().GetName(false).Version.ToString() + " (using " + Core.EngineName.ToString() + " " + Core.EngineVersion + ")";

            //Carica tutte le scene
            menu = new Scene_Menu("Scene_Menu");
            loading = new Scene_Loading("Scene_Loading");
            scene1 = new Scene_1("Scene_1");

            //Aggiunge le scene
            Core.SetLoadingScene(loading);
            Core.SetCurrentScene(scene1, false);
        }


        protected override void LoadContent()
        {
            base.LoadContent();
        }


        protected override void UnloadContent()
        {
            base.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            //Update della classe base (engine)
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //Draw della classe base (engine)
            base.Draw(gameTime);
        }
    }
}

