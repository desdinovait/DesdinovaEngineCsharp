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


namespace DesdinovaModelPipeline
{
    public class XModelAnchor : EngineObject
    {
        //Modello associato
        private XModel parentModel = null;

        //Matrice principale
        private Matrix absoluteMatrix = Matrix.Identity;

        //Ancora relativa
        private Matrix relativeMatrix = Matrix.Identity;
        public Matrix RelativeMatrix
        {
            get { return relativeMatrix; }
            set { relativeMatrix = value; }
        }

        //Rotazione
        private Vector3 rotation = Vector3.Zero;
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float RotationX
        {
            get { return rotation.X; }
            set 
            {
                rotation.X = value; 
                if (rotation.X < 0.0f) rotation.X = 360.0f;
                if (rotation.X > 360.0f) rotation.X = 0.0f;
            }
        }
        public float RotationY
        {
            get { return rotation.Y; }
            set
            {
                rotation.Y = value;
                if (rotation.Y < 0.0f) rotation.Y = 360.0f;
                if (rotation.Y > 360.0f) rotation.Y = 0.0f;
            }
        }
        public float RotationZ
        {
            get { return rotation.Z; }
            set
            {
                rotation.Z = value;
                if (rotation.Z < 0.0f) rotation.Z = 360.0f;
                if (rotation.Z > 360.0f) rotation.Z = 0.0f;
            }
        }

        //Preleva la trasformazione finale corrente (posizione base ancora * posizione relativa ancora * posizione padre)
        public Matrix FinalMatrix
        {
            get 
            {
                //Matrice relativa di rotazione (l'ancora può essere ruotata)
                //Ovviamente non può essere spostata o scalata perchè fissa in un punto
                relativeMatrix = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z));

                if (parentModel != null)
                {
                    return absoluteMatrix * relativeMatrix * parentModel.WorldMatrix * parentModel.ExternalMatrix;
                }
                else
                {
                    return absoluteMatrix * relativeMatrix;
                }
            }
        }

        //Nome dell'ancora
        private string name = "NoAnchorName";
        public string Name
        {
            get { return name; }
        }


        public XModelAnchor(string anchorName)
        {
            try
            {
                absoluteMatrix = Matrix.Identity;
                name = anchorName;

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public XModelAnchor(Matrix refMatrix, string anchorName)
        {
            try
            {
                absoluteMatrix = refMatrix;
                name = anchorName;

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public XModelAnchor(XModel refModel, string anchorToTake)
        {
            try
            {
                parentModel = refModel;
                name = anchorToTake;

                bool exist = false;
                absoluteMatrix = refModel.GetBoneTransform(anchorToTake, true, out exist);
                if (exist == false)
                {
                    absoluteMatrix = Matrix.Identity;
                }

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        ~XModelAnchor()
        {
            //Rilascia le risorse
        }
    }
}
