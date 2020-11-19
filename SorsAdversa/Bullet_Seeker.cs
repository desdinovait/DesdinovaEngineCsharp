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
    public class Bullet_Seeker : Bullet
    {
        //Stati
        public enum BulletSeekerState
        {
            Create,
            InFly,
            Destroy
        }

        private BulletSeekerState state = BulletSeekerState.Create;
        public BulletSeekerState State
        {
            get { return state; }
        }

        //Modello
        Quad3D billboard;

        //Colore generale
        private Color color;
        public Color Color
        {
            get { return color; }
            set 
            { 
                color = value;
                billboard.Color = color;
            }
        }

        //Scalatura generale
        private Vector2 scale;
        public Vector2 Scale
        {
            get { return scale; }
            set 
            { 
                scale = value;
                billboard.Scale = scale;
            }
        }

        //Posizione da seguire
        private Vector3 targetPosition = Vector3.Zero;
        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        float curveTime = 0.0f;
        private Vector3 randomVector = Vector3.Zero;

        public Bullet_Seeker(Texture2D texture, ContentManager contentManager)
        {
            //Crezione billboard
            billboard = new Quad3D(texture, Quad3D.Quad3DGeneration.Center, contentManager);
            billboard.IsBillboard = true;

            //Sfere collisione
            collisionSpheres = new BoundingSphere[1];
            collisionSpheres[0] = new BoundingSphere(billboard.Position, billboard.Radius);

            //Attivazione
            base.isActive = true;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            //Calcolo della posizione corrente rispetto alla posizione iniziale
            Vector3 distanceVector = billboard.Position - GeneratorMatrix.Translation;
            if (distanceVector.Length() >= this.DistanceLife)
            {
                //La vita del bullet è finita, procede con lo stato di distruzione
                this.state = BulletSeekerState.Destroy;
            }

            //Processa gli stati
            switch (state)
            {
                //Rilascio del bullet
                case (BulletSeekerState.Create):
                    {
                        //Genrazione del  vettore random per la generazione casuale dell'uscita dall'arma
                        Vector3 rnd = RandomHelper.GetRandomVector3(-25, 25);
                        randomVector.X = rnd.X;
                        randomVector.Y = rnd.Y;
                        randomVector.Z = rnd.Z;

                        //Calcolata la direzione cambia lo stato
                        state = BulletSeekerState.InFly;
                        break;
                    }

                //Rilascio del bullet
                case (BulletSeekerState.InFly):
                    {
                        //Se il target non è definito imposta il punto di contatto a 50 unità avanti
                        if (targetPosition == Vector3.Zero)
                        {
                            targetPosition.X = this.GeneratorMatrix.Translation.X + 50.0f;
                            targetPosition.Y = this.GeneratorMatrix.Translation.Y;
                            targetPosition.Z = this.GeneratorMatrix.Translation.Z;
                        }

                        //tempo percorrimento curva
                        curveTime = curveTime + 0.003f;

                        //Nuova posizione in base alla curva
                        base.position = MathHelper2.PointOnBezierCurve3(this.GeneratorMatrix.Translation, this.GeneratorMatrix.Translation + randomVector, targetPosition, curveTime);

                        //Posizione del billboard
                        billboard.Position = base.position;

                        break;
                    }

                case (BulletSeekerState.Destroy):
                    {
                        //Avvia la sequanza di fine della vita del bullet
                        //Fade, ecc...
                        base.isActive = false;
                        break;
                    }
            }

            //Sfera di collisione
            collisionSpheres[0].Center = billboard.Position;
            collisionSpheres[0].Radius = billboard.Radius;

            //Update billboard
            billboard.Update(gameTime, camera);
        }

        public override void Draw(LightEffect lightEffect)
        {
            //Disegna il billboard
            billboard.Draw(lightEffect);
        }

        public override bool Collide(XModel model)
        {
            if (model.CheckCollision(billboard.BoundingSphere))
            {
                return true;
            }
            else
            {
                return false;
            }
        }       

        public override bool CollisionEffect()
        {
            //Smette di disegnare
            billboard.ToDraw = false;

            return false;
        }
    }
}
