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
using SorsAdversa;

namespace EditorEngine
{
    public class Scene_Main : Scene
    {
        //Griglia
        public static Grid grid;

        //Luci
        public static LightEffect lightEffect;

        //Pannello informazioni
        public static InfoPanel infoPanel;

        //Bounding
        public static bool debugShowBoundingSpheres = false;
        public static bool debugShowCollisionSpheres = false;
        public static bool debugShowNames = true;
        public static BoundingRenderer debugBounding;

        //Colore sfondo
        public static Color backgroundColor = Color.LightGray;

        //Giocatori
        public static Player player1;
        public static Player player2;

        //Nemici
        public static List<Enemy> enemyList = new List<Enemy>();

        //Powerup
        public static List<PowerUp> powerupList = new List<PowerUp>();

        //Posizione corrente
        public static float currentBarPosition = 0;

        public Scene_Main(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            base.SceneCamera.FarDistance = 1000.0f;
            base.SceneCamera.NearDistance = 1.0f;
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 30.0f;
            base.SceneCamera.PositionZ = 120.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 30.0f;
            base.SceneCamera.TargetZ = 0.0f;
            base.SceneCamera.UpX = 0.0f;
            base.SceneCamera.UpY = 1.0f;
            base.SceneCamera.UpZ = 0.0f;
            base.SceneCamera.RotationX = 0.0f;
            base.SceneCamera.RotationY = 0.0f;
            base.SceneCamera.RotationZ = 0.0f;

            //Griglia
            grid = new Grid(50, 50, Color.Black);

            //Bounding
            debugBounding = new BoundingRenderer(true, base.SceneContent);
            
            //Effetto di luce
            lightEffect = new LightEffect(base.SceneContent, true, 2048, 0.008f, 500);
            lightEffect.AmbientColor = new Color(32, 32, 32);
            lightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(-1, -1, -1), Color.White, Color.Black);

            //Pannello informazioni
            infoPanel = new InfoPanel();
            infoPanel.Position = new Vector2(5,55);
            infoPanel.Mode = InfoPanel.InfoPanelMode.Extended;

            //Players
            PlayerParameters def1 = new PlayerParameters();
            def1.Index = PlayerIndex.One;
            def1.Type = PlayerType.Zenith;
            def1.Name = "Player1";
            def1.Index = PlayerIndex.One;
            def1.StructurePoint = 3;
            def1.ShieldEnergyMax = 100.0f;
            def1.ShieldEnergy = 50.0f;
            def1.ShieldRegen = 150.0f;
            def1.Velocity = 40.0f;
            def1.FirePower = 5;
            def1.Score = 0;
            player1 = new Player(def1, base.SceneContent, this);
            player1.Position = new Vector3(-35, 40, 0);

            PlayerParameters def2 = new PlayerParameters();
            def2.Index = PlayerIndex.Two;
            def2.Type = PlayerType.Zenith;
            def2.Name = "Player2";
            def2.Index = PlayerIndex.One;
            def2.StructurePoint = 3;
            def2.ShieldEnergyMax = 100.0f;
            def2.ShieldEnergy = 50.0f;
            def2.ShieldRegen = 150.0f;
            def2.Velocity = 40.0f;
            def2.FirePower = 5;
            def2.Score = 0;
            player2 = new Player(def2, base.SceneContent, this);
            player2.Position = new Vector3(-35, 20, 0);

            //Pausa
            //Thread.Sleep(4000);

            //Creazione avvenuta
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            //Luci
            lightEffect.Update(gameTime, base.SceneCamera);

            //Posizione
            base.SceneCamera.PositionX = currentBarPosition;
            base.SceneCamera.TargetX = currentBarPosition;
            
            //Griglia
            grid.Update(gameTime, base.SceneCamera);

            //Players
            player1.Update(gameTime, base.SceneCamera);
            player2.Update(gameTime, base.SceneCamera);

            //Nemici
            foreach (Enemy currentEnemy in enemyList)
            {
                currentEnemy.Update(gameTime, base.SceneCamera);
            }

            //Powerup
            foreach (PowerUp currentPowerup in powerupList)
            {
                currentPowerup.Update(gameTime, base.SceneCamera);
            }

