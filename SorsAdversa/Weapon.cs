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
using System.Diagnostics;

namespace SorsAdversa
{
    public class Weapon : XModel
    {
        //ContentManager temporaneo (usato per generare i colpi)
        protected ContentManager contentManager = null;

        //Nome
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
	
        //Souni
        private Sound sound = null;
	
        //Velocità di fuoco (in ms)
        private int fireRate = 100;
        public int FireRate
        {
            get { return fireRate; }
            set { fireRate = value; }
        }

        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Produce shake della camera
        private bool useShake = false;
        public bool UseShake
        {
            get { return useShake; }
            set { useShake = value; }
        }

        //Billboard fuoco
        private Quad3D fireFlash = null;
        private float fireFlashTime = 0.0f;

        //Tempo di fuoco precedente
        private double oldTime = 0;
        private double currentTime = 0;

        //Quaternione di Rotazione
        protected Quaternion localRotation = Quaternion.Identity;

        //Ancora per il punto di genrazione del colpo
        protected XModelAnchor firegeneratorAnchor = null;

        public Weapon(Scene parentScene):base(parentScene)
        {
        }

        public bool Initialize(string assetName, string soundName, ContentManager contentManager)
        {
            try
            {
                if (base.Initialize(assetName, contentManager))
                {
                    //Content manager
                    this.contentManager = contentManager;

                    //Suono
                    sound = new Sound(soundName, true);

                    //Flash
                    fireFlash = new Quad3D("Content\\Texture\\Ray03", Quad3D.Quad3DGeneration.Center, contentManager);
                    fireFlash.BlendProperties = BlendMode.Additive;
                    fireFlash.IsBillboard = true;
                    fireFlash.Scale = new Vector2(4.0f, 8.0f);
                    fireFlash.Color = new Color(Color.Orange.R, Color.Orange.G, Color.Orange.B, 128);

                    //Ancora della bocca di fuoco
                    firegeneratorAnchor = new XModelAnchor(this, "FiregeneratorAnchor");

                    //Creazione OK
                    isCreated = true;
                    return true;
                }
                else
                {
                    //Creazione fallita
                    isCreated = false;
                    return false;
                }
            }
            catch
            {
                //Creazione fallita
                isCreated = false;
                return false;
            }
        }

        public sealed override void Update(GameTime gameTime, Camera camera)
        {
            if (isCreated)
            {
                //Update base (modello)
                base.Update(gameTime, camera);

                //Suono
                if (sound != null)
                {
                    //Update del suono (usa il pivot del modello)
                    sound.Position = base.WorldMatrix.Translation + base.ExternalMatrix.Translation;
                    sound.Update(gameTime, camera);
                }

                //Flash
                if (fireFlash.ToDraw)
                {
                    fireFlashTime = fireFlashTime + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (fireFlashTime > fireRate / 10.0f)
                    {
                        fireFlashTime = 0.0f;
                        fireFlash.ToDraw = false;
                    }
                    fireFlash.Position = firegeneratorAnchor.FinalMatrix.Translation;
                    fireFlash.Update(gameTime, camera);
                }

                //Tempo
                currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public sealed override void Draw(LightEffect lightEffect)
        {
            if (isCreated)
            {
                //Draw base (modello)
                base.Draw(lightEffect);

                //Flash
                this.fireFlash.Draw(lightEffect);
            }
        }

        public virtual void ResetFire()
        {
            oldTime = 0.0f;
        }

        public virtual bool Fire(Vector3 targetPosition, ContentManager contentManager)
        {
            if (isCreated)
            {
                //Calcolo del fire rate
                if ((currentTime - oldTime) > fireRate)
                {
                    //Tempo di fuoco precedente
                    oldTime = currentTime;

                    //Suono
                    sound.Play();

                    //Decompone la matrice finale dell'arma (ancora di fuoco) in modo da prelevarne il quaternione di rotazione (non funzionerebbe con un semplice vettore)
                    //Il quaternione viene usato per la generazione locale del bullet in base alla rotazione locale dell'arma
                    Vector3 scaleTemp;
                    Vector3 positionTemp;
                    Matrix matTemp = Matrix.Multiply(firegeneratorAnchor.FinalMatrix, base.WorldMatrix);
                    matTemp.Decompose(out scaleTemp, out localRotation, out positionTemp);
                    
                    //Flash bocca di fuoco
                    fireFlash.ToDraw = true;

                    //Fuoco eseguito
                    return true;
                }
            }
            return false;
        }
    }
}
