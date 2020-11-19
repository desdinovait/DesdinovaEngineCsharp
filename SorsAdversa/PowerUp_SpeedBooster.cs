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
    public class PowerUp_SpeedBooster : PowerUp
    {
        //Valore da aggiungere allo Score quando si prende il PU
        private int score = 0;
        protected int Score
        {
            get { return score; }
            set { score = value; }
        }

        //Valore da modificare del powerup
        private float speed = 0;
        protected float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public PowerUp_SpeedBooster(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            //Impostazioni base
            base.Initialize("Content\\Model\\PowerUp\\PowerUp_SpeedBooster", "Content\\Texture\\Ray03", "NewWeapon", contentManager);
            base.Position = new Vector3(0, 0, 0);
            base.GlowColor = new Color(Color.Red.R, Color.Red.G, Color.Red.B, 254);
            base.GlowScale = new Vector2(3, 3);
            base.Time = 16000.0f;

            //Impostazioni specifiche
            this.score = 200;
            this.speed = 2.0f; //Moltiplicatore di velocità
        }

        public override bool CollisionEffect(ref PlayerParameters playerDef, float startTime)
        {
            if (base.CollisionEffect(ref playerDef, startTime))
            {
                //Amenta i punti
                playerDef.Score = playerDef.Score + this.score;

                //Aumenta la velocità
                playerDef.Velocity = playerDef.Velocity * this.speed;
            
                //Ok
                return true;
            }
            return false;
        }

        public override bool CollisionDeEffect(ref PlayerParameters playerDef)
        {
            if (base.CollisionDeEffect(ref playerDef))
            {
                //Decrementa il valore per riportarlo al normale
                playerDef.Velocity = playerDef.Velocity / this.speed;

                //Ok
                return true;
            }
            return false;
        }
    }
}
