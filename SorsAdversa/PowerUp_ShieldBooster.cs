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
    public class PowerUp_ShieldBooster : PowerUp
    {
        //Valore da aggiungere allo Score quando si prende il PU
        private int score = 0;
        protected int Score
        {
            get { return score; }
            set { score = value; }
        }

        //Valore da modificare del powerup
        private float shield = 0;
        protected float Shield
        {
            get { return shield; }
            set { shield = value; }
        }

        public PowerUp_ShieldBooster(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            //Impostazioni base
            base.Initialize("Content\\Model\\PowerUp\\PowerUp_ShieldBooster", "Content\\Texture\\Ray03", "NewWeapon", contentManager);
            base.Position = new Vector3(0, 0, 0);
            base.GlowColor = new Color(Color.Green.R, Color.Green.G, Color.Green.B, 254);
            base.GlowScale = new Vector2(3, 3);
            base.Time = 16000.0f;

            //Impostazioni specifiche
            this.score = 100;
            this.shield = 50.0f;
        }

        public override bool CollisionEffect(ref PlayerParameters playerDef, float startTime)
        {
            if (base.CollisionEffect(ref playerDef, startTime))
            {
                //Amenta i punti
                playerDef.Score = playerDef.Score + this.score;

                //Aumenta il valore relativo al tipo
                playerDef.ShieldEnergyMax = playerDef.ShieldEnergyMax + this.shield;
                playerDef.ShieldEnergy = playerDef.ShieldEnergyMax; //Imposta il valore di energia attuale come quello massimo

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
                playerDef.ShieldEnergyMax = playerDef.ShieldEnergyMax - this.shield;
                if (playerDef.ShieldEnergy > playerDef.ShieldEnergyMax)
                {
                    playerDef.ShieldEnergy = playerDef.ShieldEnergyMax;
                }

                //Ok
                return true;
            }
            return false;
        }
    }
}
