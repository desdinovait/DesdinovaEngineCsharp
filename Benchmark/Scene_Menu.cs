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


namespace Benchmark
{
    public enum MenuVoice
    {
        Scene1 = 1,
        Scene2 = 2,
        Exit = 3
    }

    public class Scene_Menu : Scene
    {
        Font fontScene = null;
        InfoPanel infoPanel = null;
        MovieEffect cineEffect = null;
        MenuVoice selection = MenuVoice.Scene1;
        Sprite background = null;
        SpriteBatcher spriteBatcher = null;
        AudioSong music = null;

        public Scene_Menu(string sceneName) : base(sceneName)
        {

        }

        //Metodi da implementare
        public override bool Initialize()
        {
            //Camera
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 50.0f;
            base.SceneCamera.PositionY = 20.0f;
            base.SceneCamera.PositionZ = 20.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 0.0f;
            base.SceneCamera.TargetZ = 0.0f;


            //Sfondo
            background = new Sprite("Content\\Texture\\MenuBackground", 1, this);
            background.ToDraw = true;
            background.Position = new Vector2(0, 0);

            //Sprite batcher
            spriteBatcher = new SpriteBatcher(this);
            spriteBatcher.BlendMode = SpriteBlendMode.None;
            spriteBatcher.SortMode = SpriteSortMode.Immediate;
            spriteBatcher.TransformMatrix = Matrix.Identity;
            spriteBatcher.Add(background);
            base.AddObject(spriteBatcher);

            //Informazioni
            infoPanel = new InfoPanel(this);
            infoPanel.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - infoPanel.Width - 10, 10);
            infoPanel.TextColor = Color.Yellow;
            infoPanel.BackgroundColor = new Color(0, 0, 128, 96);
            infoPanel.BorderColor = Color.Blue;
            infoPanel.Mode = InfoPanel.InfoPanelMode.Extended;
            base.AddObject(infoPanel);

            //Font
            fontScene = new Font(6, this);
            fontScene[0] = new FontLine("Content\\Font\\Courier New", this);
            fontScene[0].Text = ". Start SCENE 1";
            fontScene[0].ShadowEnabled = true;
            fontScene[0].ShadowColor = Color.DarkGray;
            fontScene[0].ShadowOffset = 1.0f;
            fontScene[0].Position = new Vector2(20, (Core.Graphics.GraphicsDevice.Viewport.Height / 2) - 30);
            fontScene[0].Color = Color.Black;

            fontScene[1] = new FontLine("Content\\Font\\Courier New", this);
            fontScene[1].Text = ". Start SCENE 2 (not implemented yet)";
            fontScene[1].ShadowEnabled = true;
            fontScene[1].ShadowColor = Color.DarkGray;
            fontScene[1].ShadowOffset = 1.0f;
            fontScene[1].Position = new Vector2(20, (Core.Graphics.GraphicsDevice.Viewport.Height / 2));
            fontScene[1].Color = Color.Black;

            fontScene[2] = new FontLine("Content\\Font\\Courier New", this);
            fontScene[2].Text = ". Exit";
            fontScene[2].ShadowEnabled = true;
            fontScene[2].ShadowColor = Color.DarkGray;
            fontScene[2].ShadowOffset = 1.0f;
            fontScene[2].Position = new Vector2(20, (Core.Graphics.GraphicsDevice.Viewport.Height / 2) + 30);
            fontScene[2].Color = Color.Black;

            fontScene[3] = new FontLine("Content\\Font\\Verdana", this);
            fontScene[3].Text = "Desdinova Engine X - Benchmark Application";
            fontScene[3].ShadowEnabled = false;
            fontScene[3].Color = Color.Black;
            fontScene[3].Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - fontScene[3].Width - 10, Core.Graphics.GraphicsDevice.Viewport.Height - (fontScene[3].Height * 3) - 10);

            fontScene[4] = new FontLine("Content\\Font\\Verdana", this);
            fontScene[4].Text = "Internal purpose and features testing only";
            fontScene[4].ShadowEnabled = false;
            fontScene[4].Color = Color.Black;
            fontScene[4].Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - fontScene[4].Width - 10, Core.Graphics.GraphicsDevice.Viewport.Height - (fontScene[4].Height * 2) - 10);

            fontScene[5] = new FontLine("Content\\Font\\Verdana", this);
            fontScene[5].Text = "Copyright (C) 2009 Ferla Daniele. Info www.desdinova.it";
            fontScene[5].ShadowEnabled = false;
            fontScene[5].Color = Color.Black;
            fontScene[5].Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - fontScene[5].Width - 10, Core.Graphics.GraphicsDevice.Viewport.Height - fontScene[5].Height - 10);

            base.AddObject(fontScene);

            //Effetto cine
            cineEffect = new MovieEffect(new Color(0, 0, 0, 255), false, false, Core.Graphics.GraphicsDevice.Viewport.Height / 2, this);
            cineEffect.Enable = true;
            cineEffect.PlayAnimation(0.25f, MovieEffect.AnimationType.FadeReverse);
            base.AddObject(cineEffect);

            //Musica
            music = new AudioSong("Content\\Music\\Ambient003", this);
            //music.Play(true);
            base.AddObject(music);

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Input
            base.SceneInput.Update(gameTime);

            //Selezione
            if (selection == MenuVoice.Scene1)
            {
                fontScene[0].Color = Color.Blue;
                fontScene[1].Color = Color.Black;
                fontScene[2].Color = Color.Black;
            }
            else if (selection == MenuVoice.Scene2)
            {
                fontScene[0].Color = Color.Black;
                fontScene[1].Color = Color.Blue;
                fontScene[2].Color = Color.Black;
            }
            else if (selection == MenuVoice.Exit)
            {
                fontScene[0].Color = Color.Black;
                fontScene[1].Color = Color.Black;
                fontScene[2].Color = Color.Blue;
            }

            //Controlli
            if (base.SceneInput.IsKeyUp(Keys.F2))
            {
                Core.ToggleFullscreen();
            }

            if (base.SceneInput.IsKeyUp(Keys.Up))
            {
                selection = selection - 1;
                if (selection < MenuVoice.Scene1)
                    selection = MenuVoice.Exit;
            }
            if (base.SceneInput.IsKeyUp(Keys.Down))
            {
                selection = selection + 1;
                if (selection > MenuVoice.Exit)
                    selection = MenuVoice.Scene1;
            }
            if (base.SceneInput.IsKeyDown(Keys.Enter))
            {
                if (selection == MenuVoice.Scene1)
                {
                    Benchmark.scene1 = new Scene_1("Scene_1");
                    Core.SetCurrentScene(Benchmark.scene1, true);
                    Benchmark.menu = null;
                    return;
                }
                else if (selection == MenuVoice.Scene2)
                {
                    /*currentMenu = MenuView.Start;
                    Benchmark.scene2 = new Scene_2("Scene_2");
                    Core.SetCurrentScene(Benchmark.scene2, true);
                    Benchmark.menu = null;*/
                }
                else if (selection == MenuVoice.Exit)
                {
                    Core.Exit();
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
        }


        public override void Release()
        {
            base.RemoveObject(spriteBatcher);
            base.RemoveObject(fontScene);
            base.RemoveObject(cineEffect);
            base.RemoveObject(music);
            base.RemoveObject(infoPanel);

            fontScene = null;
            cineEffect = null;
            music.Dispose();
            music = null;
            spriteBatcher = null;
            infoPanel = null;
        }

    }
}
