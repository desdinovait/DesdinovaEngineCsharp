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
    public class Weapon_PlasmaBolter : Weapon
    {
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Texture
        private Texture2D bulletTexture = null;


        public Weapon_PlasmaBolter(ContentManager contentManager, Scene parentScene ):base(parentScene)
        {
            try
            {
                if (base.Initialize("Content\\Model\\Weapon\\Weapon", "MgShoot", contentManager))
                {
                    base.Name = "Plasma Bolter";
                    base.FireRate = 1000;
                    base.UseShake = false;

                    this.bulletTexture = contentManager.Load<Texture2D>("Content\\Texture\\particle0");

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
                    //Bullet
                    Bullet_Linear newBullet = new Bullet_Linear(bulletTexture, contentManager);
                    newBullet.GeneratorMatrix = firegeneratorAnchor.FinalMatrix;
                    newBullet.AngleXY = 0.0f;
                    newBullet.AngleXZ = 0.0f;
                    newBullet.Speed = 0.05f;
                    newBullet.Scale = new Vector2(3.0f, 3.0f);
                    newBullet.DistanceLife = 180.0f;
                    newBullet.Color = new Color(0, 255, 0, 254);
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
