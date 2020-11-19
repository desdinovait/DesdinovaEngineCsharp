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
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline.Helpers
{
    public class CollisionHelper
    {
        public static bool Collision(BoundingSphere[] bsArray1, BoundingSphere[] bsArray2)
        {
            for (int i = 0; i < bsArray1.Length; i++)
            {
                for (int m = 0; m < bsArray2.Length; m++)
                {
                    if (bsArray1[i].Intersects(bsArray2[m]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool Collision(BoundingSphere[] bsArray, BoundingSphere bs)
        {
            for (int i = 0; i < bsArray.Length; i++)
            {
                if (bs.Intersects(bsArray[i]))
                {
                    return true;
                }
            }
            return false;
        }        
        
        public static bool Collision(BoundingSphere bs1, BoundingSphere bs2)
        {
            if (bs1.Intersects(bs2))
            {
                return true;
            }
            return false;
        }

        public static BoundingSphere[] GenerateArray(BoundingSphere[] presentArray, BoundingSphere[] linkArray)
        {
            BoundingSphere[] newArray = new BoundingSphere[ presentArray.Length + linkArray.Length];
            for (int i = 0; i < presentArray.Length; i++)
            {
                newArray[i] = presentArray[i];
            }
            for (int i = 0; i < linkArray.Length; i++)
            {
                newArray[(presentArray.Length - 1) + i] = linkArray[i];
            }

            return newArray;
        }

        //Aggiungere altre collisoni (ray, box, ecc)
        //...

    }
}
