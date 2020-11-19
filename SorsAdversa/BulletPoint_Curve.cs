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
    public class BulletPoint_Curve : Quad3D, IBullet
    {
        //Stati
        public enum BulletCurveState
        {
            Create,
            InFly,
            Destroy
        }

        //Stato corrente
        private BulletCurveState state = BulletCurveState.Create;
        public BulletCurveState State
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

        //Velocità
        protected float speed = 0.5f;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //Posizione da seguire
        private Vector3 targetPosition = Vector3.Zero;
        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
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

        float curveTime = 0.0f;
        private Vector3 randomVector = Vector3.Zero;


        public BulletPoint_Curve(string assetName, ContentManager contentManager)
            : base(assetName, Quad3DGeneration.Center, contentManager)
        {
            isActive = true;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {

            //Calcolo della posizione corrente rispetto alla posizione iniziale
            if (this.distanceCovered >= this.distanceLife)
            {
                //La vita del bullet è finita, procede con lo stato di distruzione
                state = BulletCurveState.Destroy;
            }

            //Processa gli stati
            switch (state)
            {
                //Creazione
                case (BulletCurveState.Create):
                    {
                        //genrazione del  vettore random per la generazione casuale dell'uscita dall'arma
                        randomVector.X = RandomHelper.GetRandomVector3(-35, 35).X;
                        randomVector.Y = RandomHelper.GetRandomVector3(-35, 35).Y;
                        randomVector.Z = RandomHelper.GetRandomVector3(0, 0).Z;

                        //Creazione
                        state = BulletCurveState.InFly;
                        break;
                    }

                //Esecuzione
                case (BulletCurveState.InFly):
                    {
                        //Se il target non è definito imposta il punto di contatto a 50 unità avanti
                        if (targetPosition == Vector3.Zero)
                        {
                            targetPosition.X = this.startPosition.X + 50.0f;
                            targetPosition.Y = this.startPosition.Y;
                            targetPosition.Z = this.startPosition.Z;
                        }

                        //tempo percorrimento curva
                        curveTime = curveTime + 0.003f;
                        //Nuova posizione in base alla curva
                        base.Position = MathHelper2.PointOnBezierCurve3(this.startPosition, this.startPosition + randomVector, targetPosition, curveTime);

                        break;
                    }

                //Rilascio
                case (BulletCurveState.Destroy):
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


