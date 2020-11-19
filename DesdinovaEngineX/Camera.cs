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
    public class Camera : EngineObject
    {
        //Posizione
        private Vector3 positionVector = new Vector3(50, 50, 50);
        public Vector3 Position
        {
            get { return positionVector; }
            set { positionVector = value; }
        }
        public float PositionX
        {
            get { return positionVector.X; }
            set { positionVector.X = value; }
        }
        public float PositionY
        {
            get { return positionVector.Y; }
            set { positionVector.Y = value; }
        }
        public float PositionZ
        {
            get { return positionVector.Z; }
            set { positionVector.Z = value; }
        }

        //Target (punto osservato)
        private Vector3 targetVector = new Vector3(0, 0, 0);
        public Vector3 Target
        {
            get { return targetVector; }
            set { targetVector = value; }
        }
        public float TargetX
        {
            get { return targetVector.X; }
            set { targetVector.X = value; }
        }
        public float TargetY
        {
            get { return targetVector.Y; }
            set { targetVector.Y = value; }
        }
        public float TargetZ
        {
            get { return targetVector.Z; }
            set { targetVector.Z = value; }
        }

        //UpVector
        private Vector3 upVector = new Vector3(0, 1, 0);
        public Vector3 Up
        {
            get { return upVector; }
            set { upVector = value; }
        }
        public float UpX
        {
            get { return upVector.X; }
            set { upVector.X = value; }
        }
        public float UpY
        {
            get { return upVector.Y; }
            set { upVector.Y = value; }
        }
        public float UpZ
        {
            get { return upVector.Z; }
            set { upVector.Z = value; }
        }

        //Rotazione
        private Vector3 rotationVector = new Vector3(0, 0, 0);
        public Vector3 Rotation
        {
            get { return rotationVector; }
            set { rotationVector = value; }
        }
        public float RotationX
        {
            get { return rotationVector.X; }
            set { rotationVector.X = value; }
        }
        public float RotationY
        {
            get { return rotationVector.Y; }
            set { rotationVector.Y = value; }
        }
        public float RotationZ
        {
            get { return rotationVector.Z; }
            set { rotationVector.Z = value; }
        }

        //Bounding del frustrum attuale
        private BoundingFrustum frustrum = null;
        public BoundingFrustum Frustrum
        {
            get { return frustrum; }
        }
    	
        //Matrice di vista
        private Matrix viewMatrix = Matrix.Identity;
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        //Matrice di proiezione
        private Matrix projectionMatrix = Matrix.Identity;
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        //Punto più vicino
        private float nearDistance = 0.1f;
        public float NearDistance
	    {
		    get { return nearDistance;}
		    set { nearDistance = value;}
	    }

        //Punto più distante
        private float farDistance = 1000.0f;
        public float FarDistance
	    {
		    get { return farDistance;}
		    set { farDistance = value;}
	    }

        //Ratio
        private float aspectRatio = MathHelper.PiOver4;
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }

        //Rapporto
        private float fieldView = 800.0f / 600.0f;
        public float FieldOfView
        {
            get { return fieldView; }
            set { fieldView = value; }
        }

        //Shake
        Vector3 shakeVector = Vector3.Zero;
        int shakeMilliseconds = 5000;
        int shakeCameraOffsetX = 2;
        int shakeCameraOffsetY = 2;
        double shakeTime = 0.0f;
        bool shakeEnabled = false;

        //Evento fine shake
        public event EventHandler OnShakeFinish;

        public Camera()
        {
            try
            {
                //Frustrum
                frustrum = new BoundingFrustum(Matrix.Identity);

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                if (shakeEnabled)
                {
                    shakeTime = shakeTime + gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shakeTime < shakeMilliseconds)
                    {
                        Random rnd = new Random();
                        shakeVector.X = (float)rnd.NextDouble() * shakeCameraOffsetX;
                        shakeVector.Y = (float)rnd.NextDouble() * shakeCameraOffsetY;
                        shakeVector.Z = 0.0f;
                    }
                    else
                    {
                        shakeVector = Vector3.Zero;
                        shakeEnabled = false;

                        //Esegue l'evento di fine shake
                        if (OnShakeFinish != null)
                        {
                            OnShakeFinish(this, null);
                        }
                    }
                }

                //Matrici di vista
                viewMatrix = Matrix.CreateLookAt(positionVector, targetVector + shakeVector, upVector);
                viewMatrix = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotationVector.Y), MathHelper.ToRadians(rotationVector.X), MathHelper.ToRadians(rotationVector.Z)) * viewMatrix;
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldView, aspectRatio, nearDistance, farDistance);
                frustrum = new BoundingFrustum(viewMatrix * projectionMatrix);

                //Inner frustrum
                //L'inner frustrum è un frustrum più piccolo interno a lfrustrum principale
                //Utilizzato solitamente per creare un'area utile in cui confinare i movimenti usando:
                //if ((camera.InnerFrustrum.Contains(base.BoundingSphereTransformed) != ContainmentType.Intersects))          
                //
                //Matrix innerProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldView / (1.0f + innerFrustrumValue), aspectRatio, nearDistance, farDistance);
                //innerFrustrum = new BoundingFrustum(viewMatrix * innerProjectionMatrix);

                //Vector3 fwd = targetVector - positionVector;
                //fwd.Normalize();
            }
        }

        public void Shake(int mseconds, int offsetX, int offsetY)
        {
            if (IsCreated)
            {
                shakeMilliseconds = mseconds;
                shakeCameraOffsetX = offsetX;
                shakeCameraOffsetY = offsetY;
                shakeTime = 0.0f;
                shakeEnabled = true;
            }
        }

        ~Camera()
        {
            //Rilascia le risorse
        }
    }
}
