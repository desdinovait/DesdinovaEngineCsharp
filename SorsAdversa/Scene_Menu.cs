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

namespace SorsAdversa
{
    public class Scene_Menu : Scene
    {
        //Suono di fondo
        private Sound soundBackground;
        private Sound click;

        //Font di livello
        private Font font;

        //Sprite
        private Sprite title;
        private Sprite menuVoice1;
        private Sprite menuVoice2;
        private Sprite menuVoice3;
        private Sprite cursor;

        //SpriteBacther 2D
        private SpriteBatcher spriteBatcher;

        public Scene_Menu(string sceneName): base(sceneName)
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

            //Font di livello
            font = new Font("Content\\Font\\Courier New", base.SceneContent);
            font.Color = Color.DarkBlue;
            font.Text = "ver 0.2";
            font.Scale = 0.75f;
            font.CharactersSpacing = 5.0f;
            font.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - font.Width - 6, Core.Graphics.GraphicsDevice.Viewport.Height - font.Height - 3);

            //Sprite
            title = new Sprite("Content\\Texture\\Title", 1, base.SceneContent);
            title.Scale = new Vector2(1.0f, 1.0f);
            title.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (title.Width / 2), 25);
            menuVoice1 = new Sprite("Content\\Texture\\MenuVoice1", 2, base.SceneContent);
            menuVoice1.Position = new Vector2(100, 200);
            menuVoice2 = new Sprite("Content\\Texture\\MenuVoice1", 2, base.SceneContent);
            menuVoice2.Position = new Vector2(300, 200);
            menuVoice3 = new Sprite("Content\\Texture\\MenuVoice1", 2, base.SceneContent);
            menuVoice3.Position = new Vector2(500, 200);
            cursor = new Sprite("Content\\Texture\\MouseCursor", 2, base.SceneContent);

            //SpriteBatcher 2D
            spriteBatcher = new SpriteBatcher();
            //spriteBatcher.Add(menuBackground);
            spriteBatcher.Add(title);
            spriteBatcher.Add(menuVoice1);
            spriteBatcher.Add(menuVoice2);
            spriteBatcher.Add(menuVoice3);
            spriteBatcher.Add(cursor);

            //Creazione avvenuta
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            //Update suoni
            soundBackground.Update(gameTime, base.SceneCamera);
            click.Update(gameTime, base.SceneCamera);

            //Suono di prova
            if (base.SceneInput.IsKeyUp(Keys.B))
                click.Play();

            //Font
            font.Update(gameTime);

            //Sprite
            title.Update(gameTime);
            menuVoice1.Update(gameTime);
            menuVoice2.Update(gameTime);
            menuVoice3.Update(gameTime);
            cursor.Position = new Vector2(base.SceneInput.GetMouseState().X, base.SceneInput.GetMouseState().Y);
            cursor.Update(gameTime);

            //SpriteBatcher 2D
            spriteBatcher.Update(gameTime);

            //Selezione voce di menu 1
            if (cursor.IntersectSimple(menuVoice1))
            {
                menuVoice1.CurrentFrame = 1;
                if (base.SceneInput.GetMouseState().LeftButton == ButtonState.Pressed)
                {
                    SorsAdversa.level = new Scene_Level("Scene_Level");
                    Core.SetCurrentScene(SorsAdversa.level, true);
                    SorsAdversa.menu = null;
                    return;
                }
            }
            else
            {
                menuVoice1.CurrentFrame = 0;
            }

            //Selezione voce di menu 2
            if (cursor.IntersectSimple(menuVoice2))
            {
                menuVoice2.CurrentFrame = 1;
            }
            else
            {
                menuVoice2.CurrentFrame = 0;
            }

            //Selezione voce di menu 3
            if (cursor.IntersectSimple(menuVoice3))
            {
                menuVoice3.CurrentFrame = 1;
                if (base.SceneInput.GetMouseState().LeftButton == ButtonState.Pressed)
                {
                    Core.Exit();
                }
            }
            else
            {
                menuVoice3.CurrentFrame = 0;
            }
        }


        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);

            //Font
            font.Draw();

            //Sprite
            title.Draw();
            menuVoice1.Draw();
            menuVoice2.Draw();
            menuVoice3.Draw();
            cursor.Draw();

            //SpriteBatcher 2D
            spriteBatcher.Draw(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, Matrix.Identity);
        }

        public override void Release()
        {
            //Rilascia le risorse
            soundBackground.Stop();
            soundBackground = null;
            click.Stop();
            click = null;
            font = null;
            title = null;
            menuVoice1 = null;
            menuVoice2 = null;
            menuVoice3 = null;
            spriteBatcher = null;
            cursor = null;
        }
    }
}

