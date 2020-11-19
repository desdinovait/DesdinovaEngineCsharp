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
    public class BulletArea_Beam : IBullet
    {
        //Stati 
        public enum BulletAreaBeamState
        {
            Create,
            InFly,
            Destroy
        }

        //Stato corrente
        private BulletAreaBeamState state = BulletAreaBeamState.Create;
        public BulletAreaBeamState State
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
            get { return distanceCovered; }
            set { distanceCovered = value; }
        }

        //Vertici
        Quad3D laser1;
        Quad3D laser2;
        Quad3D laserGlow2;
        Quad3D laserGlow3;
        Quad3D laserFire1;
        Quad3D laserFire2;
        float scaleX = 0.0f;


        private float radius;
        public float Radius
        {
            get { return radius; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
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


        public BulletArea_Beam(string assetName, ContentManager contentManager)
        {
            laser1 = new Quad3D("Content\\Texture\\Laser", Quad3D.Quad3DGeneration.Left, contentManager);
            laser1.Vertex1Color = Color.CornflowerBlue;
            laser1.Vertex2Color = Color.CornflowerBlue;
            laser1.Vertex3Color = Color.CornflowerBlue;
            laser1.Vertex4Color = Color.CornflowerBlue;
            laser1.IsBillboard = false;

            laser2 = new Quad3D("Content\\Texture\\Laser", Quad3D.Quad3DGeneration.Left, contentManager);
            laser2.Vertex1Color = Color.CornflowerBlue;
            laser2.Vertex2Color = Color.CornflowerBlue;
            laser2.Vertex3Color = Color.CornflowerBlue;
            laser2.Vertex4Color = Color.CornflowerBlue;
            laser2.IsBillboard = false;

            laserGlow2 = new Quad3D("Content\\Texture\\LaserGlow", Quad3D.Quad3DGeneration.Center, contentManager);
            laserGlow2.Vertex1Color = Color.CornflowerBlue;
            laserGlow2.Vertex2Color = Color.CornflowerBlue;
            laserGlow2.Vertex3Color = Color.CornflowerBlue;
            laserGlow2.Vertex4Color = Color.CornflowerBlue;
            laserGlow2.IsBillboard = true;

            laserGlow3 = new Quad3D("Content\\Texture\\LaserGlow", Quad3D.Quad3DGeneration.Center, contentManager);
            laserGlow3.Vertex1Color = Color.White;
            laserGlow3.Vertex2Color = Color.White;
            laserGlow3.Vertex3Color = Color.White;
            laserGlow3.Vertex4Color = Color.White;
            laserGlow3.IsBillboard = true;

            laserFire1 = new Quad3D("Content\\Texture\\LaserGlow", Quad3D.Quad3DGeneration.Center, contentManager);
            laserFire1.Vertex1Color = Color.White;
            laserFire1.Vertex2Color = Color.White;
            laserFire1.Vertex3Color = Color.BlueViolet;
            laserFire1.Vertex4Color = Color.BlueViolet;
            laserFire1.IsBillboard = true;

            laserFire2 = new Quad3D("Content\\Texture\\LaserGlow", Quad3D.Quad3DGeneration.Center, contentManager);
            laserFire2.Vertex1Color = Color.White;
            laserFire2.Vertex2Color = Color.White;
            laserFire2.Vertex3Color = Color.BlueViolet;
            laserFire2.Vertex4Color = Color.BlueViolet;
            laserFire2.IsBillboard = true;

            isActive = true;
        }



        public void Update(GameTime gameTime, Camera camera)
        {
            if (scaleX < this.distanceLife)
            {
                scaleX = scaleX + 0.1f;
            }

            laser1.Scale = new Vector2(scaleX, 1.62f);
            laser1.Position = startPosition;
            laser1.RotationX = 0;
            laser1.Update(gameTime, camera);

            laser2.Scale = new Vector2(scaleX, 1.0f);
            laser2.Position = startPosition;
            laser2.RotationX = 90;
            laser2.Update(gameTime, camera);

            Vector3 startPosition2 = startPosition;
            startPosition2.Z = startPosition.Z + 0.01f;
            laserGlow2.Scale = new Vector2(5.0f, 5.0f);
            laserGlow2.Position = startPosition2;
            laserGlow2.Update(gameTime, camera);

            Vector3 startPosition3 = startPosition;
            startPosition3.Z = startPosition.Z + 0.05f;
            laserGlow3.Scale = new Vector2(4.0f, 4.0f);
            laserGlow3.Position = startPosition3;
            laserGlow3.Update(gameTime, camera);

            Vector3 startPosition4 = startPosition;
            startPosition4.X = startPosition.X + scaleX * 2;
            laserFire1.Scale = new Vector2(4.0f, 4.0f);
            laserFire1.Position = startPosition4;
            laserFire1.Update(gameTime, camera);

            Vector3 startPosition5 = startPosition;
            startPosition5.X = startPosition.X + scaleX * 2;
            laserFire2.Scale = new Vector2(3.0f, 3.0f);
            laserFire2.Position = startPosition5;
            laserFire2.Update(gameTime, camera);
        }


        public void Draw(LightEffect lightEffect, SpriteBlendMode blendMode)
        {
            laser1.Draw(lightEffect, blendMode);
            laser1.Draw(lightEffect, blendMode);
            laser2.Draw(lightEffect, blendMode);
            laser2.Draw(lightEffect, blendMode);
            laserGlow2.Draw(lightEffect, blendMode);
            laserGlow3.Draw(lightEffect, blendMode);
            laserFire1.Draw(lightEffect, blendMode);
            laserFire2.Draw(lightEffect, blendMode);
        }


        public void CollisionEffect()
        {
            //base.ToDraw = false;
        }

    }
}
