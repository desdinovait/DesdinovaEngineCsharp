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
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;

namespace SorsAdversa
{


    public class BulletPoint_Seeker : Quad3D, IBullet
    {
        //Stati
        public enum BulletSeekerState
        {
            Create,
            Deploying,
            Targeting,
            Releasing,
            Destroy
        }

        //Stato di animazione
        private BulletSeekerState state = BulletSeekerState.Create;
        public BulletSeekerState State
        {
            get { return state; }
        }

        //Attivo
        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //Posizione iniziale del bullet
        private Vector3 startPosition = Vector3.Zero;
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        //Posizione da seguire
        private Vector3 targetPosition = Vector3.Zero;
        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        //Direzione
        protected Vector3 direction = Vector3.One;
        public Vector3 Direction
        {
            get { return direction; }
        }

        //Tempo da mantenere prima di lasciare il target
        private float seekLife = 2000.0f;
        public float SeekLife
        {
            get { return seekLife; }
            set { seekLife = value; }
        }

        //Velocità di rilascio
        protected float speedDeploy = 0.5f;
        public float SpeedDeploy
        {
            get { return speedDeploy; }
            set { speedDeploy = value; }
        }

        //Velocità di targetting
        protected float speedTargeting = 0.5f;
        public float SpeedTargeting
        {
            get { return speedTargeting; }
            set { speedTargeting = value; }
        }

        //Angolo formato tra l'asse X e l'asse Y
        protected float angleXY = 0.0f;
        public float AngleXY
        {
            get { return angleXY; }
            set
            {
                angleXY = value;

                //Vettore direzione (in base ai 2 angoli XY e XZ)
                direction.X = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Cos(MathHelper.ToRadians(angleXZ));
                direction.Y = (float)Math.Sin(MathHelper.ToRadians(angleXY));
                direction.Z = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Sin(MathHelper.ToRadians(angleXZ));

                //Matrice di rotazione del punto generatore (attraverso il quaternione di rotazione)
                Matrix mat = Matrix.CreateFromQuaternion(externRotation);
                //Purtroppo non è possible moltiplicare un vettore per un quaternione quindi è necessario passare attraverso la multiplicazione con matrice
                direction = Vector3.Transform(direction, mat);
                if (direction.LengthSquared() != 0)
                    direction.Normalize();
                direction.Normalize();
            }
        }

        //Angolo formato tra l'asse X e l'asse Z
        protected float angleXZ = 0.0f;
        public float AngleXZ
        {
            get { return angleXZ; }
            set
            {
                angleXZ = value;

                //Vettore direzione (in base ai 2 angoli XY e XZ)
                direction.X = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Cos(MathHelper.ToRadians(angleXZ));
                direction.Y = (float)Math.Sin(MathHelper.ToRadians(angleXY));
                direction.Z = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Sin(MathHelper.ToRadians(angleXZ));

                //Matrice di rotazione del punto generatore (attraverso il quaternione di rotazione)
                Matrix mat = Matrix.CreateFromQuaternion(externRotation);
                //Purtroppo non è possible moltiplicare un vettore per un quaternione quindi è necessario passare attraverso la multiplicazione con matrice
                direction = Vector3.Transform(direction, mat);
                if (direction.LengthSquared() != 0)
                    direction.Normalize();
                direction.Normalize();
            }
        }

        //Rotazione del quaternione
        protected Quaternion externRotation = Quaternion.Identity;
        public Quaternion ExternRotation
        {
            get { return externRotation; }
            set
            {
                externRotation = value;

                //Vettore direzione (in base ai 2 angoli XY e XZ)
                direction.X = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Cos(MathHelper.ToRadians(angleXZ));
                direction.Y = (float)Math.Sin(MathHelper.ToRadians(angleXY));
                direction.Z = (float)Math.Cos(MathHelper.ToRadians(angleXY)) * (float)Math.Sin(MathHelper.ToRadians(angleXZ));

                //Matrice di rotazione del punto generatore (attraverso il quaternione di rotazione)
                Matrix mat = Matrix.CreateFromQuaternion(externRotation);
                //Purtroppo non è possible moltiplicare un vettore per un quaternione quindi è necessario passare attraverso la multiplicazione con matrice
                direction = Vector3.Transform(direction, mat);
                direction.Normalize();
            }
        }

