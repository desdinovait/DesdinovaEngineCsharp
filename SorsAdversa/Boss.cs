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
    public class Boss : Enemy
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Il boss deriva da Enemy impostando il corpo principale, in più è composto da una lista di nemici 
        //che definiscono le parti indipendenti del nemico (es: braccia, armi, torrette, ecc)
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Lista delle parti aggiuntive (armi, torrette, ecc)
        //List<Enemy> parts;


        /*public Boss(string assetName, ContentManager contentManager) : base(assetName, contentManager)
        {
            parts = new List<Enemy>();
        }*/

        public Boss(Scene parentScene):base(parentScene)
        {

        }
    }
}
