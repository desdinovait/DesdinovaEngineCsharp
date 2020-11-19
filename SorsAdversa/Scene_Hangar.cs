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
    public class Scene_Hangar : Scene
    {
        //Modello
        Player mainModel;

        //Luci
        LightEffect lightEffect;

        public Scene_Hangar(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            //Camera
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 10.0f;
            base.SceneCamera.PositionY = 10.0f;
            base.SceneCamera.PositionZ = 10.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 0.0f;
            base.SceneCamera.TargetZ = 0.0f;

            //Player definitions
            PlayerParameters def1 = new PlayerParameters();
            def1.Index = PlayerIndex.One;
            def1.Type = PlayerType.Zenith;
            def1.Name = "Duff";
            def1.Index = PlayerIndex.One;
            def1.StructurePoint = 3;
            def1.ShieldEnergyMax = 100.0f;
            def1.ShieldEnergy = 50.0f;
            def1.ShieldRegen = 150.0f;
            def1.Velocity = 0.035f;
            def1.FirePower = 5;
            def1.Score = 0;

            //Modello
            mainModel = new Player(def1, base.SceneContent, this);
            mainModel.ToDraw = true;
            mainModel.Position = new Vector3(-5.5f, 2.5f, 0);

            //Effetto di luce
            lightEffect = new LightEffect(base.SceneContent, true, 2048, 0.008f, 500);
            lightEffect.AmbientColor = Color.Black;
            lightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(1, -1, -1), Color.White, Color.Black);
            lightEffect.PointLights[0] = new PointLightParameters(true, mainModel.Position, 10.0f, Color.Blue, Color.DarkGray);
            
            //Creazione avvenuta
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            //Luce
            lightEffect.Update(gameTime, base.SceneCamera);
            lightEffect.PointLights[0].Position = mainModel.engineAnchor1.FinalMatrix.Translation;

            //Modello
            mainModel.RotationY = mainModel.RotationY + 0.01f;
            mainModel.Update(gameTime, base.SceneCamera);
        }


        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            //LuUci
            //lightEffect.Draw(base.SceneCamera);

            //Modello
            mainModel.BlendProperties = BlendMode.AlphaBlend;
            mainModel.Draw(lightEffect);

            //Modello
            mainModel.BlendProperties = BlendMode.AlphaBlend;
            mainModel.Draw(lightEffect);
        }

        public override void Release()
        {
            //Rilascia le risorse
        }
    }
}

