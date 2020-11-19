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
using System.Windows.Forms;
using SorsAdversa;
using EditorEngine;

namespace EditorEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Editor : Core
    {
        //Input
        Input input = new Input();

        //Scene
        public static Scene_Loading loading = new Scene_Loading("Scene_Loading");
        public static Scene_Main main = new Scene_Main("Scene_Main");

        public Editor(CoreSettings deviceSettings)
            : base(deviceSettings)
        {

        }

        protected override void Initialize()
        {
            //Initialize della classe base (engine)
            base.Initialize();
            base.WindowTitle = Assembly.GetExecutingAssembly().GetName(false).Name.ToString() + " " + Assembly.GetExecutingAssembly().GetName(false).Version.ToString() + " (using " + Core.EngineName.ToString() + " " + Core.EngineVersion + ")";
            base.DefaultLoadingColor = Color.Black;

            //Aggiunge le scene
            Core.SetLoadingScene(loading);
            Core.SetCurrentScene(main, false);

            //Aggiunge il menu
            EditorMenu menuControl = new EditorMenu();
            menuControl.Location = new System.Drawing.Point(0, 0);
            menuControl.Width = this.Window.ClientBounds.Width;
            Control.FromHandle(this.Window.Handle).Controls.Add(menuControl);

            //Aggiunge la barra sotto
            PositionBar posBarControl = new PositionBar();
            posBarControl.Location = new System.Drawing.Point(0, this.Window.ClientBounds.Height - posBarControl.Height);
            posBarControl.Width = this.Window.ClientBounds.Width;
            Control.FromHandle(this.Window.Handle).Controls.Add(posBarControl);
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
