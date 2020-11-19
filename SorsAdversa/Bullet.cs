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
    public abstract class Bullet
    {
        //Attivo
        protected bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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

        //Posizione attuale
        protected Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Velocità
        protected float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //Posizione da aggangiare
        private Vector3 trackPosition;
        public Vector3 TrackPosition
        {
            get { return trackPosition; }
            set { trackPosition = value; }
        }

        //Collisioni
        protected BoundingSphere[] collisionSpheres;
        public BoundingSphere[] CollisionSpheres
        {
            get { return collisionSpheres; }
        }

        //Matrice esterna
        private Matrix generatorMatrix;
        public Matrix GeneratorMatrix
        {
            get { return generatorMatrix; }
            set { generatorMatrix = value; }
        }
	


        //Metodi da implementare necessariamente
        public abstract void Update(GameTime gameTime, Camera camera);
        public abstract void Draw(LightEffect lightEffect);
        public abstract bool Collide(XModel model);
        public abstract bool CollisionEffect();

    }
}
