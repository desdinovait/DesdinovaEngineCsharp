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

namespace SorsAdversa
{
    public enum PlayerType
    {
        Knife = 0,
        Zenith = 1,
        Introduer = 2,
        Destroyer = 3
    }

    public struct PlayerParameters
    {
        //Tipo di fighter
        private PlayerType type;
        public PlayerType Type
        {
            get { return type; }
            set { type = value; }
        }
	
        //Indice (1 o 2)
        private PlayerIndex index;
        public PlayerIndex Index
        {
            get { return index; }
            set { index = value; }
        }
	 
        //Nome del giocatore
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
       
        //Punti struttura
        private int structurePoint;
        public int StructurePoint
        {
            get { return structurePoint; }
            set
            {
                structurePoint = value;
                if (structurePoint <= 0) structurePoint = 0;
            }
        }         

        //Punteggio
        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                if (score <= 0) score = 0;
            }
        }

        //Energia massima dello scudo
        private float shieldEnergyMax;
        public float ShieldEnergyMax
        {
            get { return shieldEnergyMax; }
            set { shieldEnergyMax = value;}
        }

        //Energia dello scudo corrente
        private float shieldEnergy;
        public float ShieldEnergy
        {
            get { return shieldEnergy; }
            set
            {
                shieldEnergy=value;
                if (shieldEnergy <= 0) shieldEnergy = 0;
                if (shieldEnergy >= shieldEnergyMax) shieldEnergy = shieldEnergyMax;
            }
        }

        //Velocità di rigenerazione
        private float shieldRegen;
        public float ShieldRegen
        {
            get { return shieldRegen; }
            set
            {
                shieldRegen = value;
                if (shieldRegen <= 0) shieldRegen = 0;
            }
        }

        //veclocità di movimento
        private float velocity;
        public float Velocity
        {
            get { return velocity; }
            set
            {
                velocity = (float)Math.Round(value,3);
                if (velocity <= 0) velocity = 0;
            }
        }

        //potenza di fuoco
        private float firePower;
        public float FirePower
        {
            get { return firePower; }
            set
            {
                firePower = value;
                if (firePower <= 0) firePower = 0;
            }
        }

        //Abilita tutte le armi
        private bool allWeapons;
        public bool AllWeapons
        {
            get { return allWeapons; }
            set { allWeapons = value; }
        }
	
    }


    public class Player : XModel
    {
        //Armi
        public Weapon weapon1;
        public Weapon weapon2;
        public Weapon weapon3;
        private int weaponCurrent = 0;
        public int WeaponCurrent
        {
            get { return weaponCurrent; }
            set { weaponCurrent = value; }
        }

        //Proprietà
        public PlayerParameters PlayerProperties;
	
        //Velocità
        private Vector3 velocityVector = Vector3.Zero;

        //Motori
        private Quad3D motor1;
        private Quad3D motor2;
        private Quad3D motor3;

        //Ancore
        public XModelAnchor engineAnchor1;
        public XModelAnchor engineAnchor2;
        public XModelAnchor engineAnchor3;
        public XModelAnchor weaponAnchor1;
        public XModelAnchor weaponAnchor2;
        public XModelAnchor weaponAnchor3;
        public XModelAnchor centerAnchor;   //Ancora utile a qualcosa di generale

        //Sprites
        private SpriteBatcher spriteBatcher;

        //PowerUp corrente
        private PowerUp currentPowerUp;
        public PowerUp CurrentPowerUp
        {
            get { return currentPowerUp; }
        }

        //Danni
        private int currentDamage = 0;
        public int CurrentDamage
        {
            get { return currentDamage; }
            set { currentDamage = value; }
        }
	
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Timer
        private double oldTime = 0;

        //HUD
        private float hudBorderOffset = 10;
        private float hudInnerSpace = 3;
        private Sprite hudShieldBorderCap1;
        private Sprite hudShieldBorderCap2;
        private Sprite hudShieldBorder;
        private Sprite hudShieldBar;
        private Sprite hudShieldBarMega;
        private Sprite hudStructureBar;
        private Sprite hudTimer;
        private Font hudDebugFont;

        //Collisioni
        private Camera cam;

        public Player(PlayerParameters playerDefinition, ContentManager contentManager, Scene parentScene)
            : base(parentScene)
        {
            try
            {
                //Proprietà
                PlayerProperties = playerDefinition;

                //Differenziare il tipo di modello
                if (playerDefinition.Type == PlayerType.Knife)
                {
                    base.Initialize("Content\\Model\\Player\\Zenith", contentManager);
                }
                else if (playerDefinition.Type == PlayerType.Zenith)
                {
                    base.Initialize("Content\\Model\\Player\\Zenith", contentManager);
                }
                else if (playerDefinition.Type == PlayerType.Destroyer)
                {
                    base.Initialize("Content\\Model\\Player\\Zenith", contentManager);
                }
                else if (playerDefinition.Type == PlayerType.Introduer)
                {
                    base.Initialize("Content\\Model\\Player\\Zenith", contentManager);
                }

                //Ancore
                engineAnchor1 = new XModelAnchor(this, "EngineAnchor1");
                engineAnchor2 = new XModelAnchor(this, "EngineAnchor2");
                engineAnchor3 = new XModelAnchor(this, "EngineAnchor3");
                weaponAnchor1 = new XModelAnchor(this, "WeaponAnchor1");
                weaponAnchor2 = new XModelAnchor(this, "WeaponAnchor2");
                weaponAnchor3 = new XModelAnchor(this, "WeaponAnchor3");
                centerAnchor = new XModelAnchor(this, "CenterAnchor");

                //Motori
                motor1 = new Quad3D("Content\\Texture\\Star", Quad3D.Quad3DGeneration.Center, contentManager);
                motor2 = new Quad3D("Content\\Texture\\Star", Quad3D.Quad3DGeneration.Center, contentManager);
                motor3 = new Quad3D("Content\\Texture\\Star", Quad3D.Quad3DGeneration.Center, contentManager);
                motor1.Color = new Color(Color.White.R, Color.White.G, Color.White.B, 12);
                motor2.Color = new Color(Color.CornflowerBlue.R, Color.CornflowerBlue.G, Color.CornflowerBlue.B, 21);
                motor3.Color = new Color(Color.CornflowerBlue.R, Color.CornflowerBlue.G, Color.CornflowerBlue.B, 21);
                motor1.IsBillboard = true;
                motor2.IsBillboard = true;
                motor3.IsBillboard = true;

                //Armi correnti
                weapon1 = new Weapon_DeamonBeam(contentManager, this);
                weapon2 = new Weapon_5WayMachineGun(contentManager, this);
                weapon3 = new Weapon_PlasmaBolter(contentManager, this);

                //HUD
                if (this.PlayerProperties.Index == PlayerIndex.One)
                {
                    //Bordo Scudo chiusura 1
                    hudShieldBorderCap1 = new Sprite("Content\\Texture\\ShieldBorderCap", 1, contentManager);
                    hudShieldBorderCap1.ToDraw = true;
                    hudShieldBorderCap1.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorderCap1.Height - hudBorderOffset;
                    hudShieldBorderCap1.SpriteEffect = SpriteEffects.None;
                    //Bordo Scudo chiusura 2
                    hudShieldBorderCap2 = new Sprite("Content\\Texture\\ShieldBorderCap", 1, contentManager);
                    hudShieldBorderCap2.ToDraw = true;
                    hudShieldBorderCap2.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorderCap2.Height - hudBorderOffset;
                    hudShieldBorderCap2.SpriteEffect = SpriteEffects.None;
                    //Bordo Scudo
                    hudShieldBorder = new Sprite("Content\\Texture\\ShieldBorder", 1, contentManager);
                    hudShieldBorder.ToDraw = true;
                    hudShieldBorder.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorder.Height - hudBorderOffset;
                    hudShieldBorder.SpriteEffect = SpriteEffects.None;
                    hudShieldBorder.ScaleX = playerDefinition.ShieldEnergyMax;
                    //Barra Scudo
                    hudShieldBar = new Sprite("Content\\Texture\\ShieldBar", 1, contentManager);
                    hudShieldBar.ToDraw = true;
                    hudShieldBar.PositionY = hudShieldBorder.PositionY;
                    hudShieldBar.SpriteEffect = SpriteEffects.None;
                    hudShieldBar.ScaleX = playerDefinition.ShieldEnergy;
                    //Barra Scudo Mega (aggiuntiva)
                    hudShieldBarMega = new Sprite("Content\\Texture\\ShieldBarMega", 1, contentManager);
                    hudShieldBarMega.ToDraw = true;
                    hudShieldBarMega.PositionY = hudShieldBorder.PositionY;
                    hudShieldBarMega.SpriteEffect = SpriteEffects.None;
                    hudShieldBarMega.Color = Color.Red;
                    hudShieldBarMega.ScaleX = 0.5f; //Il file originale è di 100 pixels :-S
                    //Indicazione punti struttura
                    hudStructureBar = new Sprite("Content\\Texture\\StructureIndicator", 1, 6, true, contentManager);
                    hudStructureBar.ToDraw = true;
                    hudStructureBar.PositionX = hudBorderOffset;
                    hudStructureBar.PositionY = hudShieldBorder.PositionY - hudInnerSpace - hudStructureBar.Height;
                    hudStructureBar.SpriteEffect = SpriteEffects.None;
                    hudStructureBar.CurrentFrame = 5;
                    //Timer
                    hudTimer = new Sprite("Content\\Texture\\Timer", 6, 3, false, contentManager);
                    hudTimer.ToDraw = true;
                    hudTimer.SpriteEffect = SpriteEffects.None;
                    hudTimer.Origin = new Vector2(hudTimer.Width / 2, hudTimer.Height / 2);
                    hudTimer.CurrentFrame = 5;
                    hudTimer.ScaleX = 0.75f;
                    hudTimer.ScaleY = 0.75f;
                    //Debug
                    hudDebugFont = new Font("Content\\Font\\Courier New", contentManager);
                }
                else
                {
                    //Bordo Scudo chiusura
                    hudShieldBorderCap1 = new Sprite("Content\\Texture\\ShieldBorderCap", 1, contentManager);
                    hudShieldBorderCap1.ToDraw = true;
                    hudShieldBorderCap1.PositionX = Core.Graphics.GraphicsDevice.Viewport.Width - hudBorderOffset - hudShieldBorderCap1.Width;
                    hudShieldBorderCap1.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorderCap1.Height - hudBorderOffset;
                    //Bordo Scudo
                    hudShieldBorder = new Sprite("Content\\Texture\\ShieldBorder", 1, contentManager);
                    hudShieldBorder.ToDraw = true;
                    hudShieldBorder.ScaleX = 150;
                    hudShieldBorder.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorder.Height - hudBorderOffset;
                    hudShieldBorder.SpriteEffect = SpriteEffects.FlipHorizontally;
                    //Bordo Scudo chiusura
                    hudShieldBorderCap2 = new Sprite("Content\\Texture\\ShieldBorderCap", 1, contentManager);
                    hudShieldBorderCap2.ToDraw = true;
                    hudShieldBorderCap2.PositionX = hudShieldBorder.PositionX - hudShieldBorderCap2.Width;
                    hudShieldBorderCap2.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBorderCap2.Height - hudBorderOffset;
                    //Barra Scudo
                    hudShieldBar = new Sprite("Content\\Texture\\ShieldBar", 1, contentManager);
                    hudShieldBar.ToDraw = true;
                    hudShieldBar.ScaleX = playerDefinition.ShieldEnergy;
                    hudShieldBar.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - hudShieldBar.Height - hudBorderOffset;
                    hudShieldBar.SpriteEffect = SpriteEffects.None;
                    //Barra Scudo Mega (aggiuntiva)
                    hudShieldBarMega = new Sprite("Content\\Texture\\ShieldBarMega", 1, contentManager);
                    hudShieldBarMega.ToDraw = true;
                    hudShieldBarMega.PositionY = hudShieldBorder.PositionY;
                    hudShieldBarMega.SpriteEffect = SpriteEffects.None;
                    hudShieldBarMega.Color = Color.Red;
                    hudShieldBarMega.ScaleX = 0.5f; //Il file originale è di 100 pixels :-S
                    //Indicazione punti struttura
                    hudStructureBar = new Sprite("Content\\Texture\\StructureIndicator", 1, 6, true, contentManager);
                    hudStructureBar.ToDraw = true;
                    hudStructureBar.PositionX = Core.Graphics.GraphicsDevice.Viewport.Width - hudBorderOffset - hudStructureBar.Width;
                    hudStructureBar.PositionY = hudShieldBorder.PositionY - hudInnerSpace - hudStructureBar.Height;
                    hudStructureBar.SpriteEffect = SpriteEffects.FlipHorizontally;
                    hudStructureBar.CurrentFrame = 0;
                    //Timer
                    hudTimer = new Sprite("Content\\Texture\\Timer", 6, 3, false, contentManager);
                    hudTimer.ToDraw = true;
                    hudTimer.SpriteEffect = SpriteEffects.None;
                    hudTimer.Origin = new Vector2(hudTimer.Width / 2, hudTimer.Height / 2);
                    hudTimer.CurrentFrame = 12;
                    hudTimer.ScaleX = 0.75f;
                    hudTimer.ScaleY = 0.75f;
                    //Debug
                    hudDebugFont = new Font("Content\\Font\\Courier New", contentManager);
                }

                //Batcher 2D
                spriteBatcher = new SpriteBatcher();
                spriteBatcher.Add(hudStructureBar);
                spriteBatcher.Add(hudShieldBar);
                spriteBatcher.Add(hudShieldBarMega);
                spriteBatcher.Add(hudShieldBorder);
                spriteBatcher.Add(hudShieldBorderCap1);
                spriteBatcher.Add(hudShieldBorderCap2);
                spriteBatcher.Add(hudTimer);

                //Power up attivo
                currentPowerUp = null;

                //Creazione OK
                isCreated = true;
            }
            catch
            {
                //Creazione fallita
                isCreated = false;
            }
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            cam = camera;
            //Ricarica energia
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if ((currentTime - oldTime) > PlayerProperties.ShieldRegen)
            {
                //Tempo di fuoco precedente
                oldTime = currentTime;
                PlayerProperties.ShieldEnergy = PlayerProperties.ShieldEnergy + 1;
            }

            //Distanza
            Vector3 distance = Vector3.Zero;
            
            //Calcola lo spazio percorso in base alla velocità
            //Viene poi resettata la velocità perchè altrimenti continuerebbe lungo la direzione
            distance.X = velocityVector.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            distance.Y = velocityVector.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            distance.Z = velocityVector.Z * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocityVector = Vector3.Zero;

            //Confina il movimento nel frustrum attuale (da vedere ?!?!) 
            //10000 ???
            //Vector3 clampedPosition = Vector3.Clamp(base.Position + distance, new Vector3(-40, camera.Frustrum.Bottom.D - ((camera.Frustrum.Top.D / 100) * 10), 0), new Vector3(10000, -camera.Frustrum.Top.D - ((-camera.Frustrum.Top.D / 100) * 10), 0));
            //base.Position = clampedPosition;

            //Aggiornamento modello di base
            //pos = pos + s     (posizione attuale)
            //s = v * t         (spazio percorso)
            base.Position = base.Position + distance;
            base.Update(gameTime, camera);

            //Rotazione asse X (Riposizionamento nella posizione originaria)
            if (base.RotationX > 0) base.RotationX = base.RotationX - 0.5f;
            if (base.RotationX < 0) base.RotationX = base.RotationX + 0.5f;

            //Motore1
            motor1.RotationZ = 0.0f;
            motor1.Scale = new Vector2(4.0f, 4.0f);
            motor1.ExternalMatrix = engineAnchor1.FinalMatrix;
            motor1.Update(gameTime, camera);
            //Motore2
            motor2.RotationZ = 45.0f;
            motor2.Color = new Color(motor2.Color.R, motor2.Color.G, motor2.Color.B, RandomHelper.GetRandomByte(200, 255));
            motor2.Scale = new Vector2(RandomHelper.GetRandomFloat(3.9f, 4.2f), RandomHelper.GetRandomFloat(3.9f, 4.2f));
            motor2.ExternalMatrix = engineAnchor2.FinalMatrix;
            motor2.Update(gameTime, camera);
            //Motore3
            motor3.RotationZ = 0.0f;
            motor3.Color = new Color(motor3.Color.R, motor3.Color.G, motor3.Color.B, RandomHelper.GetRandomByte(200, 255));
            motor3.Scale = new Vector2(RandomHelper.GetRandomFloat(3.9f, 4.2f), RandomHelper.GetRandomFloat(3.9f, 4.2f));
            motor3.ExternalMatrix = engineAnchor3.FinalMatrix;
            motor3.Update(gameTime, camera);

            //Arma 1
            weapon1.ExternalMatrix = weaponAnchor1.FinalMatrix;
            weapon1.Update(gameTime, camera);
            
            //Arma 2
            weapon2.ExternalMatrix = weaponAnchor2.FinalMatrix;
            weapon2.Update(gameTime, camera);
            
            //Arma 3
            //La terza arma "fluttua ruotando" intorno al caccia
            //weapon3.PositionY = 1.5f;
            //weapon3.PositionX = -1.0f;
            //weaponAnchor1.RotationX = weaponAnchor1.RotationX + 0.5f;
            weapon3.ExternalMatrix = weaponAnchor3.FinalMatrix;
            weapon3.Update(gameTime, camera);

            //Impostazioni HUD in base ai valori correnti
            hudShieldBar.ScaleX = this.PlayerProperties.ShieldEnergy;
            if (this.PlayerProperties.Index == PlayerIndex.One)
            {
                hudShieldBorderCap1.PositionX = hudBorderOffset;
                hudShieldBorderCap2.PositionX = hudShieldBorderCap1.PositionX + hudShieldBorderCap2.Width + hudShieldBorder.Width;
                hudShieldBorder.PositionX = hudShieldBorderCap1.PositionX + hudShieldBorderCap1.Width;
                hudShieldBarMega.PositionX = hudShieldBorder.PositionX + hudShieldBorder.Width + hudShieldBorderCap2.Width;
                hudShieldBar.PositionX = hudShieldBorderCap1.PositionX + hudShieldBorderCap1.Width;
            }
            else if (this.PlayerProperties.Index == PlayerIndex.Two)
            {
                hudShieldBorderCap1.PositionX = Core.Graphics.GraphicsDevice.Viewport.Width - hudBorderOffset - hudShieldBorderCap1.Width;
                hudShieldBorderCap2.PositionX = hudShieldBorder.PositionX - hudShieldBorderCap2.Width;
                hudShieldBorder.PositionX = Core.Graphics.GraphicsDevice.Viewport.Width - hudBorderOffset - hudShieldBorder.Width;
                hudShieldBarMega.PositionX = hudShieldBorderCap2.PositionX - hudShieldBarMega.Width;
                hudShieldBar.PositionX = hudShieldBorderCap1.PositionX - hudShieldBar.ScaleX;
            }

            //Update HUD
            hudShieldBorder.Update(gameTime);
            hudShieldBorderCap1.Update(gameTime);
            hudShieldBorderCap2.Update(gameTime);
            hudShieldBar.Update(gameTime);
            hudShieldBarMega.Update(gameTime);
            hudStructureBar.Update(gameTime);
            if (this.currentPowerUp != null)hudTimer.CurrentFrame = 16-(int)this.currentPowerUp.Countdown;
            else hudTimer.CurrentFrame = 0;
            hudTimer.PositionX = this.Position2D.X;
            hudTimer.PositionY = this.Position2D.Y;
            hudTimer.Update(gameTime);
            hudDebugFont.Update(gameTime);

            //Batcher 2D
            spriteBatcher.Update(gameTime);

            //PowerUp attivo
            if (currentPowerUp != null)
            {
                currentPowerUp.Update(gameTime, camera);
                if (currentPowerUp.CollisionDeEffect(ref PlayerProperties))
                {
                    currentPowerUp = null;
                }
            }
        }

        public override void Draw(LightEffect lightEffect)
        {
            //Disegna il modello base
            base.Draw(lightEffect);

            //Disegna le armi
            weapon1.Draw(lightEffect);
            weapon2.Draw(lightEffect);
            weapon3.Draw(lightEffect);

            //Disegna i billboard dei motori
            motor1.Draw(lightEffect);
            motor2.Draw(lightEffect);
            motor3.Draw(lightEffect);     
        }

        public void Draw2D()
        {


            //Draw HUD
            hudShieldBorder.Draw();
            hudShieldBorderCap1.Draw();
            hudShieldBorderCap2.Draw();
            hudShieldBar.Draw();
            hudStructureBar.Draw();
            if (this.currentPowerUp != null)
            {
                //Se è un powerup di shield mostra la barra aggiuntiva
                if (currentPowerUp is PowerUp_ShieldBooster)
                {
                    hudShieldBarMega.Draw();
                }

                //Da vedere se ok
                if (Scene_Level.gameScrolling == GameScrolling.Horizontal)
                {
                    hudTimer.Rotation = 0;
                }
                else if (Scene_Level.gameScrolling == GameScrolling.Vertical)
                {
                    hudTimer.Rotation = 90;
                }
                hudTimer.Draw();
            }

            /*Scritte di debug
            if (this.PlayerProperties.Index == PlayerIndex.One)
            {

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_ShieldBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(20, 300);
                hudDebugFont.Text = "ShieldEnergy: " + this.PlayerProperties.ShieldEnergy + " / " + this.PlayerProperties.ShieldEnergyMax;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_RegenerationBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(20, 315);
                hudDebugFont.Text = "ShieldRegen: " + this.PlayerProperties.ShieldRegen;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_SpeedBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(20, 330);
                hudDebugFont.Text = "Velocity: " + this.PlayerProperties.Velocity;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_DamageBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(20, 345);
                hudDebugFont.Text = "FirePower: " + this.PlayerProperties.FirePower;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_AlphaStrike)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(20, 360);
                hudDebugFont.Text = "All weapons: " + this.PlayerProperties.AllWeapons.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 0) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(20, 375);
                hudDebugFont.Text = "Weapon 1: " + weapon1.Name.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 1) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(20, 390);
                hudDebugFont.Text = "Weapon 2: " + weapon2.Name.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 2) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(20, 405);
                hudDebugFont.Text = "Weapon 3: " + weapon3.Name.ToString();
                hudDebugFont.Draw();
            }
            else
            {
                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_ShieldBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 300);
                hudDebugFont.Text = "ShieldEnergy: " + this.PlayerProperties.ShieldEnergy + " / " + this.PlayerProperties.ShieldEnergyMax;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_RegenerationBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 315);
                hudDebugFont.Text = "ShieldRegen: " + this.PlayerProperties.ShieldRegen;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_SpeedBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 330);
                hudDebugFont.Text = "Velocity: " + this.PlayerProperties.Velocity;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_DamageBooster)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 345);
                hudDebugFont.Text = "FirePower: " + this.PlayerProperties.FirePower;
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.CurrentPowerUp != null) && (this.CurrentPowerUp is PowerUp_AlphaStrike)) hudDebugFont.Color = Color.Red;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 360);
                hudDebugFont.Text = "All weapons: " + this.PlayerProperties.AllWeapons.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 0) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 375);
                hudDebugFont.Text = "Weapon 1: " + weapon1.Name.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 1) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 390);
                hudDebugFont.Text = "Weapon 2: " + weapon2.Name.ToString();
                hudDebugFont.Draw();

                hudDebugFont.Color = Color.Azure;
                if ((this.weaponCurrent == 2) || this.PlayerProperties.AllWeapons) hudDebugFont.Color = Color.Yellow;
                hudDebugFont.Position = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width - 210, 405);
                hudDebugFont.Text = "Weapon 3: " + weapon3.Name.ToString();
                hudDebugFont.Draw();
            }//*/

            //Batcher 2D
            spriteBatcher.Draw(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, Matrix.Identity);
        }




        public void MoveNegativeY()
        {
            velocityVector.Y = -PlayerProperties.Velocity;
            if (base.RotationX < 35)
                base.RotationX = base.RotationX + 1;
        }
        public void MovePositiveY()
        {
            velocityVector.Y = PlayerProperties.Velocity;
            if (base.RotationX > -35)
                base.RotationX = base.RotationX - 1;
        }
        public void MovePositiveX()
        {
            velocityVector.X = PlayerProperties.Velocity;
        }
        public void MoveNegativeX()
        {
            velocityVector.X = -PlayerProperties.Velocity;
        }
        public void MovePositiveZ()
        {
            velocityVector.Z = PlayerProperties.Velocity;
            if (base.RotationX < 35)
                base.RotationX = base.RotationX + 1;
        }
        public void MoveNegativeZ()
        {
            velocityVector.Z = -PlayerProperties.Velocity;
            if (base.RotationX > -35)
                base.RotationX = base.RotationX - 1;
        }
    
        /*public void MoveDown(float analog)
        {
            velocityVector.Y = - PlayerProperties.Velocity * analog;
            if (base.RotationX < 35)
                base.RotationX = base.RotationX + 1;
        }
        public void MoveUp(float analog)
        {
            velocityVector.Y = PlayerProperties.Velocity * analog;
            if (base.RotationX > -35)
                base.RotationX = base.RotationX - 1;
        }
        public void MoveForward(float analog)
        {
            velocityVector.X = PlayerProperties.Velocity * analog;
        }
        public void MoveBackward(float analog)
        {
            velocityVector.X = - PlayerProperties.Velocity * analog;
        }*/



        public bool Collide(Enemy enemy)
        {
            //Collisione con NEMICO
            if ((IsCreated) && (enemy.ToDraw) && (enemy.InFrustrum))
            {
                if (enemy.CheckCollision(this))
                {
                    //Scontro Player - Nemico
                    //Toglie energia al nemico e al player

                    //Azione eseguita dal powerup corrente
                    this.PlayerProperties.ShieldEnergy = this.PlayerProperties.ShieldEnergy - 20;
                    return true;
                }
            }
            return false;
        }


        public bool Collide(Bullet bullet)
        {
            /*Collisione con BULLET
            if ((IsCreated) && (bullet.IsActive))
            {
                if (GraphicsHelper.SphereInSphere(bullet.Position, bullet.Radius, this.Position, this.Radius))
                {
                    //Esegue l'animazione,suono,visibilità ecc del bullet corrente
                    bullet.CollisionEffect();

                    //Azione eseguita dal powerup corrente
                    this.PlayerProperties.ShieldEnergy = this.PlayerProperties.ShieldEnergy - 10;
                    //... (es: mostrare lo scudo, togliere punti ecc)
                    //...sound.Play();
                    //...
                }
            }*/
            return false;
        }


        public bool Collide(PowerUp powerUp, float startTime)
        {
            //Collisione con POWERUP
            if ((IsCreated))
            {
                if (this.CheckCollision(powerUp))
                {
                    //C'è già un powerup attivo, lo termina subito
                    if (currentPowerUp != null)
                    {
                        if (currentPowerUp.CollisionDeEffect(ref PlayerProperties))
                        {
                            currentPowerUp = null;
                        }
                    }

                    //Esegue l'animazione,suono,visibilità ecc del power up preso
                    powerUp.CollisionEffect(ref PlayerProperties, startTime);
                   
                    //Imposta il powerup corrente come quello appena preso
                    currentPowerUp = powerUp;
                    return true;
                }
            }
            return false;
        }



        public bool ResetFire()
        {
            weaponCurrent = 0;
            weapon1.ResetFire();
            weapon2.ResetFire();
            weapon3.ResetFire();

            return true;
        }


        public bool Fire(ContentManager contentManager)
        {
            //Preleva il nemico più vicino e lo passa all'arma che lo targetterà
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //In realtà la definizione dei bullet dell'arma non andrà fatta dentro l'arma stasse ma qui
            //...dunque il target non dovrà essere passato al Fire() ma impostato qui nel bullet che sta x essere creato

            int targetEnemy = 0;
            float distance = 100000.0f;
            for (int n = 0; n < Scene_Level.listEnemy.Count; n++)
            {
                Vector3 distanceVector = this.Position - Scene_Level.listEnemy[n].Position;
                if (distanceVector.Length() < distance)
                {
                    distance = distanceVector.Length();
                    targetEnemy = n;
                }
            }

            //Trova il target
            Vector3 targetPosition = Vector3.Zero;
            if (Scene_Level.listEnemy != null && Scene_Level.listEnemy.Count > 0)
            {
                targetPosition = Scene_Level.listEnemy[targetEnemy].Position;
            }



            if (PlayerProperties.AllWeapons)
            {
                //Esegue lo sparo dell'arma corrente
                weapon1.Fire(targetPosition, contentManager);
                weapon2.Fire(targetPosition, contentManager);
                weapon3.Fire(targetPosition, contentManager);
                if ((weapon1.UseShake) || (weapon1.UseShake) || (weapon1.UseShake))
                {
                    return true;
                }
            }
            else
            {
                //Esegue lo sparo dell'arma corrente
                if (weaponCurrent == 0)
                {
                    weapon1.Fire(targetPosition, contentManager);
                    if (weapon1.UseShake) return true;
                }
                else if (weaponCurrent == 1)
                {
                    weapon2.Fire(targetPosition, contentManager);
                    if (weapon2.UseShake) return true;
                }
                else if (weaponCurrent == 2)
                {
                    weapon3.Fire(targetPosition, contentManager);
                    if (weapon3.UseShake) return true;
                }
            }
            return false;
        }
    }

}
