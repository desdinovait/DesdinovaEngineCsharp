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
using System.Xml;
using System.Reflection;
using System.IO;

namespace SorsAdversa
{
    public enum InGameSelection
    {
        Continue,
        Restart,
        Exit
    }

    public enum GameMode
    {
        Game,
        Menu
    }

    public enum GameScrolling
    {
        Horizontal,     //R-Type
        Vertical,       //Ikaruga
        Rail            //Panzer Dragoon
    }

    public enum SpacelineElementType
    {
        Instance = 0,
        Content = 1,
        Camera = 2
    }

    public struct SpacelineElement
    {
        //Posizione nel livello
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
        }

        //Tipo di caricamento (istanza, camera, ecc)
        private SpacelineElementType type;
        public SpacelineElementType Type
        {
            get { return type; }
        }

        //Nome della classe
        private string elementName;
        public string ElementName
        {
            get { return elementName; }
        }

        public SpacelineElement(SpacelineElementType newType, string newElementName, Vector3 newPosition)
        {
            //Tipo
            this.type = newType;

            //Nome
            this.elementName = newElementName;

            //Posizione
            this.position = newPosition;
        }
    }

    public struct Section
    {
        //Nome
        private string elementName;
        public string ElementName
        {
            get { return elementName; }
        }       
        
        //Posizione nel livello
        private float positionY;
        public float PositionY
        {
            get { return positionY; }
        }
        private float positionZ;
        public float PositionZ
        {
            get { return positionZ; }
        }

        //Ripetizioni
        private int repetitions;
        public int Repetitions
	    {
            get { return repetitions; }
	    }
	 
        
        public Section(string sectionName, float sectionPosY, float sectionPosZ, int sectionRepetitions)
        {
            //Tipo
            this.elementName = sectionName;

            //Posizione
            this.positionY = sectionPosY;
            this.positionZ = sectionPosZ;

            //Ripetizioni
            this.repetitions = sectionRepetitions;
        }
    }

    public class Scene_Level : Scene
    {
        //File da caricare
        private string filename;
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        //Pannello informazioni
        private InfoPanel infoPanel = null;

        //Debug mode
        private Font debugFont;
        private BoundingRenderer debugBounding;
        private bool debugMode;

        //Suono di fondo
        private Sound soundBackground;

        //Giocatore 1 e 2
        private Player player1;
        private Player player2;

        //Sfondo
        private SkySphere sky;

        //Lens Flare
        private LensFlare flare1;

        //Nemici
        public static List<Enemy> listEnemy;

        //Nemici
        public static List<PowerUp> listPowerup;

        //Elementi scenografici
        public static List<LevelElement> listLevelElement;

        //Boss finale
        //Boss boss;

        //Lista proiettili attivi
        public static BulletList bulletListByPlayers;
        public static BulletList bulletListByEnemies;

        //Luce
        private LightEffect lightEffect;
        //private FogParameters fogParameters;

        //Effetto cine
        private MovieEffect cineEffect;
        
        //Post processingù
        private PostProcessing_Blur postProcessingBlur;
        private PostProcessing_Color postProcessingColor;

        //InGame Menu
        private GameMode gameMode = GameMode.Game;
        private InGameSelection menuVoiceSelection = InGameSelection.Continue;
        private Font menuVoice1;
        private Font menuVoice2;
        private Font menuVoice3;

        //HUD
        //Font di livello
        private Font_Typed hudLevelTitle;        //Numero livello
        private Font_Typed hudLevelSubtitle;     //Nome livello
        private Font hudScore;                   //Punteggio
        private Font hudScoreCaption;            //Scritta "SCORE"
        private Sprite hudScoreBorder;           //Immagine bordo punteggio
        private SpriteBatcher spriteBatcher;     //Batcher

        //Tipo di scrolling
        public static GameScrolling gameScrolling = GameScrolling.Horizontal;

        //Superficie del livello
        private Surface surface;

        //Velocità del livello
        private Velocity velocity;

        //Colore sfondo
        private Color backgroundColor;

        //Nebula
        private Nebula nebula;


        //Altro
        bool demoRunning = false;


        public Scene_Level(string sceneName): base(sceneName)
        {

        }

        public override bool Initialize()
        {
            //File da caricare
            string fileToLoad = filename;
            //...

            //Impostazioni camera
            base.SceneCamera.FarDistance = 5000.0f;    //Troppo?
            base.SceneCamera.NearDistance = 0.1f;
            base.SceneCamera.AspectRatio = (float)Core.Graphics.GraphicsDevice.Viewport.Width / (float)Core.Graphics.GraphicsDevice.Viewport.Height;
            base.SceneCamera.FieldOfView = MathHelper.ToRadians(45.0f);

            //base.SceneCamera.PositionX = -130.0f;
            //base.SceneCamera.PositionX = 800.0f;
            base.SceneCamera.PositionX = 0.0f;
            base.SceneCamera.PositionY = 30.0f;
            base.SceneCamera.PositionZ = 90.0f;
            base.SceneCamera.TargetX = 0.0f;
            base.SceneCamera.TargetY = 30.0f;
            base.SceneCamera.TargetZ = 0.0f;
            base.SceneCamera.UpX = 0.0f;
            base.SceneCamera.UpY = 1.0f;
            base.SceneCamera.UpZ = 0.0f;
            base.SceneCamera.RotationX = 0.0f;
            base.SceneCamera.RotationY = 0.0f;
            base.SceneCamera.RotationZ = 0.0f;
            base.SceneCamera.OnShakeFinish += new EventHandler(SceneCamera_OnShakeFinish);

            //Debug Renderer
            debugBounding = new BoundingRenderer(true, base.SceneContent);

            //Esegue la musica di background
            soundBackground = new Sound("GameMusic", false);
            soundBackground.ToLoop = true;
            //soundBackground.Play();

            //HUD
            //Font di debug
            debugFont = new Font("Content\\Font\\Courier New", base.SceneContent);
            //Font livello (scritta)
            hudLevelTitle = new Font_Typed("Content\\Font\\Courier New", base.SceneContent);
            hudLevelTitle.ToDraw = true;
            hudLevelTitle.Text = "Level 1";
            hudLevelTitle.AdditionalChar = '_';
            hudLevelTitle.Interval = 100.0f;
            hudLevelTitle.Color = Color.LightGreen;
            hudLevelTitle.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (hudLevelTitle.Width / 2), Core.Graphics.GraphicsDevice.Viewport.Height - (Core.Graphics.GraphicsDevice.Viewport.Height / 4.0f));
            hudLevelSubtitle = new Font_Typed("Content\\Font\\Courier New", base.SceneContent);
            hudLevelSubtitle.ToDraw = false;
            hudLevelSubtitle.Text = "Ruins of Arkanai";
            hudLevelSubtitle.AdditionalChar = '_';
            hudLevelSubtitle.Interval = 100.0f;
            hudLevelSubtitle.Color = Color.LimeGreen;
            hudLevelSubtitle.Scale = 1.35f;
            hudLevelSubtitle.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (hudLevelSubtitle.Width / 2), (Core.Graphics.GraphicsDevice.Viewport.Height - (Core.Graphics.GraphicsDevice.Viewport.Height / 4.0f)) + 30);
            hudLevelTitle.HideInterval = hudLevelSubtitle.Interval * hudLevelSubtitle.Text.Length + hudLevelSubtitle.HideInterval;//Il tempo di durata del titolo è uguale al tempo di scrittura * caratteri da scrivere + il tempo di durata della scritta
            //Score
            hudScoreCaption = new Font("Content\\Font\\Courier New", base.SceneContent);
            hudScoreCaption.Text = "SCORE";
            hudScoreCaption.PositionX = 8;
            hudScoreCaption.PositionY = 8;
            hudScore = new Font("Content\\Font\\Courier New", base.SceneContent);
            hudScore.Text = "0";
            hudScore.PositionX = 110;
            hudScore.PositionY = 14;
            hudScoreBorder = new Sprite("Content\\Texture\\ScoreBorder", 1, base.SceneContent);

            //Giocatore 1
            PlayerParameters def1 = new PlayerParameters();
            def1.Index = PlayerIndex.One;
            def1.Type = PlayerType.Zenith;
            def1.Name = "Duff";
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

            //Giocatore 2
            PlayerParameters def2 = new PlayerParameters();
            def1.Index = PlayerIndex.Two;
            def2.Type = PlayerType.Knife;
            def2.Name = "Mondo";
            def2.Index = PlayerIndex.Two;
            def2.StructurePoint = 3;
            def2.ShieldEnergyMax = 120.0f;
            def2.ShieldEnergy = 20.0f;
            def2.ShieldRegen = 500.0f;
            def2.Velocity = 40.0f;
            def2.FirePower = 5;
            def2.Score = 0;
            player2 = new Player(def2, base.SceneContent, this);
            player2.Position = new Vector3(-35, 20, 0);

            //Lens flare
            flare1 = new LensFlare();
            flare1.OcclusionSize = 50.0f;
               flare1.AddFlare(new Flare(0.0f, new Vector2(1.0f, 1.0f), Color.Yellow, "Content\\Texture\\Flare04"), base.SceneContent);
               flare1.AddFlare(new Flare(0.0f, new Vector2(5.0f, 5.0f), Color.Orange, "Content\\Texture\\Flare04"), base.SceneContent);
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
            flare1.LightDirection = Vector3.Normalize(new Vector3(-0.4f, -0.4f, 1.0f));
            flare1.ToDraw = true;

            //Lista Nemici
            listEnemy = new List<Enemy>();

            //Power Ups
            listPowerup = new List<PowerUp>();

            //Elementi scenografici
            listLevelElement = new List<LevelElement>();

            //Liste proiettili sparati dai giocatori e nemici
            bulletListByPlayers = new BulletList(400);
            bulletListByEnemies = new BulletList(listEnemy.Count * 50);  //Si ipotizza che ogni nemico sparerà 50 bullet

            //Sfondo
            sky = new SkySphere("Content\\Texture\\SkySpace", base.SceneContent);
            sky.ToDraw = true;
            sky.Color = Color.OrangeRed;

            //Colore di sfondo
            backgroundColor = Color.BurlyWood;

            //Effetto di luce
            lightEffect = new LightEffect(base.SceneContent, true, 2048, 0.008f, 600);
            lightEffect.Fog = new FogParameters(true, 10, 250, 1, 1, Color.BurlyWood);
            lightEffect.AmbientColor = new Color(24, 24, 24);
            lightEffect.EnvironmentMap = sky.CubeTexture;
            lightEffect.DirectionalLight0 = new DirectionalLightParameters(true, new Vector3(-1, -1, -1), Color.White, new Color(200,200,200));
            //lightEffect.DirectionalLight1 = new DirectionalLightParameters(true, new Vector3(-1, -1, 1), Color.Yellow, new Color(255, 255, 255));
            //lightEffect.DirectionalLight2 = new DirectionalLightParameters(true, new Vector3(1, -1, 0), Color.BlueViolet, new Color(255, 255, 255));
            lightEffect.PointLights[0] = new PointLightParameters(true, new Vector3(100, 0, 0), 100, Color.Green, new Color(75,75,75));
            lightEffect.PointLights[1] = new PointLightParameters(true, new Vector3(0, 50, 50), 100, Color.Red, new Color(75, 75, 75));
            //lightEffect.PointLights[2] = new PointLightParameters(true, new Vector3(50, 50, 0), 300, Color.Orange, new Color(75,75,75));
            //lightEffect.PointLights[3] = new PointLightParameters(true, new Vector3(300, 100, 0), 200, Color.Yellow, new Color(75,75,75));
            lightEffect.PointLights[4] = new PointLightParameters(true, new Vector3(650, 30, 0), 300, Color.Violet, new Color(75, 75, 75));

            //Effetto cine
            cineEffect = new MovieEffect(new Color(0, 0, 0, 255), false, false, Core.Graphics.GraphicsDevice.Viewport.Height / 2);
            cineEffect.Enable = true;
            cineEffect.PlayAnimation(1.0f, MovieEffect.AnimationType.FadeReverse);

            //Post processing
            postProcessingBlur = new PostProcessing_Blur(new Vector2(0.001f, 0.001f), base.SceneContent);
            postProcessingBlur.IsEnable = false;
            postProcessingColor = new PostProcessing_Color(new Vector3(1.2f, 0.5f, 0.5f), base.SceneContent);
            postProcessingColor.IsEnable = false;
            
            //InGame Menu 
            menuVoice1 = new Font("Content\\Font\\Starcraft", base.SceneContent);
            menuVoice1.Text = "Continue";
            menuVoice1.Scale = 1.5f;
            menuVoice1.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (menuVoice1.Width / 2) , 200);
            menuVoice2 = new Font("Content\\Font\\Starcraft", base.SceneContent);
            menuVoice2.Text = "Restart";
            menuVoice2.Scale = 1.5f;
            menuVoice2.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (menuVoice2.Width / 2), 250);
            menuVoice3 = new Font("Content\\Font\\Starcraft", base.SceneContent);
            menuVoice3.Text = "Exit";
            menuVoice3.Scale = 1.5f;
            menuVoice3.Position = new Vector2((Core.Graphics.GraphicsDevice.Viewport.Width / 2) - (menuVoice3.Width / 2), 300);

            //Batcher
            spriteBatcher = new SpriteBatcher();
            spriteBatcher.Add(hudScoreBorder);
            
            //Pannello informazioni
            infoPanel = new InfoPanel();
            infoPanel.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - infoPanel.Width - 10, 10);
            infoPanel.TextColor = Color.Gold;
            infoPanel.BackgroundColor = new Color(0, 0, 128, 96);
            infoPanel.BorderColor = Color.Blue;
            infoPanel.Mode = InfoPanel.InfoPanelMode.Minimal;

            //PowerUps
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "PowerUp_AlphaStrike",   new Vector3(70, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "PowerUp_DamageBooster", new Vector3(90, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "PowerUp_RegenerationBooster", new Vector3(110, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "PowerUp_ShieldBooster", new Vector3(130, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "PowerUp_SpeedBooster", new Vector3(150, 30, 0)));

            //Nemici
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(250, 55, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(250, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(250, 10, 0)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy2", new Vector3(500, 55, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy2", new Vector3(500, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy2", new Vector3(500, 10, 0)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy3", new Vector3(600, 50, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy3", new Vector3(600, 32, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy3", new Vector3(600, 10, 0)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(1050, 60, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(1100, 30, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Instance, "Enemy_Enemy1", new Vector3(1050, 10, 0)));


            //Elementi scenografici (prelevati dal content)
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Icarus", new Vector3(20, 60, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Icarus", new Vector3(0, 0, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Icarus", new Vector3(20, 75, -260)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Icarus", new Vector3(0, 0, -150)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\IcarusStation", new Vector3(200, 90, -700)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Jupiter", new Vector3(500, 90, -800)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Hole", new Vector3(150, 35, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Hole", new Vector3(200, 35, 0)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\Hole", new Vector3(250, 35, 0)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(60, 25, 15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(60, 25, -15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(80, 25, 15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(80, 25, -15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(100, 25, 15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(100, 25, -15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(120, 25, 15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(120, 25, -15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(140, 25, 15)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\BayLight", new Vector3(140, 25, -15)));

            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\IcarusStation", new Vector3(700, 50, 50)));
            this.AddElement(new SpacelineElement(SpacelineElementType.Content, "Content\\Model\\Element\\IcarusStation", new Vector3(1000, 40, -60)));//*/

            /*Camera trigger ( DA FARE )
            timeLine.AddElement(3, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, 20));
            timeLine.AddElement(5, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, -20));
            timeLine.AddElement(10, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, 20));
            timeLine.AddElement(10, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, -20));
            timeLine.AddElement(15, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, 20));
            timeLine.AddElement(15, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, -20));
            timeLine.AddElement(20, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, 20));
            timeLine.AddElement(20, new SpacelineElement(SpacelineElementType.Camera, "", 0, 20, -20));
            timeLine.AddElement(25, new SpacelineElement(SpacelineElementType.Camera, "", 0, 30, 0));//*/

            //Superficie
            surface = new Surface(false, 150, base.SceneContent);
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_A", 0.0f, 0.0f, 3));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_D", 0.0f, 0.0f, 1));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_B", 0.0f, 0.0f, 1));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_D", 0.0f, 0.0f, 1));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_A", 0.0f, 0.0f, 2));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_B", 0.0f, 0.0f, 1));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_C", 0.0f, 0.0f, 1));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_A", 0.0f, 0.0f, 4));
            surface.AddSection(new Section("Content\\Model\\Surface\\Level6_E", 0.0f, 0.0f, 3));//*/

            //Velocità
            velocity = new Velocity(new Vector3(1, 0, 0), 20.0f);

            //Nebula
            nebula = new Nebula(base.SceneContent);
            nebula.Position = new Vector3(20, 10, -50);

            /*Prova XML
            Save("Data\\Level1.xml");
            Load("Data\\Level1.xml");
            Core.Exit();*/

            //Attesa
            //Thread.Sleep(10000);

            //Caricamento avvenuto
            return true;
        }


        public void UpdateInput(GameTime gameTime)
        {
            //Update tasti generale
            base.SceneInput.Update(gameTime);

            //Selezione Debug mode
            if (base.SceneInput.IsKeyUp(Keys.F1)) debugMode = !debugMode;

            //Fullscreen
            if (base.SceneInput.IsKeyDown(Keys.F2)) Core.ToggleFullscreen();

            //Camera
            if (base.SceneInput.IsKeyDown(Keys.Down)) base.SceneCamera.PositionZ = base.SceneCamera.PositionZ + 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Up)) base.SceneCamera.PositionZ = base.SceneCamera.PositionZ - 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Left)) base.SceneCamera.PositionX = base.SceneCamera.PositionX - 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.Right)) base.SceneCamera.PositionX = base.SceneCamera.PositionX + 0.5f;
            if (base.SceneInput.IsKeyDown(Keys.PageUp)) base.SceneCamera.PositionY = base.SceneCamera.PositionY - 0.25f;
            if (base.SceneInput.IsKeyDown(Keys.PageDown)) base.SceneCamera.PositionY = base.SceneCamera.PositionY + 0.25f;

            //Input Player 1
            if (gameScrolling == GameScrolling.Horizontal)
            {
                if (base.SceneInput.IsKeyDown(Keys.A)) player1.MoveNegativeX();
                if (base.SceneInput.IsKeyDown(Keys.D)) player1.MovePositiveX();
                if (base.SceneInput.IsKeyDown(Keys.W)) player1.MovePositiveY();
                if (base.SceneInput.IsKeyDown(Keys.S)) player1.MoveNegativeY();
            }
            else if (gameScrolling == GameScrolling.Vertical)
            {
                if (base.SceneInput.IsKeyDown(Keys.A)) player1.MoveNegativeZ();
                if (base.SceneInput.IsKeyDown(Keys.D)) player1.MovePositiveZ();
                if (base.SceneInput.IsKeyDown(Keys.W)) player1.MovePositiveX();
                if (base.SceneInput.IsKeyDown(Keys.S)) player1.MoveNegativeX();
            }
            else if (gameScrolling == GameScrolling.Rail)
            {
                if (base.SceneInput.IsKeyDown(Keys.A)) player1.MoveNegativeZ();
                if (base.SceneInput.IsKeyDown(Keys.D)) player1.MovePositiveZ();
                if (base.SceneInput.IsKeyDown(Keys.W)) player1.MovePositiveY();
                if (base.SceneInput.IsKeyDown(Keys.S)) player1.MoveNegativeY();
            }

            if (base.SceneInput.IsKeyDown(Keys.E))
            {
                player1.WeaponCurrent = 0;
                if (player1.Fire(base.SceneContent))
                {
                    //Shake camera
                    base.SceneCamera.Shake(1500, 1, 1);
                    postProcessingBlur.IsEnable = true;
                }
            }
            if (base.SceneInput.IsKeyDown(Keys.R))
            {
                player1.WeaponCurrent = 1;
                if (player1.Fire(base.SceneContent))
                {
                    //Shake camera
                    base.SceneCamera.Shake(1500, 1, 1);
                    postProcessingBlur.IsEnable = true;
                }
            }
            if (base.SceneInput.IsKeyDown(Keys.T))
            {
                player1.WeaponCurrent = 2;
                if (player1.Fire(base.SceneContent))
                {
                    //Shake camera
                    base.SceneCamera.Shake(1500, 1, 1);
                    postProcessingBlur.IsEnable = true;
                }
            }

            if (base.SceneInput.IsKeyUp(Keys.E))
            {
                player1.ResetFire();
            }
            if (base.SceneInput.IsKeyUp(Keys.R))
            {
                player1.ResetFire();
            }
            if (base.SceneInput.IsKeyUp(Keys.T))
            {
                player1.ResetFire();
            }

            if (base.SceneInput.IsKeyDown(Keys.Q))
            {
                player1.RotationX = player1.RotationX + 0.1f;
                player1.RotationY = player1.RotationY + 0.25f;
                player1.RotationZ = player1.RotationZ - 0.1f;
            }

            //Input Player2
            if (gameScrolling == GameScrolling.Horizontal)
            {
                if (base.SceneInput.IsKeyDown(Keys.J)) player2.MoveNegativeX();
                if (base.SceneInput.IsKeyDown(Keys.L)) player2.MovePositiveX();
                if (base.SceneInput.IsKeyDown(Keys.I)) player2.MovePositiveY();
                if (base.SceneInput.IsKeyDown(Keys.K)) player2.MoveNegativeY();
            }
            else if (gameScrolling == GameScrolling.Vertical)
            {
                if (base.SceneInput.IsKeyDown(Keys.J)) player2.MoveNegativeZ();
                if (base.SceneInput.IsKeyDown(Keys.L)) player2.MovePositiveZ();
                if (base.SceneInput.IsKeyDown(Keys.I)) player2.MovePositiveX();
                if (base.SceneInput.IsKeyDown(Keys.K)) player2.MoveNegativeX();
            }
            else if (gameScrolling == GameScrolling.Rail)
            {
                if (base.SceneInput.IsKeyDown(Keys.J)) player2.MoveNegativeZ();
                if (base.SceneInput.IsKeyDown(Keys.L)) player2.MovePositiveZ();
                if (base.SceneInput.IsKeyDown(Keys.I)) player2.MovePositiveY();
                if (base.SceneInput.IsKeyDown(Keys.K)) player2.MoveNegativeY();
            }

            if (base.SceneInput.IsKeyDown(Keys.O))
            {
                player2.WeaponCurrent = 0;
                player2.Fire(base.SceneContent);
            }

            //Controller analogico
            //if (base.SceneInput.GetGamecontrollerState(0).Left == ButtonState.Pressed) player2.MoveBackward();
            //if (base.SceneInput.GetGamecontrollerState(0).Right == ButtonState.Pressed) player2.MoveForward();
            //if (base.SceneInput.GetGamecontrollerState(0).Up == ButtonState.Pressed) player2.MoveUp();
            //if (base.SceneInput.GetGamecontrollerState(0).Down == ButtonState.Pressed) player2.MoveDown();
            //if (base.SceneInput.GetGamecontrollerState(0).AnalogLeft > 0.0f) player2.MoveBackward(base.SceneInput.GetGamecontrollerState(0).AnalogLeft);
            //if (base.SceneInput.GetGamecontrollerState(0).AnalogRight > 0.0f) player2.MoveForward(base.SceneInput.GetGamecontrollerState(0).AnalogRight);
            //if (base.SceneInput.GetGamecontrollerState(0).AnalogUp > 0.0f) player2.MoveUp(base.SceneInput.GetGamecontrollerState(0).AnalogUp);
            //if (base.SceneInput.GetGamecontrollerState(0).AnalogDown > 0.0f) player2.MoveDown(base.SceneInput.GetGamecontrollerState(0).AnalogDown);
            if (base.SceneInput.GetGamecontrollerState(0).Button1 == ButtonState.Pressed)
            {
                player2.WeaponCurrent = 0;
                player2.Fire(base.SceneContent);
            }
            if (base.SceneInput.GetGamecontrollerState(0).Button2 == ButtonState.Pressed)
            {
                player2.WeaponCurrent = 1;
                player2.Fire(base.SceneContent);
            }
            if (base.SceneInput.GetGamecontrollerState(0).Button3 == ButtonState.Pressed)
            {
                player2.WeaponCurrent = 2;
                player2.Fire(base.SceneContent);
            }

            //Altri input
            if (base.SceneInput.IsKeyUp(Keys.Z))
            {
                base.SceneCamera.Shake(1000, 2, 2);
                postProcessingBlur.IsEnable = true;
                postProcessingColor.IsEnable = true;
                listEnemy[0].Fire();
            }

            //GC.Collect
            if (base.SceneInput.IsKeyUp(Keys.X))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            //Avvio movimento
            if (base.SceneInput.IsKeyUp(Keys.Space))
            {
                demoRunning = !demoRunning;
            }

            //Camera alto
            if (base.SceneInput.IsKeyUp(Keys.C))
            {
                if (gameScrolling == GameScrolling.Horizontal)
                {
                    gameScrolling = GameScrolling.Vertical;
                    base.SceneCamera.PositionZ = 0;
                    base.SceneCamera.PositionY = 110;
                    base.SceneCamera.PositionX = -1;
                }
                else if (gameScrolling == GameScrolling.Vertical)
                {
                    gameScrolling = GameScrolling.Rail;
                    base.SceneCamera.PositionX = -80.0f;
                    base.SceneCamera.PositionY = 30.0f;
                    base.SceneCamera.PositionZ = 0.0f;
                }
                else if (gameScrolling == GameScrolling.Rail)
                {
                    gameScrolling = GameScrolling.Horizontal;
                    base.SceneCamera.PositionX = 0.0f;
                    base.SceneCamera.PositionY = 30.0f;
                    base.SceneCamera.PositionZ = 80.0f;
                }
            }

            //Screenshot 
            if (base.SceneInput.IsKeyUp(Keys.V))
            {
                Core.CaptureScreenshot("Screenshot", true);
            }

            //Info panel
            if (base.SceneInput.IsKeyUp(Keys.B))
            {
                infoPanel.SwithMode();
            }

            //Menu in gioco
            if (base.SceneInput.IsKeyDown(Keys.Escape))
            {
                gameMode = GameMode.Menu;
            }
        }


        public override void Update(GameTime gameTime)
        {

            //Update suoni (la musica rimane comunque in qualsiasi modalità, menu o livello)
            soundBackground.Update(gameTime, base.SceneCamera);

            //Livello di gioco
            if (gameMode == GameMode.Game)
            {
                ////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////
                if (demoRunning)
                {
                    velocity.Seconds = gameTime.ElapsedGameTime.TotalSeconds;
                    this.player1.Position = this.player1.Position + velocity.VelocityVector;
                    this.player2.Position = this.player2.Position + velocity.VelocityVector;
                    base.SceneCamera.Target = base.SceneCamera.Target + velocity.VelocityVector;
                    base.SceneCamera.Position = base.SceneCamera.Position + velocity.VelocityVector;
                }
                /////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////*/

                //Update degli input
                UpdateInput(gameTime);
                
                //Update Player 1 e Player 2
                player1.Update(gameTime, base.SceneCamera);
                player2.Update(gameTime, base.SceneCamera);

                //Luci
                lightEffect.Update(gameTime, base.SceneCamera);
                lightEffect.PointLights[0].Position = player1.Position;
                lightEffect.PointLights[1].Position = player2.Position;

                //Update + collisioni Bullet giocatori
                bulletListByPlayers.Update(player1.Position, gameTime, base.SceneCamera);

                //Update + collisioni Bullet nemici
                bulletListByEnemies.Update(player2.Position, gameTime, base.SceneCamera);  //!!!!!!!!!!!!
                bulletListByEnemies.Collide(player1);
                bulletListByEnemies.Collide(player2);

                //Update + collisoni nemici
                for (int e = 0; e < listEnemy.Count; e++)
                {
                    //Sceglie il player più vicino
                    float distanceFromP1 = Vector3.Distance(listEnemy[e].Position, player1.Position);
                    float distanceFromP2 = Vector3.Distance(listEnemy[e].Position, player2.Position);
                    if (distanceFromP1 >= distanceFromP2)
                        listEnemy[e].Target = player2.Position;
                    else
                        listEnemy[e].Target = player1.Position;

                    //Update
                    listEnemy[e].Update(gameTime, base.SceneCamera);

                    //Collisione con nemico
                    bulletListByPlayers.Collide(listEnemy[e]);

                    //Controllo collisione con il nemico
                    if (player1.Collide(listEnemy[e]))
                    {
                        //Rimuove il nemico dalla lista
                        listEnemy.Remove(listEnemy[e]);
                    }

                    //Controllo collisione con il nemico
                    if (player2.Collide(listEnemy[e]))
                    {
                        //Rimuove il nemico dalla lista
                        listEnemy.Remove(listEnemy[e]);
                    }

                }

                //Update + collisoni powerUp
                for (int p = 0; p < listPowerup.Count; p++)
                {
                    //Update
                    listPowerup[p].Update(gameTime, base.SceneCamera);

                    //Controllo collisione player1
                    if (player1.Collide(listPowerup[p], (float)gameTime.TotalGameTime.TotalMilliseconds))
                    {
                        //Rimuove il bullet dalla lista
                        listPowerup.Remove(listPowerup[p]);
                    }

                    //Controllo collisione player2
                    if (player2.Collide(listPowerup[p], (float)gameTime.TotalGameTime.TotalMilliseconds))
                    {
                        //Rimuove il bullet dalla lista
                        listPowerup.Remove(listPowerup[p]);
                    }
                }

                //Update elementi scenografici
                for (int i = 0; i < listLevelElement.Count; i++)
                {
                    listLevelElement[i].Update(gameTime, base.SceneCamera);
                }

                //Update Sfondo
                sky.Update(gameTime, base.SceneCamera);

                //Superficie
                surface.Update(gameTime, base.SceneCamera);

                //Nebula
                nebula.Update(gameTime, base.SceneCamera);

                //Flare
                flare1.Update(gameTime, base.SceneCamera);

                //Update font
                hudLevelTitle.Update(gameTime);
                if (hudLevelTitle.State == Font_Typed.TypeState.WaitToFinish)   hudLevelSubtitle.ToDraw = true;
                hudLevelSubtitle.Update(gameTime);

                //Score
                hudScoreCaption.Update(gameTime);
                hudScore.Update(gameTime);
                hudScore.Text = Convert.ToString(player1.PlayerProperties.Score + player2.PlayerProperties.Score); ;
                hudScoreBorder.Update(gameTime);
                spriteBatcher.Update(gameTime);

                //Effetto cine
                cineEffect.Update(gameTime);

                //Pannello informazioni
                infoPanel.Update(gameTime);

            }

            //InGame Menu
            if (gameMode == GameMode.Menu)
            {
                InGameMenuUpdate(gameTime);
            }
        }



        public override void Draw()
        {
            //Imposta i Renderstates del vieport corrente
            Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backgroundColor, 1.0f, 0);
             
            //Superfice
            surface.Draw(lightEffect);

            //Disegna gli elementi scenografici
            for (int i = 0; i < listLevelElement.Count; i++)
            {
                listLevelElement[i].Draw(lightEffect);
            }

            //Disegna i nemici
            for (int i = 0; i < listEnemy.Count; i++)
            {
                listEnemy[i].Draw(lightEffect);
            }

            //Disegna i power ups
            for (int i = 0; i < listPowerup.Count; i++)
            {
                listPowerup[i].Draw(lightEffect);
            }

            //Disegna il player 1
            player1.Draw(lightEffect);

            //Disegna il player 2
            player2.Draw(lightEffect);


            //Nebula
            //nebula.Draw(lightEffect);

            /*Sfondo (viene disegnato dopo aver disegnato il resto per ottimizzare l'uso dello z-buffer)
            sky.Draw(lightEffect);

            //Liste proiettili
            bulletListByPlayers.Draw(lightEffect);
            bulletListByEnemies.Draw(lightEffect);

            //Disegna gli elementi 2D
            player1.Draw2D();
            player2.Draw2D();

            //Lens flare
            //flare1.Draw();

            //Score
            hudScoreBorder.Draw();
            spriteBatcher.Draw(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, Matrix.Identity);
            hudLevelTitle.Draw();
            hudLevelSubtitle.Draw();
            hudScoreCaption.Draw();
            hudScore.Draw();

            //Effetto cine
            cineEffect.Draw();

            //Post processing
            postProcessingColor.Apply();*/

            //Debug mode
            if (debugMode)
            {
                ShowDebugMode();
            }

            //InGame Menu
            if (gameMode == GameMode.Menu)
            {
                //Post processing
                postProcessingBlur.TexelSize = new Vector2(0.0035f, 0.0035f);
                postProcessingBlur.IsEnable = true;
                postProcessingBlur.Apply();

                //Mostra menu
                InGameMenuDraw();
            }

            //Pannello informazioni
            infoPanel.Draw();


        }



        private void AddElement(SpacelineElement element)
        {
            //Tipo
            if (element.Type == SpacelineElementType.Instance)
            {
                object[] args = new object[1] { base.SceneContent };
                Type ty = Type.GetType("SorsAdversa." + element.ElementName);

                if (element.ElementName.StartsWith("Enemy"))
                {
                    Enemy en = (Enemy)Activator.CreateInstance(ty, args);
                    en.Position = element.Position;
                    listEnemy.Add(en);
                }
                else if (element.ElementName.StartsWith("PowerUp"))
                {
                    PowerUp en = (PowerUp)Activator.CreateInstance(ty, args);
                    en.Position = element.Position;
                    listPowerup.Add(en);
                }

            }

            //Elementi scenografici da content (non hanno un nome di istanza, ma usano l'asset)
            else if (element.Type == SpacelineElementType.Content)
            {
                LevelElement newElement = new LevelElement();
                newElement.Initialize(element.ElementName, base.SceneContent);
                newElement.Position = element.Position;
                listLevelElement.Add(newElement);
            }

            //Camera triggers
            else if (element.Type == SpacelineElementType.Camera)
            {
                //.....
            }


        }







           
        public void InGameMenuUpdate(GameTime gameTime)
        {
            //Muove la selezione (up e down)
            if (base.SceneInput.IsKeyUp(Keys.Down))
            {
                if (menuVoiceSelection == InGameSelection.Continue)
                {
                    menuVoiceSelection = InGameSelection.Restart;
                }
                else if (menuVoiceSelection == InGameSelection.Restart)
                {
                    menuVoiceSelection = InGameSelection.Exit;
                }
                else if (menuVoiceSelection == InGameSelection.Exit)
                {
                    menuVoiceSelection = InGameSelection.Continue;
                }
            }
            if (base.SceneInput.IsKeyUp(Keys.Up))
            {
                if (menuVoiceSelection == InGameSelection.Continue)
                {
                    menuVoiceSelection = InGameSelection.Exit;
                }
                else if (menuVoiceSelection == InGameSelection.Restart)
                {
                    menuVoiceSelection = InGameSelection.Continue;
                }
                else if (menuVoiceSelection == InGameSelection.Exit)
                {
                    menuVoiceSelection = InGameSelection.Restart;
                }
            }

            //Selezione
            if (base.SceneInput.IsKeyDown(Keys.Enter))
            {
                if (menuVoiceSelection == InGameSelection.Continue)
                {
                    //Ritorna in gioco
                    gameMode = GameMode.Game;
                }
                else if (menuVoiceSelection == InGameSelection.Restart)
                {
                    //Riavvia
                    SorsAdversa.level = new Scene_Level("Scene_Level");
                    Core.SetCurrentScene(SorsAdversa.level, true);
                    return;
                }
                else if (menuVoiceSelection == InGameSelection.Exit)
                {
                    //Esce (torna al menu principale)
                    SorsAdversa.menu = new Scene_Menu("Scene_Menu");
                    Core.SetCurrentScene(SorsAdversa.menu,true);
                    SorsAdversa.level = null;
                    return;
                }
            }

            //Colorazione delle selezione
            if (menuVoiceSelection == InGameSelection.Continue)
            {
                menuVoice1.Color = Color.Red;
                menuVoice2.Color = Color.White;
                menuVoice3.Color = Color.White;
            }
            else if (menuVoiceSelection == InGameSelection.Restart)
            {
                menuVoice1.Color = Color.White;
                menuVoice2.Color = Color.Red;
                menuVoice3.Color = Color.White;
            }
            else if (menuVoiceSelection == InGameSelection.Exit)
            {
                menuVoice1.Color = Color.White;
                menuVoice2.Color = Color.White;
                menuVoice3.Color = Color.Red;
            }

            //Updates
            menuVoice1.Update(gameTime);
            menuVoice2.Update(gameTime);
            menuVoice3.Update(gameTime);
        }


        public void InGameMenuDraw()
        {
            menuVoice1.Draw();
            menuVoice2.Draw();
            menuVoice3.Draw();
        }


        public void ShowDebugMode()
        {
            //Disegna le bounding sphere (Player 1)
            debugBounding.Color = Color.LimeGreen;
            debugBounding.ReferenceSphere = player1.BoundingSphere;
            debugBounding.Draw(lightEffect, base.SceneCamera);
            for (int i = 0; i < player1.CollisionSpheres.Length; i++)
            {
                debugBounding.Color = Color.White;
                debugBounding.ReferenceSphere = player1.CollisionSpheres[i];
                debugBounding.Draw(lightEffect, base.SceneCamera);
            }
            //Disegna le bounding sphere (Player 2)
            debugBounding.Color = Color.Red;
            debugBounding.ReferenceSphere = player2.BoundingSphere;
            debugBounding.Draw(lightEffect, base.SceneCamera);
            for (int i = 0; i < player2.CollisionSpheres.Length; i++)
            {
                debugBounding.Color = Color.White;
                debugBounding.ReferenceSphere = player2.CollisionSpheres[i];
                debugBounding.Draw(lightEffect, base.SceneCamera);
            }
            //Disegna le bounding sphere (Enemies)
            foreach (Enemy currentEnemy in listEnemy)
            {
                debugBounding.Color = Color.Blue;
                debugBounding.ReferenceSphere = currentEnemy.BoundingSphere;
                debugBounding.Draw(lightEffect, base.SceneCamera);
                for (int i = 0; i < currentEnemy.CollisionSpheres.Length; i++)
                {
                    debugBounding.Color = Color.White;
                    debugBounding.ReferenceSphere = currentEnemy.CollisionSpheres[i];
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }           
            }
            //Disegna le bounding sphere (PowerUp)
            foreach (PowerUp currentPowerup in listPowerup)
            {
                debugBounding.Color = Color.Gold;
                debugBounding.ReferenceSphere = currentPowerup.BoundingSphere;
                debugBounding.Draw(lightEffect, base.SceneCamera);
                for (int i = 0; i < currentPowerup.CollisionSpheres.Length; i++)
                {
                    debugBounding.Color = Color.White;
                    debugBounding.ReferenceSphere = currentPowerup.CollisionSpheres[i];
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }   
            }
            //Disegna le bounding sphere (Level Elements)
            foreach (LevelElement currentElement in listLevelElement)
            {
                debugBounding.Color = Color.Blue;
                debugBounding.ReferenceSphere = currentElement.BoundingSphere;
                debugBounding.Draw(lightEffect, base.SceneCamera);
                for (int i = 0; i < currentElement.CollisionSpheres.Length; i++)
                {
                    debugBounding.Color = Color.BlueViolet;
                    debugBounding.ReferenceSphere = currentElement.CollisionSpheres[i];
                    debugBounding.Draw(lightEffect, base.SceneCamera);
                }
            }
            //Disegna le bounding sphere (Bullets Players)
            for (int i = 0; i < bulletListByPlayers.Capacity; i++ )
            {
                if ((bulletListByPlayers.Elements[i] != null) && (bulletListByPlayers.Elements[i].CollisionSpheres != null))
                {
                    for (int s = 0; s < bulletListByPlayers.Elements[i].CollisionSpheres.Length; s++)
                    {
                        debugBounding.Color = Color.CornflowerBlue;
                        debugBounding.ReferenceSphere = bulletListByPlayers.Elements[i].CollisionSpheres[s];
                        debugBounding.Draw(lightEffect, base.SceneCamera);
                    }
                }
            }
            //Disegna le bounding sphere (Bullets Enemies)
            for (int i = 0; i < bulletListByEnemies.Capacity; i++)
            {
                if ((bulletListByEnemies.Elements[i] != null) && (bulletListByEnemies.Elements[i].CollisionSpheres != null))
                {
                    for (int s = 0; s < bulletListByEnemies.Elements[i].CollisionSpheres.Length; s++)
                    {
                        debugBounding.Color = Color.Red;
                        debugBounding.ReferenceSphere = bulletListByEnemies.Elements[i].CollisionSpheres[s];
                        debugBounding.Draw(lightEffect, base.SceneCamera);
                    }
                }
            }

            //Luci
            lightEffect.Draw(base.SceneCamera);

            //Update font
            debugFont.Update(null);

            debugFont.Color = Color.Yellow;
            debugFont.Position = new Vector2(20, 200);
            debugFont.Text = "Surface Sections: " + surface.VisibleSections.ToString() + "/" + surface.TotalSections.ToString();
            debugFont.Draw();

            debugFont.Color = Color.Yellow;
            debugFont.PositionY = debugFont.PositionY + 15;
            debugFont.Text = "Player1 InFrustrum: " + player1.InFrustrum.ToString() + " - " + player1.CountInfo.CurrentVerticies;
            debugFont.Draw();

            debugFont.PositionY = debugFont.PositionY + 15;
            debugFont.Text = "Camera: " + base.SceneCamera.PositionX.ToString() + ";" + base.SceneCamera.PositionY.ToString() + ";" + base.SceneCamera.PositionZ.ToString();
            debugFont.Draw();

            debugFont.PositionY = debugFont.PositionY + 25;
            debugFont.Text = "Players Bullets: " + bulletListByPlayers.ActiveElements.ToString() + "/" + bulletListByPlayers.Capacity.ToString();
            debugFont.Draw();

            debugFont.PositionY = debugFont.PositionY + 15;
            debugFont.Text = "Enemies Bullets: " + bulletListByEnemies.ActiveElements.ToString() + "/" + bulletListByEnemies.Capacity.ToString() ;
            debugFont.Draw();
        }


        /////////////////////////////////
        //    Controllo Eventi
        /////////////////////////////////
        void SceneCamera_OnShakeFinish(object sender, EventArgs e)
        {
            postProcessingBlur.IsEnable = false;
            postProcessingColor.IsEnable = false;
        }


        /////////////////////////////////
        //    Release generale 
        /////////////////////////////////
        public override void Release()
        {
            debugBounding = null;
            soundBackground.Stop();
            soundBackground = null;
            debugFont = null;
            player1 = null;
            player2 = null;
            sky = null;
            flare1 = null;
            for (int i = 0; i < listEnemy.Count; i++)
                listEnemy[i] = null;
            listEnemy = null;

            for (int i = 0; i < listPowerup.Count; i++)
                listPowerup[i] = null;
            listPowerup = null;

            for (int i = 0; i < listLevelElement.Count; i++)
                listLevelElement[i] = null;
            listLevelElement = null;

            lightEffect = null;
            cineEffect = null;
        }




        /////////////////////////////////
        //    Load & Save XML 
        /////////////////////////////////
        public bool Load(string filename)
        {
            //Il file esiste?
            if (File.Exists(Environment.CurrentDirectory + "\\" + filename))
            {
                //Carica il file
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                //Livello
                XmlElement levelElement = (XmlElement)doc.GetElementsByTagName("Level")[0];
                int indexXML = Convert.ToInt32(levelElement.GetElementsByTagName("Index")[0].InnerText);
                string nameXML = levelElement.GetElementsByTagName("Name")[0].InnerText;
                Vector3 velocityvectorXML = Vector3Helper.VectorFromString(levelElement.GetElementsByTagName("VelocityVector")[0].InnerText);
                string velocityvalueXML = levelElement.GetElementsByTagName("VelocityValue")[0].InnerText;
                string skyboxXML = levelElement.GetElementsByTagName("SkyBox")[0].InnerText;
                string lensflareXML = levelElement.GetElementsByTagName("Lensflare")[0].InnerText;
                Color backgroundcolorXML = ColorHelper.ColorFromString(levelElement.GetElementsByTagName("BackgroundColor")[0].InnerText);
                string backgroundmusicXML = levelElement.GetElementsByTagName("BackgroundMusic")[0].InnerText;
                string uuu = levelElement.GetElementsByTagName("Player1")[0].InnerText;
                Vector3 player1XML = Vector3Helper.VectorFromString(levelElement.GetElementsByTagName("Player1")[0].InnerText);
                Vector3 player2XML = Vector3Helper.VectorFromString(levelElement.GetElementsByTagName("Player2")[0].InnerText);

                //Nemici
                foreach (XmlNode node in doc.GetElementsByTagName("Enemy"))
                {
                    XmlElement enemyElement = (XmlElement)node;
                    string instanceName = enemyElement.GetElementsByTagName("Name")[0].InnerText;
                    /*Enemy newEnemy = Assembly.LoadFrom("SorsAdversa.exe").CreateInstance(instanceName);
                    newEnemy.PositionX = Vector3Helper.VectorFromString(enemyElement.GetElementsByTagName("Position")[0].InnerText));
                    listEnemy.Add(newEnemy);*/
                }

                //Powerup
                foreach (XmlNode node in doc.GetElementsByTagName("PowerUp"))
                {
                    XmlElement powerElement = (XmlElement)node;
                    string val1 = powerElement.GetElementsByTagName("Name")[0].InnerText;
                }

                //Element
                foreach (XmlNode node in doc.GetElementsByTagName("Element"))
                {
                    XmlElement elementElement = (XmlElement)node;
                    string val1 = elementElement.GetElementsByTagName("Name")[0].InnerText;
                    Vector3 val2 = Vector3Helper.VectorFromString(elementElement.GetElementsByTagName("Position")[0].InnerText);
                }

                //Section
                foreach (XmlNode node in doc.GetElementsByTagName("Section"))
                {
                    XmlElement sectionElement = (XmlElement)node;
                    string val1 = sectionElement.GetElementsByTagName("Name")[0].InnerText;
                    string val2 = sectionElement.GetElementsByTagName("Y")[0].InnerText;
                    string val3 = sectionElement.GetElementsByTagName("Z")[0].InnerText;
                    string val4 = sectionElement.GetElementsByTagName("Reps")[0].InnerText;
                }

                //Info
                XmlElement infoElement = (XmlElement)doc.GetElementsByTagName("Info")[0];
                string creatorXML = infoElement.GetElementsByTagName("Creator")[0].InnerText;
                string dateXML = infoElement.GetElementsByTagName("Date")[0].InnerText;
                string timeXML = infoElement.GetElementsByTagName("Time")[0].InnerText;
                string infoXML = infoElement.GetElementsByTagName("Note")[0].InnerText;
                string engineXML = infoElement.GetElementsByTagName("EngineVersion")[0].InnerText;
                string appXML = infoElement.GetElementsByTagName("ApplicationVersion")[0].InnerText;

                //OK
                return true;
            }
            else
            {
                return false;
            }         
        }


        public bool Save(string filename)
        {
            //Controlla l'esistenza della directory, altirmenti la crea
            string[] splitter = filename.Split('\\');
            string createDir = string.Empty;
            for (int i = 0; i < splitter.Length - 1; i++)
            {
                createDir = createDir + "\\" + splitter[i];
            }
            if (Directory.Exists(Environment.CurrentDirectory + createDir) == false)
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + createDir);
            }

            //Crea il documento
            XmlTextWriter writer = new XmlTextWriter(filename, null);

            //Il file è stato creato nelal posizione corretta?
            if (File.Exists(Environment.CurrentDirectory + "\\" + filename))
            {                
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;

                //Titolo (come commento)
                writer.WriteComment("Sors Adversa Level Composition File");

                //Root
                writer.WriteStartElement("Level");
                {
                    //Proprietà del livello
                    writer.WriteElementString("Index", "1");
                    writer.WriteElementString("Name", "Asteroids Field");
                    writer.WriteElementString("VelocityVector", Vector3Helper.VectorToString(this.velocity.Direction));
                    writer.WriteElementString("VelocityValue", this.velocity.Amount.ToString());
                    writer.WriteElementString("SkyBox", "Content\\Texture\\Sky4");
                    writer.WriteElementString("Lensflare", "\\");
                    writer.WriteElementString("BackgroundColor", ColorHelper.ColorToString(backgroundColor));
                    writer.WriteElementString("BackgroundMusic", "GameMusic");
                    writer.WriteElementString("Player1", Vector3Helper.VectorToString(new Vector3(20.5f, 222.4f, 34.7384f)));
                    writer.WriteElementString("Player2", Vector3Helper.VectorToString(player2.Position));

                    //Scrive la lista dei nemici
                    if (listEnemy.Count > 0)
                    {
                        writer.WriteStartElement("EnemyList");
                        {
                            for (int i = 0; i < listEnemy.Count; i++)
                            {
                                writer.WriteStartElement("Enemy");
                                {
                                    writer.WriteElementString("Name", listEnemy[i].GetType().Name.ToString());
                                    writer.WriteElementString("Position", Vector3Helper.VectorToString(listEnemy[i].Position));

                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }

                    //Scrive la lista dei powerup
                    if (listPowerup.Count > 0)
                    {
                        writer.WriteStartElement("PowerupList");
                        {
                            for (int i = 0; i < listPowerup.Count; i++)
                            {
                                writer.WriteStartElement("PowerUp");
                                {
                                    writer.WriteElementString("Name", listPowerup[i].GetType().Name.ToString());
                                    writer.WriteElementString("Position", Vector3Helper.VectorToString(listPowerup[i].Position));
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }

                    //Scrive la lista degli elementi grafici
                    if (listLevelElement.Count > 0)
                    {
                        writer.WriteStartElement("ElementList");
                        {
                            for (int i = 0; i < listLevelElement.Count; i++)
                            {
                                writer.WriteStartElement("Element");
                                {
                                    writer.WriteElementString("Name", listLevelElement[i].AssetName.ToString());
                                    writer.WriteElementString("Position", Vector3Helper.VectorToString(listLevelElement[i].Position));
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }

                    //Scrive la lista dei settori della superficie
                    if (surface.Sections.Count > 0)
                    {
                        writer.WriteStartElement("SectionList");
                        {
                            for (int i = 0; i < surface.Sections.Count; i++)
                            {
                                writer.WriteStartElement("Section");
                                {
                                    writer.WriteElementString("Name", surface.Sections[i].ElementName.ToString());
                                    writer.WriteElementString("Y", surface.Sections[i].PositionY.ToString());
                                    writer.WriteElementString("Z", surface.Sections[i].PositionZ.ToString());
                                    writer.WriteElementString("Reps", surface.Sections[i].Repetitions.ToString());
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }

                    //Informazioni creazione del file
                    writer.WriteStartElement("Info");
                    {
                        writer.WriteElementString("Creator", "Duff");
                        writer.WriteElementString("Date", DateTime.Today.ToShortDateString());
                        writer.WriteElementString("Time", DateTime.Now.TimeOfDay.Hours.ToString() + ":" + DateTime.Now.TimeOfDay.Minutes.ToString() + ":" + DateTime.Now.TimeOfDay.Seconds.ToString());
                        writer.WriteElementString("Note", "NOT to release");
                        writer.WriteElementString("EngineVersion", Core.EngineVersion + " (" + Core.EngineCompileMode + ")");
                        writer.WriteElementString("ApplicationVersion", Assembly.GetExecutingAssembly().GetName(false).Version.ToString()) ;
                    }
                    writer.WriteEndElement();


                }
                writer.WriteEndElement();

                //Chiude il documento
                writer.WriteEndDocument();
                writer.Close();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}