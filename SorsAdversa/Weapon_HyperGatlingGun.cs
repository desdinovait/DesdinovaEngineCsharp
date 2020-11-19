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
    public class Weapon_HyperGatlingGun : Weapon
    {
        //Creazione
        private bool isCreated = false;
        public new bool IsCreated
        {
            get { return isCreated; }
        }

        //Texture
        private Texture2D bulletTexture;

        //Varibili effetto
        private bool positionY = false;

        public Weapon_HyperGatlingGun(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            try
            {
                if (Initialize("Content\\Model\\Weapon\\Weapon", "PlasmaShoot", contentManager))
                {
                    base.Name = "Hyper Gatling Gun";
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
                    positionY = !positionY;

                    //Bullet
                    for (int i = 0; i < 300; i++)  //Prova multipla
                    {
                        Bullet_Linear newBullet = new Bullet_Linear(bulletTexture, contentManager);
                        newBullet.GeneratorMatrix = firegeneratorAnchor.FinalMatrix;
                        newBullet.AngleXY = RandomHelper.GetRandomFloat(90, -90);
                        newBullet.AngleXZ = 0;
                        newBullet.Speed = 0.05f;
                        newBullet.Scale = new Vector2(1.0f, 1.0f);
                        newBullet.DistanceLife = 180.0f;
                        newBullet.Color = new Color(255, 0, 0, 254);

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
