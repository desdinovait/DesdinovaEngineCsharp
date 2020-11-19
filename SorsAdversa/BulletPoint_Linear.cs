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
    public class BulletPoint_Linear : Quad3D, IBullet
    {
        //Stati
        public enum BulletLinearState
        {
            Create,
            InFly,
            Destroy
        }

        //Stato
        private BulletLinearState state = BulletLinearState.Create;
        public BulletLinearState State
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
        private Vector3 startPosition;
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        //Velocità
        protected float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //Angolo formato tra l'asse X e l'asse Y
        protected float angleXY;
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
        protected float angleXZ;
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
        protected Quaternion externRotation;
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

        //Direzione
        protected Vector3 direction;
        public Vector3 Direction
        {
            get { return direction; }
        }

        //Distanza percorribile
        protected float distanceLife;
        public float DistanceLife
        {
            get { return distanceLife; }
            set { distanceLife = value; }
        }

        //Distanza percorsa
        protected float distanceCovered;
        public float DistanceCovered
        {
            get
            {
                Vector3 len = base.Position - startPosition;
                return len.Length();
            }
        }
	
        public BulletPoint_Linear(string assetName, ContentManager contentManager)
            : base(assetName, Quad3DGeneration.Center, contentManager)
        {
            isActive = true;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            //Calcolo della posizione corrente rispetto alla posizione iniziale
            if (this.DistanceCovered >= this.DistanceLife)
            {
                //La vita del bullet è finita, procede con lo stato di distruzione
                this.state = BulletLinearState.Destroy;
            }

            //Processa gli stati
            switch (this.state)
            {
                //Creazione
                case (BulletLinearState.Create):
                    {
                        //Creazione
                        this.state = BulletLinearState.InFly;
                        break;
                    }

                //Esecuzione
                case (BulletLinearState.InFly):
                    {
                        //calcola la distanza da parcorrere in base alla velocità
                        Vector3 distance = (direction * speed) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        //Nuova posizione in base alla distanza percorsa
                        base.Position = (startPosition + distance);

                        break;
                    }

                //Rilascio
                case (BulletLinearState.Destroy):
                    {
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

