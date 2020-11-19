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
using SorsAdversa.Properties;

namespace SorsAdversa
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SorsAdversa : Core
    {
        //Input
        Input input = new Input();

        //Scene
        public static Scene_Intro intro = null;
        public static Scene_Demo demo = null;
        public static Scene_Test test = null;
        public static Scene_Loading loading = null;
        public static Scene_Menu menu = null;
        public static Scene_Level level = null;  //Passare il file da caricare con le info sul livello
        public static Scene_Hangar hangar = null;


        public SorsAdversa(CoreSettings deviceSettings) : base(deviceSettings)
        {
        }

        protected override void Initialize()
        {
            //Initialize della classe base (engine)
            base.Initialize();
            base.WindowTitle = Assembly.GetExecutingAssembly().GetName(false).Name.ToString() + " " + Assembly.GetExecutingAssembly().GetName(false).Version.ToString() + " (using " + Core.EngineName.ToString() + " " + Core.EngineVersion + ")";

            //Carica tutte le scene
            intro = new Scene_Intro("Scene_Intro");
            menu = new Scene_Menu("Scene_Menu");
            loading = new Scene_Loading("Scene_Loading");
            demo = new Scene_Demo("Scene_Demo");
            test = new Scene_Test("Scene_Test");
            hangar = new Scene_Hangar("Scene_Hangar");

            level = new Scene_Level("Scene_Level");
            level.Filename = "Level1.xml";

            //Aggiunge le scene
            Core.SetLoadingScene(loading);
            Core.SetCurrentScene(level, false);
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
