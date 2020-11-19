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
    public class BulletList
    {
        //Elementi della lista
        private Bullet[] elements;
        public Bullet[] Elements
        {
            get { return elements; }
        }
	
        //Linee di collisione
        private Lines3D collisionLines;

        //Lunghezza
        public int Capacity
        {
            get { return elements.Length; }
        }

        //Elementi attivi correntemente
        private int activeElements;
        public int ActiveElements
        {
            get { return activeElements; }
            set { activeElements = value; }
        }

        public BulletList(int capacity)
        {
            elements = new Bullet[capacity];
            collisionLines = new Lines3D(capacity);
        }

        public void Add(Bullet newBullet)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null)
                {
                    elements[i] = newBullet;
                    collisionLines.AddLine(new Line3D(newBullet.GeneratorMatrix.Translation, newBullet.Position, Color.White, Color.White));
                    return;
                }
            }
        }

        public void Remove(int currentBullet)
        {
            elements[currentBullet] = null;
            return;
        }

        public void Remove(Bullet currentBullet)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == currentBullet)
                {
                    elements[i] = null;
                    collisionLines[i] = new Line3D();
                    return;
                }
            }
        }

        public void Update(Vector3 trackPosition, GameTime gameTime, Camera camera)
        {
            activeElements = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] != null)
                {
                    //collisionLines[i] = new Line3D(elements[i].GeneratorMatrix.Translation, Vector3.Transform(elements[i].Position, elements[i].GeneratorMatrix) , Color.White, Color.White);
                    //collisionLines.Update(gameTime, camera);

                    elements[i].TrackPosition = trackPosition;
                    elements[i].Update(gameTime, camera);
                    if (elements[i].IsActive == false)
                    {
                        //Rimuove il bullet dalla lista perchè non più attivo
                        elements[i] = null;
                        collisionLines[i] = new Line3D();
                    }
                    else
                    {
                        //Aggiorna il conteggio dei bullet attivi
                        activeElements = activeElements + 1;
                    }
                }
            }
        }

        public void Draw(LightEffect lightEffect)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] != null)
                {
                    elements[i].Draw(lightEffect);
                }
            }

            //collisionLines.Draw(lightEffect);
        }


        public bool Collide(XModel model)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] != null)
                {
                    //Collide il bullet con il modello
                    if (elements[i].Collide(model))
                    {
                        //Rimuove il bullet dalla lista
                        elements[i] = null;
                        collisionLines[i] = new Line3D();
                        return true;
                    }
                }
            }
            return false;
        }


    }

}
