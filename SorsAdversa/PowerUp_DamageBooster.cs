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
    public class PowerUp_DamageBooster : PowerUp
    {
        //Valore da aggiungere allo Score quando si prende il PU
        private int score = 0;
        protected int Score
        {
            get { return score; }
            set { score = value; }
        }

        //Valore da modificare del powerup
        private float damage = 0;
        protected float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public PowerUp_DamageBooster(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            //Impostazioni base
            base.Initialize("Content\\Model\\PowerUp\\PowerUp_DamageBooster", "Content\\Texture\\Ray03", "NewWeapon", contentManager);
            base.Position = new Vector3(0, 0, 0);
            base.GlowColor = Color.BlueViolet;
            base.GlowColor = new Color(Color.BlueViolet.R, Color.BlueViolet.G, Color.BlueViolet.B, 254);
            base.GlowScale = new Vector2(3, 3);
            base.Time = 16000.0f;

            //Impostazioni specifiche
            this.score = 100;
            this.damage = 0.5f;
        }

        public override bool CollisionEffect(ref PlayerParameters playerDef, float startTime)
        {
            if (base.CollisionEffect(ref playerDef, startTime))
            {

                //Amenta i punti
                playerDef.Score = playerDef.Score + this.score;

                //Aumenta il valore relativo al tipo
                playerDef.FirePower = playerDef.FirePower + this.damage;

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
                playerDef.FirePower = playerDef.FirePower - this.damage;

                //Ok
                return true;
            }
            return false;
        }
    }
}
