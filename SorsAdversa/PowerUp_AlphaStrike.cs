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
    public class PowerUp_AlphaStrike : PowerUp
    {
        //Valore da aggiungere allo Score quando si prende il PU
        private int score = 0;
        protected int Score
        {
            get { return score; }
            set { score = value; }
        }

        public PowerUp_AlphaStrike(ContentManager contentManager, Scene parentScene):base(parentScene)
        {
            //Impostazioni base
            base.Initialize("Content\\Model\\PowerUp\\PowerUp_AlphaStrike", "Content\\Texture\\Ray03", "NewWeapon", contentManager);
            base.Position = new Vector3(0, 0, 0);
            base.GlowColor = new Color(Color.Goldenrod.R, Color.Goldenrod.G, Color.Goldenrod.B, 254);
            base.GlowScale = new Vector2(3, 3);
            base.Time = 10000.0f;

            //Impostazioni specifiche
            this.score = 500;
        }

        public override bool CollisionEffect(ref PlayerParameters playerDef, float startTime)
        {
            if (base.CollisionEffect(ref playerDef, startTime))
            {
                //Amenta i punti
                playerDef.Score = playerDef.Score + this.score;

                //Colpo multiplo...
                playerDef.AllWeapons = true;

                //Ok
                return true;
            }
            return false;
        }

        public override bool CollisionDeEffect(ref PlayerParameters playerDef)
        {
            if (base.CollisionDeEffect(ref playerDef))
            {
                //Reimposta il valore
                playerDef.AllWeapons = false;

                //Ok
                return true;
            }
            return false;
        }
    }
}
