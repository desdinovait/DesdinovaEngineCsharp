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

namespace EditorEngine
{
    public class Scene_Loading : Scene
    {
        private Font fontLoading;
        private Sprite spriteLoading1;
        private SpriteBatcher spriteBatcher;

        public Scene_Loading(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {

            //Sprite di caricamento
            spriteLoading1 = new Sprite("Content\\Texture\\Loading2", 1, base.SceneContent);
            spriteLoading1.PositionX = (Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (spriteLoading1.Width / 2);
            spriteLoading1.PositionY = (Core.Graphics.GraphicsDevice.Viewport.Height / 2) - (spriteLoading1.Height / 2);

            //Font di avviso
            fontLoading = new Font("Content\\Font\\Courier New", base.SceneContent);
            fontLoading.Text = "EDLibitum for Sors Adversa";
            fontLoading.Scale = 1.50f;
            fontLoading.PositionX = (Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (fontLoading.Width / 2);
            fontLoading.PositionY = spriteLoading1.PositionY - fontLoading.Height - 10;
            fontLoading.Color = Color.LightBlue;

            //Batcher
            spriteBatcher = new SpriteBatcher();
            spriteBatcher.Add(spriteLoading1);

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            fontLoading.Update(gameTime);
            spriteLoading1.Update(gameTime);
            spriteBatcher.Update(gameTime);
        }

        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            fontLoading.Draw();

            spriteLoading1.Draw();
            spriteBatcher.Draw(SpriteBlendMode.None, SpriteSortMode.Immediate, Matrix.Identity);
        }

        public override void Release()
        {
            //Rilascia le risorse
            fontLoading = null;
            spriteLoading1 = null;
            spriteBatcher = null;
        }
    }
}
