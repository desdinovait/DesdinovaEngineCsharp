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
    public class Weapon_HomingMissileLauncher : Weapon
    {
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Texture
        private Texture2D bulletTexture;


        public Weapon_HomingMissileLauncher(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            try
            {
                if (base.Initialize("Content\\Model\\Weapon\\Weapon", "MgShoot", contentManager))
                {
                    base.Name = "Homing Missile Launcher";
                    base.FireRate = 100;
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
                    //Bullet
                    Bullet_Seeker newBullet = new Bullet_Seeker(bulletTexture, contentManager);
                    newBullet.GeneratorMatrix = firegeneratorAnchor.FinalMatrix;
                    newBullet.TargetPosition = targetPosition;
                    newBullet.AngleXY = 0.0f;
                    newBullet.AngleXZ = 0.0f;
                    newBullet.Speed = 0.05f;
                    newBullet.Scale = new Vector2(3.0f, 3.0f);
                    newBullet.DistanceLife = 180.0f;
                    newBullet.Color = Color.LimeGreen;
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
