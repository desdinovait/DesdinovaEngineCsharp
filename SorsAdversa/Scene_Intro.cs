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
    public class Scene_Intro : Scene
    {
        //Suono di fondo
        private Sound soundBackground;

        //Font dei crediti
        private Font_Typed credits;
        private int currentCredit = 0;

        //Cielo
        SkySphere sky;

        //Lens flare
        LensFlare flare1;

        //Pianeta
        XModel planet;

        //Luce
        LightEffect lightEffect;

        //Sprite
        private float titleAlpha = 0;
        private float engineAlpha = 0;
        private Sprite title;
        private Sprite engine;
        private SpriteBatcher spriteBatcher;

        //Effetto cine
        private MovieEffect cineEffectFade;
        private MovieEffect cineEffectMovie;

        public Scene_Intro(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            //Impostazioni camera
            base.SceneCamera.FarDistance = 10000.0f;
            base.SceneCamera.NearDistance = 1.0f;
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 30.0f;
            base.SceneCamera.PositionZ = 150.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 30.0f;
            base.SceneCamera.TargetZ = 0.0f;

            //Esegue la musica di background
            soundBackground = new Sound("Menu", false);
            soundBackground.ToLoop = true;
            soundBackground.Play();

            //Sfondo
            sky = new SkySphere("Content\\Texture\\Sky1", base.SceneContent);

            //Pianeta
            planet = new XModel(this);
            planet.Initialize("Content\\Model\\Jupiter", base.SceneContent);
            planet.ToDraw = true;
            planet.Position = new Vector3(0, 0, -600);
            planet.Scale = new Vector3(0.5f, 0.5f, 0.5f);

            //Effetto di luce
            lightEffect = new LightEffect(base.SceneContent, true, 2048, 0.008f, 500);
            lightEffect.AmbientColor = Color.Red;
            lightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(-1, 0, 0.5f), Color.White, Color.Black);

            //Lens flare
            flare1 = new LensFlare();
            flare1.OcclusionSize = 50.0f;
            flare1.AddFlare(new Flare(0.0f, new Vector2(1.0f, 1.0f), Color.Yellow, "Content\\Texture\\ray03"), base.SceneContent);
            flare1.AddFlare(new Flare(0.0f, new Vector2(2.0f, 2.0f), Color.White, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(-0.5f, new Vector2(0.7f, 0.7f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(0.3f, new Vector2(0.4f, 0.4f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(1.2f, new Vector2(1.0f, 1.0f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(1.5f, new Vector2(1.5f, 1.5f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(-0.3f, new Vector2(0.7f, 0.7f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(0.6f, new Vector2(0.9f, 0.9f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(-0.7f, new Vector2(0.7f, 0.7f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(0.2f, new Vector2(0.6f, 0.6f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
            flare1.AddFlare(new Flare(2.0f, new Vector2(1.4f, 1.4f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);

            //Fonts
            credits = new Font_Typed("Content\\Font\\Courier New", base.SceneContent);
            credits.ToDraw = true;
            credits.Color = Color.Green;
            credits.Scale = 1.0f;
            credits.CharactersSpacing = 5.0f;
            credits.AdditionalChar = '_';
            credits.Interval = 60.0f;
            credits.HideInterval = 3000;
            credits.Position = new Vector2(80, Core.Graphics.GraphicsDevice.Viewport.Height - 120);
            credits.OnFinishTyping += new EventHandler(credits_OnFinishTyping);

            //Titolo
            engine = new Sprite("Content\\Texture\\DesdinovaLogo", 1, base.SceneContent);
            engine.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (engine.Width / 2), 200);
            engine.ToDraw = false;
            engine.ScaleX = 0.75f;
            engine.ScaleY = 0.75f;
            title = new Sprite("Content\\Texture\\SorsAdversa", 1, base.SceneContent);
            title.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (title.Width / 2), 200);
            title.ToDraw = false;
            spriteBatcher = new SpriteBatcher();
            spriteBatcher.Add(engine);
            spriteBatcher.Add(title);

            //Effetto cine
            cineEffectFade = new MovieEffect(new Color(0, 0, 0, 255), false, false, Core.Graphics.GraphicsDevice.Viewport.Height / 2);
            cineEffectFade.Enable = true;
            cineEffectFade.PlayAnimation(0.5f, MovieEffect.AnimationType.FadeReverse);
            cineEffectMovie = new MovieEffect(new Color(0, 0, 0, 255), false, true, Core.Graphics.GraphicsDevice.Viewport.Height / 2);
            cineEffectMovie.Enable = true;




            //Creazione avvenuta
            return true;
        }

        void credits_OnFinishTyping(object sender, EventArgs e)
        {
            currentCredit = currentCredit + 1;
            credits.Reset();
        }


        public override void Update(GameTime gameTime)
        {
            //Luci
            lightEffect.Update(gameTime, base.SceneCamera);

            //Update suoni
            soundBackground.Update(gameTime, base.SceneCamera);

            //Cielo
            sky.Update(gameTime, base.SceneCamera);

            //Pianeta
            planet.Update(gameTime, base.SceneCamera);

            //Lens flare
            flare1.Update(gameTime, base.SceneCamera);

            //Font
            if (currentCredit == 0)
            {
                credits.Text = "Ferla 'Duff' Daniele";
            }
            else if (currentCredit == 1)
            {
                credits.Text = "and";
            }
            else if (currentCredit == 2)
            {
                credits.Text = "Antonio 'Mondo' Mondonico";
            }
            else if (currentCredit == 3)
            {
                credits.Text = "a game developed with";
            }
            else if (currentCredit == 4)
            {
                credits.ToDraw = false;

                engineAlpha = engineAlpha + 0.05f;
                if (engineAlpha > 255)
                {
                    currentCredit = 5;
                    credits.Reset();
                }
                engine.ToDraw = true;
                engine.Color = new Color(engine.Color.R, engine.Color.G, engine.Color.B, (byte)engineAlpha);
            }
            else if (currentCredit == 5)
            {
                engine.ToDraw = false;
                credits.Text = "presents";
            }
            else if (currentCredit == 6)
            {
                credits.ToDraw = false;

                titleAlpha = titleAlpha + 0.05f;
                if (titleAlpha > 255)
                {
                    SorsAdversa.menu = new Scene_Menu("Scene_Menu");
                    Core.SetCurrentScene(SorsAdversa.menu, true);
                    SorsAdversa.intro = null;
                    return;
                }
                title.ToDraw = true;
                title.Color = new Color(title.Color.R, title.Color.G, title.Color.B, (byte)titleAlpha);
            }

            credits.Update(gameTime);

            //Titolo
            engine.Update(gameTime);
            title.Update(gameTime);
            spriteBatcher.Update(gameTime);

            //Effetto cine
            cineEffectFade.Update(gameTime);
            cineEffectMovie.Update(gameTime);

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

            //Cielo
            sky.Draw(lightEffect);

            //Pianete
            planet.Draw(lightEffect);

            //Lens flare
            flare1.Draw();

            //Font
            credits.Draw();

            //Titolo
            engine.Draw();
            title.Draw();
            spriteBatcher.Draw(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, Matrix.Identity);

            //Effetto cine
            cineEffectFade.Draw();
            cineEffectMovie.Draw();
        }

        public override void Release()
        {
            //Rilascia le risorse
            soundBackground.Stop();
            soundBackground = null;
            credits = null;
            sky = null;
            flare1 = null;
            planet = null;
            lightEffect = null;
            title = null;
            engine = null;
            spriteBatcher = null;
            cineEffectFade = null;
            cineEffectMovie = null;
        }
    }
}

