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
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;

namespace DesdinovaEngineX
{
    public class BoneSelector
    {
        private ModelBone modelBone;
        private Matrix modelTransform;

        private Matrix movementMatrix;
        public Matrix MovementMatrix
        {
            get { return movementMatrix; }
            set { movementMatrix = value; }
        }

        private bool isCreated = true;
        public bool IsCreated
        {
            get { return isCreated; }
            set { isCreated = value; }
        }

        public BoneSelector(Model model, string boneName)
        {
            try
            {
                modelBone = model.Bones[boneName];
                modelTransform = modelBone.Transform;
                isCreated = true;
            }
            catch
            {
                isCreated = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isCreated) modelBone.Transform = movementMatrix * modelTransform;
        }

        ~BoneSelector()
        {
            //Rilascia le risorse
        }
    }
}
