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
    public class Scene_Test : Scene
    {
        XModel model1;
        XModel model2;
        XModel weapon;
        XModelAnchor anchor1;
        XModelAnchor anchor2;

        LightEffect lightEffect;

        List<XModel> prova = new List<XModel>();

        public Scene_Test(string sceneName) : base(sceneName)
        {

        }

        //Metodi da implementare
        public override bool Initialize()
        {
            //Camera
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 10.0f;
            base.SceneCamera.PositionZ = 10.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 0.0f;
            base.SceneCamera.TargetZ = 0.0f;

            //Prova multipla
            for (int i=0; i<100; i++)
            {
                XModel provaModel = new XModel(this);
                provaModel.Initialize("Content\\Model\\zenith", base.SceneContent);
                provaModel.ToDraw = true;
                provaModel.Position = new Vector3(0, 0, -(i * 0.5f));
                prova.Add(provaModel);
            }

            //Modello
            weapon = new XModel(this);
            weapon.Initialize("Content\\Model\\weapon", base.SceneContent);

            model1 = new XModel(this);
            model1.Initialize("Content\\Model\\knife", base.SceneContent);

            model2 = new XModel(this);
            model2.Initialize("Content\\Model\\knife", base.SceneContent);

            anchor1 = new XModelAnchor(model1, "WeaponAnchor1");
            anchor2 = new XModelAnchor(model2, "WeaponAnchor1");


            //Effetto di luce
            lightEffect = new LightEffect(base.SceneContent, true, 2048, 0.008f, 500);
            lightEffect.AmbientColor = Color.Black;
            lightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(1, -1, -1), Color.White, Color.Black);


            return true;
        }

        public override void Update(GameTime gameTime)
        {
            lightEffect.Update(gameTime, base.SceneCamera);

            if (base.SceneInput.IsKeyDown(Keys.D))
            {
                model1.PositionX = model1.PositionX + 0.01f;
            }
            if (base.SceneInput.IsKeyDown(Keys.A))
            {
                model1.PositionX = model1.PositionX - 0.01f;
            }
            if (base.SceneInput.IsKeyDown(Keys.W))
            {
               model1.PositionY = model1.PositionY + 0.01f;
            } 
            if (base.SceneInput.IsKeyDown(Keys.S))
            {
                model1.PositionY = model1.PositionY - 0.01f;
            }
            if (base.SceneInput.IsKeyDown(Keys.F))
            {
                model1.RotationX = model1.RotationX - 0.1f;
            }            
            if (base.SceneInput.IsKeyDown(Keys.G))
            {
                model2.RotationY = model2.RotationY - 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.H))
            {
                anchor2.RotationY = anchor2.RotationY + 0.5f;
            }
            if (base.SceneInput.IsKeyDown(Keys.I))
            {
                anchor2.RotationZ = anchor2.RotationZ + 0.5f;
            }

            weapon.PositionY = 2;
            anchor2.RotationX = anchor2.RotationX + 0.1f;

            model1.ExternalMatrix = Matrix.Identity;        //Ancorato a niente :-)
            model2.ExternalMatrix = anchor1.FinalMatrix;    //Ancorato al modello 1
            weapon.ExternalMatrix = anchor2.FinalMatrix;    //Ancorato al modello 2 (che è ancorato al modello 1)

            //model1
            //    |
            //    --- model2
            //            |
            //            --- weapon


            model1.Update(gameTime, base.SceneCamera);
            model2.Update(gameTime, base.SceneCamera);
            weapon.Update(gameTime, base.SceneCamera);

            //Prova multipla
            foreach (XModel model in prova)
            {
                model.Update(gameTime, base.SceneCamera);
            }
        }

        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkBlue, 1.0f, 0);

            model1.Draw(lightEffect);
            model2.Draw(lightEffect);
            weapon.Draw(lightEffect);

            //Prova multipla
            foreach (XModel model in prova)
            {
                model.Draw(lightEffect);
            }
        }


        public override void Release()
        {
        }

    }
}
