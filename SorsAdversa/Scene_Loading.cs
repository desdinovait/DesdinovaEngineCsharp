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
using SorsAdversa.Properties;

namespace SorsAdversa
{
    public class Scene_Loading : Scene
    {
        private Font fontLoading;
        private Font_Typed fontTyped;
        private Sprite spriteLoading1;
        private SpriteBatcher spriteBatcher;
        private string currentTip;

        public Scene_Loading(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            //Font di avviso
            fontLoading = new Font("Content\\Font\\Courier New", base.SceneContent);
            fontLoading.Color = Color.White;
            fontLoading.Scale = 0.75f;

            //Sprite di caricamento
            spriteLoading1 = new Sprite("Content\\Texture\\Loading2", 1, base.SceneContent);
            spriteLoading1.PositionX = (Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (spriteLoading1.Width / 2);
            spriteLoading1.PositionY = (Core.Graphics.GraphicsDevice.Viewport.Height / 2) - (spriteLoading1.Height / 2);

            //Batcher
            spriteBatcher = new SpriteBatcher();
            spriteBatcher.Add(spriteLoading1);

            //Preleva il suggerimento corrente dal file
            Random rnd = new Random();
            currentTip = Tips.Default["Tip" + rnd.Next(1, Tips.Default.Properties.Count)].ToString();

            //Fonts
            fontTyped = new Font_Typed("Content\\Font\\Courier New", base.SceneContent);
            fontTyped.ToDraw = true;
            fontTyped.Color = Color.LightGreen;
            fontTyped.Scale = 1.0f;
            fontTyped.CharactersSpacing = 5.0f;
            fontTyped.AdditionalChar = '_';
            fontTyped.Interval = 60.0f;
            fontTyped.HideInterval = 1500;
            fontTyped.Text = "Loading...";
            fontTyped.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (fontTyped.Width / 2), (Core.Graphics.GraphicsDevice.Viewport.Height / 2.0f) - fontTyped.Height);
            fontTyped.OnFinishTyping += new EventHandler(fontTyped_OnFinishTyping);

            return true;
        }

        void fontTyped_OnFinishTyping(object sender, EventArgs e)
        {
            fontTyped.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            fontLoading.Update(gameTime);
            fontTyped.Update(gameTime);
            spriteLoading1.Update(gameTime);
            spriteBatcher.Update(gameTime);
        }

        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            fontLoading.ShadowEnabled = false;
            fontLoading.Text = "Sors Adversa Development Techdemo (do not distribute)";
            fontLoading.Color = Color.CornflowerBlue;
            fontLoading.Position = new Vector2(10, 10);
            fontLoading.Draw();

            fontLoading.Text = "W.A.S.D - Player 1 Movements";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 25);
            fontLoading.Draw();
            fontLoading.Text = "E.R.T - Player 1 Weapons";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();

            fontLoading.Text = "I.J.K.L - Player 2 Movements";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 25);
            fontLoading.Draw();
            fontLoading.Text = "O - Player 2 Weapon";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();

            fontLoading.Text = "Z - Shake camera";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 25);
            fontLoading.Draw();
            fontLoading.Text = "X - Invoke GarbageCollector";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();
            fontLoading.Text = "C - Change camera";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();
            fontLoading.Text = "V - Screenshot";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();
            fontLoading.Text = "B - Info panel mode";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();
            fontLoading.Text = "ESC - InGame menu";
            fontLoading.Color = Color.White;
            fontLoading.Position = new Vector2(10, fontLoading.Position.Y + 15);
            fontLoading.Draw();

            fontLoading.ShadowEnabled = true;
            fontLoading.ShadowColor = Color.Gray;
            fontLoading.Color = Color.White;
            fontLoading.Text = "TIP: " + currentTip;
            fontLoading.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (fontLoading.Width / 2), Core.Graphics.GraphicsDevice.Viewport.Height - fontLoading.Height - 20);
            fontLoading.Draw();

            fontTyped.Draw();

            //spriteLoading1.Draw();
            //spriteBatcher.Draw(SpriteBlendMode.None, SpriteSortMode.Immediate, Matrix.Identity);
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
