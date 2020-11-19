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
    //Stati Intelligenza Artificiale
    public enum AIControlState : int
    {
        Search,
        Attack,
        Retreat,
        Die,
    }
    public enum AIFireState : int
    {
        NoFire,
        LowFire,
        MediumFire,
        HighFire,
    }

    public class Enemy : XModel
    {
        //Stati attuali
        private AIControlState stateControl = AIControlState.Search;
        private AIFireState stateFire = AIFireState.NoFire;

        //Nome
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //Velocità (s = v * t)
        private Velocity velocity = new Velocity(new Vector3(-1, 0, 0), 0);
        public Velocity Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        //Target
        private Vector3 targetOld;
        private Vector3 target;
        public Vector3 Target
        {
            get { return target; }
            set 
            {
                targetOld = target;
                target = value; 
            }
        }

        //Energia
        private uint energy = 100;
        public uint Energy
        {
            get { return energy; }
            set 
            { 
                energy = value;
                //in qualsiasi caso se l'energia è sotto il 10% scappa
                if (energy <= 10)
                {
                    stateControl = AIControlState.Retreat;
                }
                //in qualsiasi caso se l'energia è sotto 0 muore
                if (energy <= 0)
                {
                    stateControl = AIControlState.Die;
                }
            }
        }

        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }


        //Pericolosità (grado di pericolosità del nemico, usato per far decidere i seeker bullet)
        private int dangerousMeter = 10;
        public int DangerousMeter
        {
            get { return dangerousMeter; }
            set { dangerousMeter = value; }
        }

        //Indicatore
        //private Lines2D targetIndicator;


        public Enemy(Scene parentScene):base(parentScene)
        {
        }

        public override bool Initialize(string assetName, ContentManager contentManager)
        {
            try
            {
                //Crea il modello principale
                base.Initialize(assetName, contentManager);

                //////////////////////////////////////////////// ////////////////////////////////////////////////
                base.RotationY = MathHelper.ToRadians(-90);       ///////////////Rotazione sull'asse in modo da andare da destra a sinistra (da rivedere, da fare nei singoli nemici)////////////////
                ////////////////////////////////////////////////  ///////////////////////////////////////////////

                /*Indicatore
                targetIndicator = new Lines2D(3);
                targetIndicator.AddLine(new Line2D(new Vector2(0, 0), new Vector2(100, -50), Color.Red, Color.Orange));
                targetIndicator.AddLine(new Line2D(new Vector2(100, -50), new Vector2(200, -50), Color.Orange, Color.Yellow));
                targetIndicator.AddLine(new Line2D(new Vector2(200, -50), new Vector2(200, -44), Color.Yellow, Color.Yellow));*/

                //Creazione OK
                isCreated = true;
                return true;
            }
            catch
            {
                //Creazione fallita
                isCreated = false;
                return false;
            }
        }

        public override sealed void Update(GameTime gameTime, Camera camera)
        {
            if (IsCreated)
            {
                //Indicatore
                //targetIndicator.PositionOffset = this.Position2D;
                //targetIndicator.Update(gameTime);


                //Aggiornamento AI
                UpdateAI(gameTime);

                //Update generale
                base.Update(gameTime, camera);

            }
        }

        public void UpdateAI(GameTime gameTime)
        {

            //Stati di movimento
            switch (stateControl)
            {
                case AIControlState.Search:
                    {
                        //Nuova posizione ( s = v * t )
                        //Velocità
                        velocity.Seconds = gameTime.ElapsedGameTime.TotalSeconds;

                        base.Position = base.Position + velocity;

                        /*Inizia lo stato di Attack solo quando il target e il nemico sono abbastanza vicini 20m
                        if (GraphicsHelper.SphereInSphere(base.Position, base.Radius, target, 20))
                        {
                            stateControl = AIControlState.Attack;
                        }*/

                        //TEMP di prova
                        //Vector3 viewVector = new Vector3(1, 1, 0);
                        //this.velocity = GraphicsHelper.DirectionVector(model.Position, 45.0f) * viewVector *0.005f;


                        break;
                    }

                case AIControlState.Attack:
                    {
                        //Predictive Chasing
                        //Il target del nemico punta alla posizione che potrebbe avere il target tra N steps
                        Vector3 n = ((target - targetOld) * 50) + target ;
                        Vector3 v = n - base.Position;
                        if (v.LengthSquared() != 0)
                            v.Normalize();
                        velocity.Direction = v * 0.0075f;

                        Vector3 posRound = new Vector3((float)Math.Round(base.PositionX, 1), (float)Math.Round(base.PositionY, 1), (float)Math.Round(base.PositionZ, 1));
                        Vector3 tarRound = new Vector3((float)Math.Round(target.X, 1), (float)Math.Round(target.Y, 1), (float)Math.Round(target.Z, 1));
                        if (posRound == tarRound)
                            stateControl = AIControlState.Retreat;

                        break;
                    }

                case AIControlState.Retreat:
                    {
                        Vector3 v = base.Position - target;
                        if (v.LengthSquared() != 0)
                            v.Normalize();
                        velocity.Direction = v * 0.005f;

                        break;
                    }

                case AIControlState.Die:
                    {
                        //Muore !!!
                        base.Position = new Vector3(0, 0, 0);
                        break;
                    }
            }


            //Stati di fuoco
            switch (stateFire)
            {
                case AIFireState.NoFire:
                    {
                        break;
                    }

                case AIFireState.MediumFire:
                    {
                        break;
                    }
            }

        }

        public override sealed void Draw(LightEffect lightEffect)
        {
            if (isCreated)
            {
                //Disegna il modello principale
                base.Draw(lightEffect);

                //Indicatore
                //targetIndicator.Draw();
            }
        }


        public bool Fire()
        {
            try
            {
                //Decompone la matrice finale dell'arma in modo da prelevarne il quaternione di rotazione (non funzionerebbe con un semplice vettore)
                //Il quaternione viene usato per la generazione locale del bullet in base alla rotazione locale dell'arma
                Vector3 scaleTemp;
                Vector3 positionTemp;
                Quaternion localRotation;
                Matrix matTemp = Matrix.Multiply(base.WorldMatrix, base.ExternalMatrix);
                matTemp.Decompose(out scaleTemp, out localRotation, out positionTemp);


                /*Genera 5 colpi
                Bullet bulletArray;
                bulletArray = new Bullet("Content\\Texture\\Flare04", this.Position, localRotation, 60, 0, 0.025f, 3.0f, 3.0f, 100.0f, Color.Gold, Core.Content);
                Scene_Level.bulletListByEnemy.Add(bulletArray);

                bulletArray = new Bullet("Content\\Texture\\Flare04", this.Position, localRotation, 30, 0, 0.025f, 2.5f, 2.5f, 100.0f, Color.GreenYellow, Core.Content);
                Scene_Level.bulletListByEnemy.Add(bulletArray);

                bulletArray = new Bullet("Content\\Texture\\Flare04", this.Position, localRotation, 0, 0, 0.030f, 3.0f, 3.0f, 100.0f, Color.Gold, Core.Content);
                Scene_Level.bulletListByEnemy.Add(bulletArray);

                bulletArray = new Bullet("Content\\Texture\\Flare04", this.Position, localRotation, -30, 0, 0.025f, 2.5f, 2.5f, 100.0f, Color.GreenYellow, Core.Content);
                Scene_Level.bulletListByEnemy.Add(bulletArray);

                bulletArray = new Bullet("Content\\Texture\\Flare04", this.Position, localRotation, -60, 0, 0.030f, 3.0f, 3.0f, 100.0f, Color.Gold, Core.Content);
                Scene_Level.bulletListByEnemy.Add(bulletArray);
                */

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Collide(Bullet bullet)
        {
            //Collisione con BULLET
            if ((IsCreated) && (bullet.IsActive))
            {
                /*if (GraphicsHelper.SphereInSphere(bullet.Position, bullet.Radius, this.Position, this.Radius))
                {
                    //Esegue l'animazione,suono,visibilità ecc del bullet corrente
                    bullet.CollisionEffect();
                    
                    //Esegue la morte per scrontro (animazione, disegno, suoni, ecc)
                    //Andrebbe messo in uno stato di morte con la StateMachine, da fare!!!!!
                    //base.ToDraw = false;
                    //...Dovrebbe togliere l'energia
                    //...sound.Play();
                    //...

                    return true;
                }*/
            }
            return false;
        }
    }
}