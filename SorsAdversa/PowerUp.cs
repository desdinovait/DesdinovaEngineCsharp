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
    public class PowerUp : XModel
    {
        //Stati
        public enum PowerUpState
        {
            Running,
            Collecting,
            Using,
            Finish,
        }

        //Glow
        private Quad3D glow;

        //Suoni
        private Sound sound;
        
        //Colore glow
        protected Color GlowColor
        {
            set { glow.Color = value; }
        }

        //Scalatura glow
        protected Vector2 GlowScale
        {
            get { return glow.Scale; }
            set { glow.Scale = value; }
        }
	
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Rotazione del powerup
        private float tempRotation;

        //Tempo di permamenza
        private float startTime = 0.0f;
        private float time;
        protected float Time
        {
            get { return time; }
            set { time = value; }
        }

        //Tempo corrente d'uso
        private float countdown;
        public float Countdown
        {
            get { return countdown; }
        }

        //Veloictà
        private Vector3 velocity = new Vector3(0, 0, 0);
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
	

        //Stato
        private PowerUpState state = PowerUpState.Running;
        public PowerUpState State
        {
            get { return state; }
            set { state = value; }
        }

        public PowerUp(Scene parentScene):base(parentScene)
        {

        }

        public void Initialize(string modelName, string glowName, string soundName, ContentManager contentManager)
        {
            try
            {
                //Modello base
                if (base.Initialize(modelName, contentManager))
                {
                    //Glow
                    glow = new Quad3D(glowName, Quad3D.Quad3DGeneration.Center, contentManager);
                    glow.Scale = Vector2.One;
                    glow.Color = new Color(0,255,0,128);
                    glow.IsBillboard = true;

                    //Suono
                    sound = new Sound(soundName, true);

                    //Creazione OK
                    isCreated = true;
                }
                else
                {
                    //Creazione fallita
                    isCreated = false;
                }
            }
            catch
            {
                //Creazione fallita
                isCreated = false;
            }
        }


        public override sealed void Update(GameTime gameTime, Camera camera)
        {
            if (isCreated)
            {
                switch (state)
                {
                    case (PowerUpState.Running):
                        {
                            //Nuova posizione ( s = v * t )
                            base.PositionX = base.PositionX + (velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                            base.PositionY = base.PositionY + (velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                            base.PositionZ = base.PositionZ + (velocity.Z * (float)gameTime.ElapsedGameTime.TotalSeconds);

                            //Aggiornamento glow
                            glow.Position = base.Position;
                            glow.Update(gameTime, camera);

                            //Rotazione
                            tempRotation = tempRotation + (0.1f * (float)gameTime.ElapsedGameTime.Milliseconds);
                            if (tempRotation > 360.0f) tempRotation = 0;
                            base.RotationY = tempRotation;


                            //Aggiornamento modello pincipale
                            base.Update(gameTime, camera);

                            break;
                        }

                    case (PowerUpState.Collecting):
                        {
                            //Aggiornamento suoni
                            sound.Position = base.Position;
                            sound.Update(gameTime, camera);

                            state = PowerUpState.Using;

                            break;
                        }

                    case (PowerUpState.Using):
                        {
                            //Calcolo del fire rate
                            float currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds - startTime;
                            if (currentTime > time)
                            {
                                currentTime = 0.0f;
                                state = PowerUpState.Finish;
                            }

                            //Calcolo del countdown da visualizzare
                            countdown = (float)Math.Round((time - (float)(currentTime)) / 1000, 2);
                            if (countdown <= 0.0f)
                            {
                                countdown = 0.0f;
                            }

                            break;
                        }

                    case (PowerUpState.Finish):
                        {
                            break;
                        }
                }
            }
        }

        public override sealed void Draw(LightEffect lightEffect)
        {
            if (isCreated)
            {
                //Disegna solo se è ancora da raccogliere
                if (state == PowerUpState.Running)
                {
                    //Disegna il glow
                    glow.Draw(lightEffect);

                    //Disegna il modello principale
                    base.Draw(lightEffect);

                }
            }
        }

        public virtual bool CollisionEffect(ref PlayerParameters playerDef, float startTime)
        {
            if (IsCreated)
            {
                //Esegue la raccolta del powerup (animazione, disegno, suoni, ecc)
                base.ToDraw = false;
                this.glow.ToDraw = false;
                this.sound.Play();
                this.state = PowerUpState.Collecting;
                this.startTime = startTime;
                this.Position = new Vector3(-1000, -1000, -1000);   //Lo posiziona in un punto irraggiungibile (da verificare?!?)
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CollisionDeEffect(ref PlayerParameters playerDef)
        {
            if (this.State == PowerUpState.Finish)
            {
                return true;
            }
            return false;
        }
    }
}
