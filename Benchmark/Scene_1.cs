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
    public class Scene_1 : Scene
    {
        InfoPanel infoPanel = null;
        Font font = null;
        MovieEffect cineEffect = null;
        PostProcessing_Pixelate postProcessing1 = null;
        PostProcessing_BlackAndWhite postProcessing2 = null;
        SkySphere sky = null;
        XModel model1 = null;
        XModel model2 = null;
        XModel model3 = null;
        XModel[] modelArray = null;
        SimpleModel modelSimple = null;
        Movie movie = null;
        AudioSound sound = null;
        LensFlare flare1 = null;

        int selectedLight = 0;

        public Scene_1(string sceneName) : base(sceneName)
        {

        }

        //Metodi da implementare
        public override bool Initialize()
        {           
            //Camera
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);
            base.SceneCamera.FarDistance = 1000.0f;
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 200.0f;
            base.SceneCamera.PositionZ = 200.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 0.0f;
            base.SceneCamera.TargetZ = 0.0f;

            //Effetto di luce
            base.SceneLightEffect.AmbientColor = new Color(25, 25, 25);
            base.SceneLightEffect.EnvironmentMap = base.SceneContent.Load<TextureCube>("Content\\texture\\SkySpace");
            base.SceneLightEffect.Fog = new FogParameters(false, 1, 500, 1, 1, Color.Black);
            base.SceneLightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(-1, -1, -1), Color.White, new Color(25, 25, 25));
            //base.SceneLightEffect.DirectionalLight1 = new DirectionalLightParameters(true, new Vector3(0, -1, 0), Color.White, new Color(25, 25, 25));
            //base.SceneLightEffect.DirectionalLight2 = new DirectionalLightParameters(true, new Vector3(1, 0, 0), Color.Green, new Color(25, 25, 25));
            //base.SceneLightEffect.PointLights[0] = new PointLightParameters(true, new Vector3(0, 50, 0), 75, Color.Red, Color.White);
            //base.SceneLightEffect.PointLights[1] = new PointLightParameters(true, new Vector3(50, 50, 0), 75, Color.Green, Color.White);
            //base.SceneLightEffect.PointLights[2] = new PointLightParameters(true, new Vector3(-50, 50, 0), 75, Color.Blue, Color.White);
            //base.SceneLightEffect.PointLights[3] = new PointLightParameters(true, new Vector3(0, 50, -50), 75, Color.Yellow, Color.White);
            base.SceneLightEffect.PointLights[0] = new PointLightParameters(true, new Vector3(-100, 50, 0), 100, Color.Red, Color.White);
            base.SceneLightEffect.PointLights[1] = new PointLightParameters(true, new Vector3(0, 50, 0), 75, Color.Green, Color.White);
            base.SceneLightEffect.PointLights[2] = new PointLightParameters(true, new Vector3(100, 50, 0), 75, Color.Blue, Color.White);
            base.SceneLightEffect.PointLights[3] = new PointLightParameters(true, new Vector3(0, 50, -100), 75, Color.Yellow, Color.White);//*/

            //Altro
            base.BackgroundColor = Color.Black;
            base.DrawMode = SceneDrawMode.All;
            base.SortObjects = true;
            base.DrawBoundings = true;

            //Lens flare
            flare1 = new LensFlare(this);
            flare1.OcclusionSize = 50.0f;
            flare1.AddFlare(new Flare(0.0f, new Vector2(2.0f, 2.0f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(0.0f, new Vector2(5.0f, 5.0f), Color.Orange, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(0.0f, new Vector2(2.0f, 2.0f), Color.White, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(-0.5f, new Vector2(0.7f, 0.7f), Color.Orange, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(0.3f, new Vector2(0.4f, 0.4f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(1.2f, new Vector2(1.0f, 1.0f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(1.5f, new Vector2(1.5f, 1.5f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(-0.3f, new Vector2(0.7f, 0.7f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(0.6f, new Vector2(0.9f, 0.9f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(-0.7f, new Vector2(0.7f, 0.7f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(0.2f, new Vector2(0.6f, 0.6f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.AddFlare(new Flare(2.0f, new Vector2(1.4f, 1.4f), Color.Yellow, "Content\\Texture\\Flare04"));
            flare1.LightDirection = Vector3.Normalize(new Vector3(0.1f, 0.2f, 1.0f));
            flare1.ToDraw = true;
            this.AddObject(flare1);

            //Font
            font = new Font(3, this);
            font[0] = new FontLine("Content\\Font\\Verdana", this);
            font[0].ShadowEnabled = true;
            font[0].ShadowColor = Color.Black;
            font[0].ShadowOffset = 1.0f;
            font[0].Color = Color.Red;
            font[0].Scale = 1.25f;
            font[0].Position = new Vector2(20, 20);

            font[1] = new FontLine("Content\\Font\\Verdana", this);
            font[1].ShadowEnabled = true;
            font[1].ShadowColor = Color.Black;
            font[1].ShadowOffset = 1.0f;
            font[1].Color = Color.Salmon;
            font[1].Scale = 1.0f;
            font[1].Position = new Vector2(20, 40);

            font[2] = new FontLine("Content\\Font\\Verdana", this);
            font[2].ShadowEnabled = true;
            font[2].ShadowColor = Color.Black;
            font[2].ShadowOffset = 1.0f;
            font[2].Color = Color.Salmon;
            font[2].Scale = 1.0f;
            font[2].Position = new Vector2(20, 60);

            base.AddObject(font);

            //Cielo
            sky = new SkySphere("Content\\Texture\\SkySpace", this);
            sky.Color = Color.White;
           // base.AddObject(sky);

            //Modelli
            model1 = new XModel(this);
            model1.Initialize("Content\\Model\\level6_b");
            model1.Position = new Vector3(0, 0, 0);
            model1.ToDraw = true;
            model1.Scale = new Vector3(1, 1, 1);
            model1.CastShadow = true;//*/
            base.AddObject(model1);

            model2 = new XModel(this);
            model2.Initialize("Content\\Model\\Cube");
            model2.Position = new Vector3(0, 60, 30);
            model2.ToDraw = true;
            model2.Scale = new Vector3(2, 2, 2);
            model2.CastShadow = true;//*/
            base.AddObject(model2);

            model3 = new XModel(this);
            model3.Initialize("Content\\Model\\Sphere");
            model3.Position = new Vector3(0, 40, 60);
            model3.ToDraw = true;
            model3.Scale = new Vector3(1, 1, 1);
            model3.CastShadow = true;//*/
            base.AddObject(model3);

            modelArray = new XModel[100];
            /*Random rnd = new Random();
            for (int i = 0; i < modelArray.Length; i++)
            { 
                modelArray[i] = new XModel(this);
                modelArray[i].Initialize("Content\\Model\\asteroids");
                modelArray[i].ToDraw = true;
                modelArray[i].Scale = new Vector3(1, 1, 1);
                modelArray[i].PositionX = rnd.Next(-500, 500);
                modelArray[i].PositionY = rnd.Next(-500, 500);
                modelArray[i].PositionZ = rnd.Next(-500, 500);
                base.AddObject(modelArray[i]);
            }//*/

            /*Simplemodel
            modelSimple = new SimpleModel("Content\\Model\\sphere", "Content\\Texture\\parallax_color_map", this);
            modelSimple.Position = new Vector3(0, 50, 0);
            modelSimple.DiffuseColor = Color.DarkBlue;
            modelSimple.TextureEnabled = true;
            modelSimple.FogEnabled = false;
            modelSimple.ToDraw = true;
            modelSimple.LightingEnabled = false;
            modelSimple.Scale = new Vector3(1, 1, 1);
            base.AddObject(modelSimple);//*/

            //Effetto cine
            cineEffect = new MovieEffect(new Color(0, 0, 0, 255), false, false, Core.Graphics.GraphicsDevice.Viewport.Height / 2, this);
            cineEffect.Enable = true;
            cineEffect.PlayAnimation(0.5f, MovieEffect.AnimationType.FadeReverse);
            base.AddObject(cineEffect);

            //Post processing
            postProcessing1 = new PostProcessing_Pixelate(128, 0.15f, Color.LightGray, this);
            postProcessing1.IsEnable = false;
            postProcessing2 = new PostProcessing_BlackAndWhite(this);
            postProcessing2.IsEnable = false;
            base.AddObject(postProcessing1);
            base.AddObject(postProcessing2);

            //Video
            movie = new Movie("Content\\Video\\Star", this);
            base.AddObject(movie);

            //Souno
            sound = new AudioSound("Content\\Fx\\Missile", this);
            base.AddObject(sound);

            //Informazioni
            infoPanel = new InfoPanel(this);
            infoPanel.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - infoPanel.Width - 10, 10);
            infoPanel.TextColor = Color.Yellow;
            infoPanel.BackgroundColor = new Color(0, 0, 128, 96);
            infoPanel.BorderColor = Color.Blue;
            infoPanel.Mode = InfoPanel.InfoPanelMode.Normal;
            base.AddObject(infoPanel);


            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Input
            if (base.SceneInput.IsKeyDown(Keys.F2))
            {
                Core.ToggleFullscreen();
            }
            if (base.SceneInput.IsKeyDown(Keys.NumPad0))
            {
                selectedLight = 0;
            }
            if (base.SceneInput.IsKeyDown(Keys.NumPad1))
            {
                selectedLight = 1;
            }
            if (base.SceneInput.IsKeyDown(Keys.NumPad2))
            {
                selectedLight = 2;
            }
            if (base.SceneInput.IsKeyDown(Keys.NumPad3))
            {
                selectedLight = 3;
            }

            if (base.SceneInput.IsKeyDown(Keys.W))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionZ = base.SceneLightEffect.PointLights[selectedLight].PositionZ - 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.S))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionZ = base.SceneLightEffect.PointLights[selectedLight].PositionZ + 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.A))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionX = base.SceneLightEffect.PointLights[selectedLight].PositionX - 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.D))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionX = base.SceneLightEffect.PointLights[selectedLight].PositionX + 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.Q))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionY = base.SceneLightEffect.PointLights[selectedLight].PositionY + 0.1f;
            }
            if (base.SceneInput.IsKeyDown(Keys.E))
            {
                base.SceneLightEffect.PointLights[selectedLight].PositionY = base.SceneLightEffect.PointLights[selectedLight].PositionY - 0.1f;
            }

            if (base.SceneInput.IsKeyDown(Keys.Up))
            {
                base.SceneCamera.PositionZ = base.SceneCamera.PositionZ - 0.25f;
                base.SceneCamera.TargetZ = base.SceneCamera.TargetZ - 0.25f;
            }
            if (base.SceneInput.IsKeyDown(Keys.Down))
            {
                base.SceneCamera.PositionZ = base.SceneCamera.PositionZ + 0.25f;
                base.SceneCamera.TargetZ = base.SceneCamera.TargetZ + 0.25f;
            }
            if (base.SceneInput.IsKeyDown(Keys.Left))
            {
                base.SceneCamera.PositionX = base.SceneCamera.PositionX - 0.25f;
                base.SceneCamera.TargetX = base.SceneCamera.TargetX - 0.25f;
            }
            if (base.SceneInput.IsKeyDown(Keys.Right))
            {
                base.SceneCamera.PositionX = base.SceneCamera.PositionX + 0.25f;
                base.SceneCamera.TargetX = base.SceneCamera.TargetX + 0.25f;
            }
            if (base.SceneInput.IsKeyDown(Keys.PageUp))
            {
                base.SceneCamera.PositionY = base.SceneCamera.PositionY + 0.25f;
            }
            if (base.SceneInput.IsKeyDown(Keys.PageDown))
            {
                base.SceneCamera.PositionY = base.SceneCamera.PositionY - 0.25f;
            }

            if (base.SceneInput.IsKeyDown(Keys.R))
            {
                base.SceneLightEffect.Fog = new FogParameters(true, base.SceneLightEffect.Fog.Start, base.SceneLightEffect.Fog.End + 0.1f, base.SceneLightEffect.Fog.Altitude, base.SceneLightEffect.Fog.Thinning, base.SceneLightEffect.Fog.Color);
            }
            if (base.SceneInput.IsKeyDown(Keys.F))
            {
                base.SceneLightEffect.Fog = new FogParameters(true, base.SceneLightEffect.Fog.Start, base.SceneLightEffect.Fog.End - 0.1f, base.SceneLightEffect.Fog.Altitude, base.SceneLightEffect.Fog.Thinning, base.SceneLightEffect.Fog.Color);
            }

            if (base.SceneInput.IsKeyDown(Keys.V))
            { 
                Core.CaptureScreenshot("Benchmark", true);
            }

            if (base.SceneInput.IsKeyDown(Keys.P))
            {
                base.SortObjects = !base.SortObjects;
            }

            if (base.SceneInput.IsKeyDown(Keys.Space))
            {
                sound.Play();
            }

            if (base.SceneInput.IsKeyDown(Keys.Escape))
            {
                Benchmark.menu = new Scene_Menu("menu");
                Core.SetCurrentScene(Benchmark.menu, true);
                Benchmark.scene1 = null;
                return;
            }

            //Modelli
            //model2.RotationX = model2.RotationX + 0.1f;
            //model2.RotationZ = model2.RotationZ + 0.1f;

            //Scritte
            font[0].Text = "SCENE 1";
            font[1].Text = "Dynamic lights system";
            font[2].Text = "3D: " + this.Count3DObjects.ToString() + " - 2D: " + this.Count2DObjects.ToString();
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Release()
        {
            base.RemoveObject(sound);
            base.RemoveObject(movie);
            base.RemoveObject(postProcessing1);
            base.RemoveObject(postProcessing2);
            base.RemoveObject(cineEffect);
            base.RemoveObject(modelSimple);
            base.RemoveObject(model3);
            base.RemoveObject(model2);
            base.RemoveObject(model1);
            for (int i = 0; i < modelArray.Length; i++)
                base.AddObject(modelArray[i]);
            base.RemoveObject(sky);
            base.RemoveObject(font);
            base.RemoveObject(infoPanel);

            sound = null;
            movie = null;
            postProcessing1 = null;
            postProcessing2 = null;
            cineEffect = null;
            modelSimple = null;
            model3 = null;
            model2 = null;
            model1 = null;
            for (int i = 0; i < modelArray.Length; i++)
                modelArray[i] = null;
            sky = null;
            font = null;
            infoPanel = null;
        }

    }
}