            //Pannello informazioni
            infoPanel.Update(gameTime);
        }


        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backgroundColor, 1.0f, 0);

            //Griglia
            grid.Draw();

            //Player1
            player1.Draw(lightEffect);
            if (debugShowBoundingSpheres)
            {
                debugBounding.Color = Color.Blue;
                debugBounding.ReferenceSphere = player1.BoundingSphere;
                debugBounding.Draw(lightEffect, base.SceneCamera);
            }
            if (debugShowCollisionSpheres)
            {
                for (int i = 0; i < player1.CollisionSpheres.Length; i++)
                {
                    debugBounding.Color = Color.White;
                    debugBounding.ReferenceSphere = player1.CollisionSpheres[i];
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }
            }
            if (debugShowNames)
            {
                base.SceneFont.Scale = 0.75f;
                base.SceneFont.Color = Color.Blue;
                base.SceneFont.Text = "Player 1";
                base.SceneFont.PositionX = player1.Position2D.X - (base.SceneFont.Width / 2.0f);
                base.SceneFont.PositionY = player1.Position2D.Y + 15;
                base.SceneFont.Draw();
            }

            //Player2
            player2.Draw(lightEffect);
            if (debugShowBoundingSpheres)
            {
                debugBounding.Color = Color.Blue;
                debugBounding.ReferenceSphere = player2.BoundingSphere;
                debugBounding.Draw(lightEffect, base.SceneCamera);
            }
            if (debugShowCollisionSpheres)
            {
                for (int i = 0; i < player2.CollisionSpheres.Length; i++)
                {
                    debugBounding.Color = Color.White;
                    debugBounding.ReferenceSphere = player2.CollisionSpheres[i];
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }
            }
            if (debugShowNames)
            {
                base.SceneFont.Scale = 0.75f;
                base.SceneFont.Color = Color.Blue;
                base.SceneFont.Text = "Player 2";
                base.SceneFont.PositionX = player2.Position2D.X - (base.SceneFont.Width / 2.0f);
                base.SceneFont.PositionY = player2.Position2D.Y + 15;
                base.SceneFont.Draw();           
            }


            //Nemici
            foreach (Enemy currentEnemy in enemyList)
            {
                currentEnemy.Draw(lightEffect);

                if (debugShowBoundingSpheres)
                {
                    debugBounding.Color = Color.Blue;
                    debugBounding.ReferenceSphere = currentEnemy.BoundingSphere;
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }
                if (debugShowCollisionSpheres)
                {
                    for (int i = 0; i < currentEnemy.CollisionSpheres.Length; i++)
                    {
                        debugBounding.Color = Color.White;
                        debugBounding.ReferenceSphere = currentEnemy.CollisionSpheres[i];
                        debugBounding.Draw(lightEffect, base.SceneCamera);
                    }
                }
                if (debugShowNames)
                {
                    base.SceneFont.Scale = 0.75f;
                    base.SceneFont.Color = Color.Red;
                    base.SceneFont.Text = (string)currentEnemy.Tag;
                    base.SceneFont.PositionX = currentEnemy.Position2D.X - (base.SceneFont.Width / 2.0f);
                    base.SceneFont.PositionY = currentEnemy.Position2D.Y + 15;
                    base.SceneFont.Draw();
                }
            }

            //Powerup
            foreach (PowerUp currentPowerup in powerupList)
            {
                currentPowerup.Draw(lightEffect);

                if (debugShowBoundingSpheres)
                {
                    debugBounding.Color = Color.Blue;
                    debugBounding.ReferenceSphere = currentPowerup.BoundingSphere;
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }
                if (debugShowCollisionSpheres)
                {
                    for (int i = 0; i < currentPowerup.CollisionSpheres.Length; i++)
                    {
                        debugBounding.Color = Color.White;
                        debugBounding.ReferenceSphere = currentPowerup.CollisionSpheres[i];
                        debugBounding.Draw(lightEffect, base.SceneCamera);
                    }
                }
                if (debugShowNames)
                {
                    base.SceneFont.Scale = 0.75f;
                    base.SceneFont.Color = Color.Red;
                    base.SceneFont.Text = (string)currentPowerup.Tag;
                    base.SceneFont.PositionX = currentPowerup.Position2D.X - (base.SceneFont.Width / 2.0f);
                    base.SceneFont.PositionY = currentPowerup.Position2D.Y + 15;
                    base.SceneFont.Draw();
                }
            }


            //Pannello informazioni
            infoPanel.Draw();

        }

        public override void Release()
        {

        }
    }
}

