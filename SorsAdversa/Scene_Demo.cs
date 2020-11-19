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
using System.Threading;
using System.Reflection;

namespace SorsAdversa
{
    public class Scene_Demo : Scene
    {
        //Suono di fondo
        private Sound soundBackground;
        private Sound click;

        //Font di livello
        private Font version;
        private Font play;

        //Sprite
        private Sprite title;
        private Sprite background;
        private Sprite cursor;

        //SpriteBacther 2D
        private SpriteBatcher spriteBatcher;

        public Scene_Demo(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            //Esegue la musica di background
            soundBackground = new Sound("Menu", false);
            soundBackground.ToLoop = true;
            soundBackground.Play();

            //Suono per il movimento del mouse
            click = new Sound("Click", true);
            click.ToLoop = false;

            //Fonts
            version = new Font("Content\\Font\\Courier New", base.SceneContent);
            version.Color = Color.Black;
            version.Text = "(C) 2008 ver " + Assembly.GetExecutingAssembly().GetName(false).Version.ToString();
            version.Scale = 0.75f;
            version.CharactersSpacing = 5.0f;
            version.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - version.Width - 6, Core.Graphics.GraphicsDevice.Viewport.Height - version.Height - 3);
            
            play = new Font("Content\\Font\\Courier New", base.SceneContent);
            play.Color = Color.Red;
            play.Text = "Press ENTER to play";
            play.Scale = 1.5f;
            play.CharactersSpacing = 5.0f;
            play.Position = new Vector2(15, Core.Graphics.GraphicsDevice.Viewport.Height - version.Height - 15);

            //Sprite
            title = new Sprite("Content\\Texture\\Title", 1, base.SceneContent);
            title.Scale = new Vector2(1.0f, 1.0f);
            title.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (title.Width / 2), 15);
            background = new Sprite("Content\\Texture\\Demo", 1, base.SceneContent);
            background.Scale = new Vector2(0.75f, 0.75f);
            background.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (background.Width / 2), title.Height + 15);
            cursor = new Sprite("Content\\Texture\\MouseCursor", 2, base.SceneContent);

            //SpriteBatcher 2D
            spriteBatcher = new SpriteBatcher();
            spriteBatcher.Add(title);
            spriteBatcher.Add(background);
            spriteBatcher.Add(cursor);

            //Creazione avvenuta
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            //Update suoni
            soundBackground.Update(gameTime, base.SceneCamera);
            click.Update(gameTime, base.SceneCamera);

            //Font
            version.Update(gameTime);
            play.Update(gameTime);

            //Sprite
            title.Update(gameTime);
            background.Update(gameTime);
            cursor.Position = new Vector2(base.SceneInput.GetMouseState().X, base.SceneInput.GetMouseState().Y);
            cursor.Update(gameTime);

            //SpriteBatcher 2D
            spriteBatcher.Update(gameTime);

            //Input
            if (base.SceneInput.IsKeyUp(Keys.Enter))
            {
                SorsAdversa.level = new Scene_Level("Scene_Level");
                Core.SetCurrentScene(SorsAdversa.level, true);
                SorsAdversa.demo = null;
            }

        }


        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);

            //Font
            version.Draw();
            play.Draw();

            //Sprite
            title.Draw();
            background.Draw();
            cursor.Draw();

            //SpriteBatcher 2D
            spriteBatcher.Draw(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, Matrix.Identity);
        }

        public override void Release()
        {
            //Rilascia le risorse
            soundBackground.Stop();
            soundBackground = null;
            click.Stop();
            click = null;
            version = null;
            title = null;
            background = null;
            spriteBatcher = null;
            cursor = null;
        }
    }
}

