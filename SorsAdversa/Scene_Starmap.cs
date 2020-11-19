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
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;
using System.Threading;
using System.Reflection;

namespace SorsAdversa
{
    public class Scene_Starmap : Scene
    {
        //Luce
        LightEffect lightEffect;

        Starmap starmap;

        public override bool Initialize(string filename)
        {
            //Impostazioni camera
            base.SceneCamera.FarDistance = 1000.0f;
            base.SceneCamera.NearDistance = 1.0f;
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 30.0f;
            base.SceneCamera.PositionZ = 150.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 30.0f;
            base.SceneCamera.TargetZ = 0.0f;

            //Effetto di luce
            lightEffect = new LightEffect();
            lightEffect.Camera = base.SceneCamera;

            //Mappa
            starmap = new Starmap(50, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Sole", new Vector3(10, 10, 10), 15, Color.Yellow, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Terra", new Vector3(30, 30, 0), 8, Color.Orange, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Giove", new Vector3(-30, -30, -30), 10, Color.OrangeRed, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Pluto", new Vector3(30, 30, -200), 15, Color.Red, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Luna", new Vector3(-70, 70, -100), 15, Color.Yellow, base.SceneContent);
            starmap.Add("Content\\Texture\\Flare04", "Io", new Vector3(70, 90, -80), 15, Color.Yellow, base.SceneContent);
            starmap.Connect("Sole", "Terra", Color.Red, Color.Red);
            starmap.Connect("Sole", "Giove", Color.Red, Color.Red);
            starmap.Connect("Sole", "Pluto", Color.Red, Color.Red);
            starmap.Connect("Luna", "Pluto", Color.Red, Color.Red);
            starmap.Connect("Io", "Pluto", Color.GreenYellow, Color.GreenYellow);

            //Creazione avvenuta
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            //Camera
            if (base.SceneInput.IsKeyDown(Keys.Down)) base.SceneCamera.PositionZ = base.SceneCamera.PositionZ + 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Up)) base.SceneCamera.PositionZ = base.SceneCamera.PositionZ - 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Left)) base.SceneCamera.PositionX = base.SceneCamera.PositionX - 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Right)) base.SceneCamera.PositionX = base.SceneCamera.PositionX + 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.PageUp)) base.SceneCamera.PositionY = base.SceneCamera.PositionY - 0.25f;
            if (base.SceneInput.IsKeyDown(Keys.PageDown)) base.SceneCamera.PositionY = base.SceneCamera.PositionY + 0.25f;

            //Mappa
            starmap.Update(gameTime, base.SceneCamera);

        }


        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, new Color(0,0,50), 1.0f, 0);

            //Mappa
            starmap.Draw(lightEffect);

        }

        public override void Release()
        {

        }
    }
}

