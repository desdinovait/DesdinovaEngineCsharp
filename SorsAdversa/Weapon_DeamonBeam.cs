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
    public class Weapon_DeamonBeam : Weapon
    {
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Texture
        private Texture2D bulletTexture1;
        private Texture2D bulletTexture2;
        private Texture2D bulletTexture3;

        public Weapon_DeamonBeam(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            try
            {
                if (base.Initialize("Content\\Model\\Weapon\\Weapon", "EMP", contentManager))
                {
                    base.Name = "Laser Cutter";
                    base.FireRate = 1000;
                    base.UseShake = false;

                    //this.bulletTexture1 = contentManager.Load<Texture2D>("Content\\Texture\\particle0");
                    //this.bulletTexture2 = contentManager.Load<Texture2D>("Content\\Texture\\particle0");
                    //this.bulletTexture3 = contentManager.Load<Texture2D>("Content\\Texture\\particle0");

                    this.bulletTexture1 = contentManager.Load<Texture2D>("Content\\Texture\\Laser");
                    this.bulletTexture2 = contentManager.Load<Texture2D>("Content\\Texture\\LaserGlow");
                    this.bulletTexture3 = contentManager.Load<Texture2D>("Content\\Texture\\LaserGlow");

                    isCreated = true;
                }
                else
                {
                    isCreated = false;
                }
            }
            catch
            {
                isCreated = false;
            }
        }

        public override bool Fire(Vector3 targetPosition, ContentManager contentManager)
        {
            if (base.Fire(targetPosition, contentManager))
            {
                if (isCreated)
                {
                    //Posizione di partenza del colpo (proviene dall'ancora)
                    Vector3 startPosition = firegeneratorAnchor.FinalMatrix.Translation;

                    //Bullet
                    Bullet_Beam newBullet = new Bullet_Beam(bulletTexture1, bulletTexture2, bulletTexture3, 20, contentManager);
                    newBullet.GeneratorMatrix = firegeneratorAnchor.FinalMatrix;
                    newBullet.Speed = 1.0f;
                    newBullet.LoadTime = 3000;
                    newBullet.WaitTime = 5000;
                    newBullet.DistanceLife = 100.0f;
                    newBullet.LaserColor = new Color(Color.CornflowerBlue.R, Color.CornflowerBlue.G, Color.CornflowerBlue.B, 254);
                    newBullet.FireColor = new Color(Color.Salmon.R, Color.Salmon.G, Color.Salmon.B, 254);
                    newBullet.DropColor = new Color(Color.CornflowerBlue.R, Color.CornflowerBlue.G, Color.CornflowerBlue.B, 254);
                    Scene_Level.bulletListByPlayers.Add(newBullet);

                    return true;
                }
            }
            return false;
        }

        public override void ResetFire()
        {
            base.ResetFire();
        }
    }
}