        //Distanza percorribile
        protected float distanceLife = 100.0f;
        public float DistanceLife
        {
            get { return distanceLife; }
            set { distanceLife = value; }
        }

        //Distanza percorsa
        protected float distanceCovered = 0.0f;
        public float DistanceCovered
        {
            get
            {
                Vector3 len = base.Position - startPosition;
                return len.Length();
            }
        }

        public BulletPoint_Seeker(string assetName, ContentManager contentManager)
            : base(assetName, Quad3DGeneration.Center, contentManager)
        {
            isActive = true;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            /*Calcolo della posizione corrente rispetto alla posizione iniziale
            if (base.DistanceCovered >= base.distanceLife)
            {
                //La vita del bullet è finita, procede con lo stato di distruzione
                state = BulletSeekerState.Destroy;
            }*/

            //Processa gli stati
            switch (state)
            {
                //Rilascio del bullet
                case (BulletSeekerState.Create):
                    {
                        //Calcola una direzione random
                        Random rnd = new Random((int)gameTime.TotalGameTime.TotalMilliseconds);
                        this.angleXY = (float)rnd.Next(-110, 110);
                        this.angleXZ = (float)rnd.Next(-20, 20);

                        //Calcolata la direzione cambia lo stato
                        state = BulletSeekerState.Deploying;
                        break;
                    }

                //Rilascio del bullet
                case (BulletSeekerState.Deploying):
                    {
                        //Calcolo velocità e direzione in base all'angolo
                        Vector3 distance = (direction * speedDeploy) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        //Calcolo della posizione corrente rispetto alla posizione iniziale
                        if (this.distanceCovered <= this.distanceLife / 10.0f)
                        {
                            base.Position = (base.Position + distance);
                        }
                        else
                        {
                            //A 1/10 della vita del bullet cambia lo stato iniziando a targettare
                            state = BulletSeekerState.Targeting;
                        }
                        break;
                    }


                //Avvicinamento del target
                case (BulletSeekerState.Targeting):
                    {
                        //Calcolo velocità e direzione in base ad un target
                        Vector3 targetDistance = targetPosition - base.Position;
                        if (targetDistance.LengthSquared() != 0)
                            targetDistance.Normalize();
                        Vector3 distance = (targetDistance * speedTargeting) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        base.Position = (base.Position + distance);

                        //Se raggiunge il punto dell'obiettivo continua oltre (significa che non ha ancora colpito nulla)
                        if (Vector3Helper.CompareVectors(targetPosition, base.Position, 1))
                        {
                            //Rilascia il target (cambia stato)
                            state = BulletSeekerState.Releasing;
                        }


                        /*if (base.DistanceCovered >= distanceLife / 1.5f)
                        {
                            //Rilascia il target (cambia stato)
                            state = BulletSeekerState.Releasing;
                        }*/
                        break;
                    }

                //Rilascio del target
                case (BulletSeekerState.Releasing):
                    {
                        //!!!!!!!!!!!!!!!!!!!!!!!!!
                        // Rilascia il target e prosegue dritto lungo la direttrice (da fare)
                        //!!!!!!!!!!!!!!!!!!!!!!!!!
                        //????????????????????????????????????????????

                        Vector3 velocity = this.direction - base.Position;
                        if (velocity.LengthSquared() != 0)
                            velocity.Normalize();
                        Vector3 distance = (velocity * speedTargeting) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        base.Position = (base.Position + distance);

                        break;
                    }


                case (BulletSeekerState.Destroy):
                    {
                        //Avvia la sequanza di fine della vita del bullet
                        //Fade, ecc...
                        isActive = false;
                        break;
                    }
            }

            //Update base
            base.Update(gameTime, camera);
        }

        public void CollisionEffect()
        {
            base.ToDraw = false;
        }
    }
}
