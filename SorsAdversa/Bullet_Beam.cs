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
    public class Bullet_Beam : Bullet
    {
        //Stati
        public enum BulletLinearState
        {
            Create,
            Load,
            InFly,
            Wait,
            Fade,
            Destroy
        }
        private BulletLinearState state = BulletLinearState.Create;
        public BulletLinearState State
        {
            get { return state; }
        }

        

        //Modello
        private Quad3D laser1;
        private Quad3D laser2;
        private Quad3D laserGlow1;
        private Quad3D laserGlow2;
        private Quad3D laserFire1;
        private Quad3D laserFire2;

        //Scalatura generale
        private Vector2 scale;
        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
            }
        }

        //Colori
        private Color laserColor;
        public Color LaserColor
        {
            get { return laserColor; }
            set 
            { 
                laserColor = value;
                laser1.Color = laserColor;
                laser2.Color = laserColor;
            }
        }
        private Color dropColor;
        public Color DropColor
        {
            get { return dropColor; }
            set 
            { 
                dropColor = value;
                laserGlow1.Color = dropColor;
                laserGlow2.Color = dropColor;
            }
        }
        private Color fireColor;
        public Color FireColor
        {
            get { return fireColor; }
            set 
            { 
                fireColor = value;
                laserFire1.Color = fireColor;
                laserFire2.Color = fireColor;
            }
        }

        private int loadTime = 3000;
        public int LoadTime
        {
            get { return loadTime; }
            set { loadTime = value; }
        }


        private int waitTime = 5000;
        public int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }
	

        private int fadeAlpha1 = 255;
        private int fadeAlpha2 = 255;
        private float startTime = 0.0f;

        //Movimento
        private float velocity = 0.0f;


        public Bullet_Beam(Texture2D laserTexture, Texture2D dropTexture, Texture2D fireTexture, int collisionSpheresNumber, ContentManager contentManager)
        {
            //Crezione billboards
            laser1 = new Quad3D(laserTexture, Quad3D.Quad3DGeneration.Left, contentManager);
            laser1.IsBillboard = false;
            laser1.BlendProperties = BlendMode.AlphaBlend;
            laser1.RotationX = 0;

            laser2 = new Quad3D(laserTexture, Quad3D.Quad3DGeneration.Left, contentManager);
            laser2.IsBillboard = false;
            laser2.BlendProperties = BlendMode.AlphaBlend;
            laser1.RotationX = 90;  //Perpendicolare a laser1

            laserGlow1 = new Quad3D(dropTexture, Quad3D.Quad3DGeneration.Center, contentManager);
            laserGlow1.IsBillboard = true;
            laserGlow1.BlendProperties = BlendMode.AlphaBlend;

            laserGlow2 = new Quad3D(dropTexture, Quad3D.Quad3DGeneration.Center, contentManager);
            laserGlow2.IsBillboard = true;
            laserGlow2.BlendProperties = BlendMode.AlphaBlend;

            laserFire1 = new Quad3D(fireTexture, Quad3D.Quad3DGeneration.Center, contentManager);
            laserFire1.IsBillboard = true;
            laserFire1.BlendProperties = BlendMode.AlphaBlend;

            laserFire2 = new Quad3D(fireTexture, Quad3D.Quad3DGeneration.Center, contentManager);
            laserFire2.IsBillboard = true;
            laserFire2.BlendProperties = BlendMode.AlphaBlend;

            //Sfere collisione
            if (collisionSpheresNumber < 1) collisionSpheresNumber = 1;
            collisionSpheres = new BoundingSphere[collisionSpheresNumber];
            collisionSpheres[0] = new BoundingSphere(laserFire1.Position, laserFire1.Radius);
            for (int i = 1; i < collisionSpheres.Length; i++)
            {
                collisionSpheres[i] = new BoundingSphere(laser1.Position, laser1.Radius);
            }

            //Attivazione
            base.isActive = true;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            //Processa gli stati
            switch (state)
            {
                //Creazione
                case (BulletLinearState.Create):
                    {
                        //Creazione
                        startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
                        state = BulletLinearState.Load;
                        break;
                    }

                //Caricamento
                case (BulletLinearState.Load):
                    {
                        float currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds - startTime;
                        if (currentTime > loadTime)
                        {
                            currentTime = 0.0f;
                            //Creazione
                            state = BulletLinearState.InFly;
                        }
                        else
                        {
                            laserGlow1.Scale = new Vector2(6.0f, 6.0f);
                            laserGlow1.ExternalMatrix = GeneratorMatrix;

                            laserGlow2.Scale = new Vector2(5.0f, 5.0f);
                            laserGlow2.ExternalMatrix = GeneratorMatrix;

                            laser1.ToDraw = false;
                            laser2.ToDraw = false;
                            laserGlow1.ToDraw = true;
                            laserGlow2.ToDraw = true;
                            laserFire1.ToDraw = false;
                            laserFire2.ToDraw = false;

                            base.position = GeneratorMatrix.Translation;
                        }

                        break;
                    }



                //Esecuzione
                case (BulletLinearState.InFly):
                    {
                        //Aumenta la scalatura (lunghezza del beam)
                        velocity = velocity + (speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                        laser1.Scale = new Vector2(velocity / 2.0f, 1.62f);
                        laser1.ExternalMatrix = GeneratorMatrix;

                        laser2.Scale = new Vector2(velocity / 2.0f, 1.62f);
                        laser2.ExternalMatrix = GeneratorMatrix;

                        laserGlow1.Scale = new Vector2(5.0f, 5.0f);
                        laserGlow1.ExternalMatrix = GeneratorMatrix;

                        laserGlow2.Scale = new Vector2(4.0f, 4.0f);
                        laserGlow2.ExternalMatrix = GeneratorMatrix;

                        Vector3 firePosition = laser1.Position;
                        firePosition.X = firePosition.X + velocity;

                        laserFire1.Scale = new Vector2(4.0f, 4.0f);
                        laserFire1.Position = firePosition;
                        laserFire1.ExternalMatrix = GeneratorMatrix;

                        laserFire2.Scale = new Vector2(3.0f, 3.0f);
                        laserFire2.Position = firePosition;
                        laserFire2.ExternalMatrix = GeneratorMatrix;

                        //Posizione attuale
                        base.position = firePosition + (TrackPosition - GeneratorMatrix.Translation);

                        //Calcolo della posizione corrente rispetto alla posizione iniziale
                        Vector3 distanceVector = firePosition - GeneratorMatrix.Translation;
                        if (distanceVector.Length() >= this.DistanceLife)
                        {
                            //Riusa lo startTime per lo stato seguente
                            startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

                            //La corsa del bullet è finita
                            this.state = BulletLinearState.Wait;
                        }

                        laser1.ToDraw = true;
                        laser2.ToDraw = true;
                        laserGlow1.ToDraw = true;
                        laserGlow2.ToDraw = true;
                        laserFire1.ToDraw = true;
                        laserFire2.ToDraw = true;

                        break;
                    }

                //Esecuzione
                case (BulletLinearState.Wait):
                    {
                        float currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds - startTime;
                        if (currentTime > waitTime)
                        {
                            currentTime = 0.0f;
                            state = BulletLinearState.Fade;
                        }

                        break;
                    }

                //Dissolvenza
                case (BulletLinearState.Fade):
                    {
                        //Dissolvenza
                        fadeAlpha1 = fadeAlpha1 - 2;
                        fadeAlpha2 = fadeAlpha2 - 1;
                        if (fadeAlpha2 <= 0)
                        {
                            this.state = BulletLinearState.Destroy;
                            break;
                        }

                        if (fadeAlpha1 >= 0)
                        {                        
                            laser1.Color = new Color(laser1.Color.R, laser1.Color.G, laser1.Color.B, Convert.ToByte(fadeAlpha1));
                            laser2.Color = new Color(laser2.Color.R, laser2.Color.G, laser2.Color.B, Convert.ToByte(fadeAlpha1));
                        }

                        if (fadeAlpha2 >= 0)
                        {
                            laserGlow1.Color = new Color(laserGlow1.Color.R, laserGlow1.Color.G, laserGlow1.Color.B, Convert.ToByte(fadeAlpha2));
                            laserGlow2.Color = new Color(laserGlow2.Color.R, laserGlow2.Color.G, laserGlow2.Color.B, Convert.ToByte(fadeAlpha2));
                            laserFire1.Color = new Color(laserFire1.Color.R, laserFire1.Color.G, laserFire1.Color.B, Convert.ToByte(fadeAlpha2));
                            laserFire2.Color = new Color(laserFire2.Color.R, laserFire2.Color.G, laserFire2.Color.B, Convert.ToByte(fadeAlpha2));
                        }
                        break;
                    }

                //Rilascio
                case (BulletLinearState.Destroy):
                    {
                        //Disattivazione
                        base.isActive = false;
                        break;
                    }
            }

            //Sfere di collisione (principale)
            collisionSpheres[0].Center = Vector3.Transform(laserFire1.Position, GeneratorMatrix);
            collisionSpheres[0].Radius = laserFire1.Radius;

            //Sfere di collisione (secondarie, generate a runtime)
            float position = laser1.ScaleX / collisionSpheres.Length;
            for (int i = 1; i < collisionSpheres.Length; i++)
            {
                Vector3 tempVector = new Vector3(laser1.PositionX + ((position * i)*2), laser1.PositionY, laser1.PositionZ);
                collisionSpheres[i].Center = Vector3.Transform(tempVector, GeneratorMatrix); ;
                collisionSpheres[1].Radius = Math.Min(laser1.ScaleX, laser1.ScaleY);
            }

            //Update billboard
            laser1.Update(gameTime, camera);
            laser2.Update(gameTime, camera);
            laserGlow1.Update(gameTime, camera); 
            laserGlow2.Update(gameTime, camera);
            laserFire1.Update(gameTime, camera);
            laserFire2.Update(gameTime, camera);
        }

        public override void Draw(LightEffect lightEffect)
        {
            //Disegna il billboard
            laser1.Draw(lightEffect);
            laser1.Draw(lightEffect);
            laser2.Draw(lightEffect);
            laser2.Draw(lightEffect);
            laserGlow1.Draw(lightEffect);
            laserGlow2.Draw(lightEffect);
            laserFire1.Draw(lightEffect);
            laserFire2.Draw(lightEffect);
        }

        public override bool Collide(XModel model)
        {
            //Collisione iniziale con la punta del beam
            for (int i = 0; i < collisionSpheres.Length; i++)
            {
                if (model.CheckCollision(collisionSpheres[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CollisionEffect()
        {
            //Smette di disegnare
            laser1.ToDraw = false;
            laser2.ToDraw = false;
            laserGlow1.ToDraw = false;
            laserGlow2.ToDraw = false;
            laserFire1.ToDraw = false;
            laserFire2.ToDraw = false;

            return false;
        }
    }
}
