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
    public class Bullet_Linear : Bullet
    {
        //Stati
        public enum BulletLinearState
        {
            Create,
            InFly,
            Destroy
        }
        private BulletLinearState state = BulletLinearState.Create;
        public BulletLinearState State
        {
            get { return state; }
        }

        //Modello
        private Quad3D billboard;

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

        private float velocity = 0.0f;

        public Bullet_Linear(Texture2D texture, ContentManager contentManager)
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
            //Processa gli stati
            switch (state)
            {
                //Creazione
                case (BulletLinearState.Create):
                    {
                        //Creazione
                        state = BulletLinearState.InFly;
                        break;
                    }

                //Esecuzione
                case (BulletLinearState.InFly):
                    {
                        //Velocità
                        velocity = velocity + (speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

                        //Posizone
                        Vector3 newPosition = Vector3.Zero;
                        newPosition.X = direction.X * velocity;
                        newPosition.Y = direction.Y * velocity;
                        newPosition.Z = direction.Z * velocity;

                        //Calcola la distanza da parcorrere in base alla velocità
                        billboard.Position = newPosition;
                        billboard.ExternalMatrix = GeneratorMatrix;
                        billboard.Update(gameTime, camera);

                        //Nuova posizione in base alla velocità
                        base.position = newPosition;

                        //Sfera di collisione
                        collisionSpheres[0].Center = Vector3.Transform(billboard.Position, GeneratorMatrix);
                        collisionSpheres[0].Radius = billboard.Radius;

                        //Calcolo della posizione corrente rispetto alla posizione iniziale
                        Vector3 distanceVector = Vector3.Zero;
                        distanceVector.X = newPosition.X - GeneratorMatrix.Translation.X;
                        distanceVector.Y = newPosition.Y - GeneratorMatrix.Translation.Y;
                        distanceVector.Z = newPosition.Z - GeneratorMatrix.Translation.Z;
                        if (distanceVector.Length() >= this.DistanceLife)
                        {
                            //La vita del bullet è finita, procede con lo stato di distruzione
                            this.state = BulletLinearState.Destroy;
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

        }

        public override void Draw(LightEffect lightEffect)
        {
            //Disegna il billboard
            if (state == BulletLinearState.InFly)
            {
                billboard.Draw(lightEffect);
            }
        }

        public override bool Collide(XModel model)
        {
            Ray collisionRay = new Ray(GeneratorMatrix.Translation, direction);

            float? collisionDistance = 0;
            collisionDistance = collisionRay.Intersects(model.BoundingSphere);
            if (collisionDistance == null)
            {
                return false;
            }
            else
            {
                Vector3 diffVector = Vector3.Transform(position, GeneratorMatrix) - GeneratorMatrix.Translation;
                float diffDistance = diffVector.Length();
                if ( diffDistance > collisionDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
