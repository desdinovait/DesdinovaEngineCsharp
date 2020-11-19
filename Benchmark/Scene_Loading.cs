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

namespace Benchmark
{
    public class Scene_Loading : Scene
    {
        //private Font_Typed fontLoading;
        private Font fontLoading;

        public Scene_Loading(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            base.BackgroundColor = Color.Black;

            /*Font
            fontLoading = new Font_Typed("Content\\Font\\Courier New", base.SceneContent);
            fontLoading.ToDraw = true;
            fontLoading.Color = Color.White;
            fontLoading.Scale = 1.0f;
            fontLoading.CharactersSpacing = 5.0f;
            fontLoading.AdditionalChar = '_';
            fontLoading.Interval = 40.0f;
            fontLoading.HideInterval = 5000;
            fontLoading.Text = "Loading...";
            fontLoading.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (fontLoading.Width / 2), (Core.Graphics.GraphicsDevice.Viewport.Height / 2.0f) - fontLoading.Height);
            */

            fontLoading = new Font(1, this);
            fontLoading[0] = new FontLine("Content\\Font\\Courier New", this);
            fontLoading[0].Text = "Loading...";
            fontLoading[0].Color = Color.White;
            fontLoading[0].Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (fontLoading[0].Width / 2), (Core.Graphics.GraphicsDevice.Viewport.Height / 2.0f) - fontLoading[0].Height);
            this.AddObject(fontLoading);

            return true;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Release()
        {
            //Rilascia le risorse
            this.RemoveObject(fontLoading);
            fontLoading = null;
        }
    }
}
