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
    public class Weapon_5WayMachineGun : Weapon
    {
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Texture
        private Texture2D bulletTexture;

        public Weapon_5WayMachineGun(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            try
            {
                if (base.Initialize("Content\\Model\\Weapon\\Weapon", "MgShoot", contentManager))
                {
                    base.Name = "5-Way Machine Gun";
                    base.FireRate = 200;
                    base.UseShake = false;

                    this.bulletTexture = contentManager.Load<Texture2D>("Content\\Texture\\Flare04");

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
                    //Bullets
                    for (int i = 0; i < 5; i++)
                    {
                        Bullet_Linear newBullet = new Bullet_Linear(bulletTexture, contentManager);
                        newBullet.GeneratorMatrix = firegeneratorAnchor.FinalMatrix;
                        newBullet.AngleXY = 0.0f;
                        newBullet.AngleXZ = 0.0f;
                        newBullet.Speed = 0.05f;
                        if (i == 0) newBullet.AngleXY = 0.0f;
                        else if (i == 1) newBullet.AngleXY = 40.0f;
                        else if (i == 2) newBullet.AngleXY = -40.0f;
                        else if (i == 3) newBullet.AngleXZ = 40.0f;
                        else if (i == 4) newBullet.AngleXZ = -40.0f;
                        newBullet.Scale = new Vector2(2.0f, 2.0f);
                        newBullet.DistanceLife = 180.0f;
                        newBullet.Color = new Color(0,0,255,254);
                        Scene_Level.bulletListByPlayers.Add(newBullet);
                    }

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
