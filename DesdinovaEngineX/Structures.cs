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
//DurectX
using Microsoft.DirectX.DirectInput;
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline
{
    /// <summary>
    /// Dimensioni (larghezza, altezza, profondità)
    /// </summary>
    public struct Dimensions
    {
        //Larghezza
        private float width;
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        //Altezza
        private float height;
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        //Profondità
        private float depth;
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        //Valore default (0)
        public static Dimensions Zero
        {
            get
            {
                return new Dimensions(0, 0, 0);
            }
        }

        //Valore default (1)
        public static Dimensions One
        {
            get
            {
                return new Dimensions(1, 1, 1);
            }
        }

        public Dimensions(float newWidth, float newHeight, float newDepth)
        {
            width = newWidth;
            height = newHeight;
            depth = newDepth;
        }
    }






    /// <summary>
    /// Velocità (con vettore direzione)
    /// </summary>
    public struct Velocity
    {
        //Somma
        public static Vector3 operator +(Vector3 op1, Velocity op2)
        {
            return op1 + op2.VelocityVector;
        }
        public static Vector3 operator +(Velocity op1, Vector3 op2)
        {
            return op1.VelocityVector + op2;
        }

        //Sottrazione
        public static Vector3 operator -(Vector3 op1, Velocity op2)
        {
            return op1 - op2.VelocityVector;
        }
        public static Vector3 operator -(Velocity op1, Vector3 op2)
        {
            return op1.VelocityVector - op2;
        }

        //Moltiplicazione
        public static Vector3 operator *(Vector3 op1, Velocity op2)
        {
            return new Vector3(op1.X * op2.VelocityVector.X, op1.Y * op2.VelocityVector.Y, op1.Z * op2.VelocityVector.Z);
        }
        public static Vector3 operator *(Velocity op1, Vector3 op2)
        {
            return new Vector3(op1.VelocityVector.X * op2.X, op1.VelocityVector.Y * op2.Y, op1.VelocityVector.Z * op2.Z);
        }
        public static Vector3 operator *(Velocity op1, float op2)
        {
            return Vector3.Multiply(op1.VelocityVector, op2);
        }
        public static Vector3 operator *(float op1, Velocity op2)
        {
            return Vector3.Multiply(op2.VelocityVector, op1);
        }

        //Divisione
        public static Vector3 operator /(Vector3 op1, Velocity op2)
        {
            return new Vector3(op1.X / op2.VelocityVector.X, op1.Y / op2.VelocityVector.Y, op1.Z / op2.VelocityVector.Z);
        }
        public static Vector3 operator /(Velocity op1, Vector3 op2)
        {
            return new Vector3(op1.VelocityVector.X / op2.X, op1.VelocityVector.Y / op2.Y, op1.VelocityVector.Z / op2.Z);
        }

        //Vettore direzione
        private Vector3 direction;
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        //Vettore direzione (componente X)
        public float DirectionX
        {
            get { return direction.X; }
            set { direction.X = value; }
        }

        //Vettore direzione (componente Y)
        public float DirectionY
        {
            get { return direction.Y; }
            set { direction.Y = value; }
        }

        //Vettore direzione (componente Z)
        public float DirectionZ
        {
            get { return direction.Z; }
            set { direction.Z = value; }
        }

        //Valore di velocità
        private float amount;
        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        //Tempo
        private double seconds;
        public double Seconds
        {
            get { return seconds; }
            set { seconds = value; }
        }

        //Vettore velocità
        public Vector3 VelocityVector
        {
            get
            {
                Vector3 ret = Vector3.Zero;
                ret.X = direction.X * amount * (float)seconds;
                ret.Y = direction.Y * amount * (float)seconds;
                ret.Z = direction.Z * amount * (float)seconds;
                return ret;
            }
        }


        public Velocity(Vector3 initialDirection, float initialAmount)
        {
            direction = initialDirection;
            amount = initialAmount;
            seconds = 0;
        }
    }




    /// <summary>
    /// Luce direzionale
    /// </summary>
    public struct DirectionalLightParameters
    {
        private Color diffuseColor;
        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        private Color specularColor;
        public Color SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }

        private Vector3 direction;
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public DirectionalLightParameters(bool lightEnable, Vector3 lightDirection, Color lightDiffuseColor, Color lightSpecularColor)
        {
            isEnabled = lightEnable;
            diffuseColor = lightDiffuseColor;
            specularColor = lightSpecularColor;
            direction = lightDirection;
            direction.Normalize();
        }
    }


    /// <summary>
    /// Luce point
    /// </summary>
    public struct PointLightParameters
    {
        private Color diffuseColor;
        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        private Color specularColor;
        public Color SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float PositionZ
        {
            get { return position.Z; }
            set { position.Z = value; }
        }

        private float radius;
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
       
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        internal bool inFrustrum;

        internal BoundingSphere boundingSphere;

        public PointLightParameters(bool lightEnable, Vector3 lightPosition, float lightRadius, Color lightDiffuseColor, Color lightSpecularColor)
        {
            isEnabled = lightEnable;
            diffuseColor = lightDiffuseColor;
            specularColor = lightSpecularColor;
            position = lightPosition;
            radius = lightRadius;
            inFrustrum = true;
            boundingSphere = new BoundingSphere(lightPosition, lightRadius);
        }
    }

    /// <summary>
    /// Nebbia
    /// </summary>
    public struct FogParameters
    {
        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private float start;
        public float Start
        {
            get { return start; }
            set { start = value; }
        }

        private float end;
        public float End
        {
            get { return end; }
            set { end = value; }
        }

        private float altitude;
        public float Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }

        //Per una nebbia "normale" (quindi non in base anche all'altezza) impostare questo valore a 1
        private float thinning;
        public float Thinning
        {
            get { return thinning; }
            set { thinning = value; }
        }

        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        public FogParameters(bool fogEnable, float fogStart, float fogEnd, float fogAltitude, float fogThinning, Color fogColor)
        {
            enable = fogEnable;
            color = fogColor;
            start = fogStart;
            altitude = fogAltitude;
            thinning = fogThinning; 
            end = fogEnd;
        }
    }



    public struct CountInfo
    {
        //Vertici visibili renderizzati
        private int currentVerticies;
        public int CurrentVerticies
        {
            get { return currentVerticies; }
            internal set { currentVerticies = value; }
        }

        //Primitive visibili renderizzati
        private int currentPrimitives;
        public int CurrentPrimitives
        {
            get { return currentPrimitives; }
            internal set { currentPrimitives = value; }
        }

        //Mesh visibili renderizzate
        private int currentMeshes;
        public int CurrentMeshes
        {
            get { return currentMeshes; }
            internal set { currentMeshes = value; }
        }

        //Modelli visibili
        private int currentModels;
        public int CurrentModels
        {
            get { return currentModels; }
            internal set { currentModels = value; }
        }



        //Vertici totali
        private int loadedVerticies;
        public int LoadedVerticies
        {
            get { return loadedVerticies; }
            internal set { loadedVerticies = value; }
        }

        //Primitive totali
        private int loadedPrimitives;
        public int LoadedPrimitives
        {
            get { return loadedPrimitives; }
            internal set { loadedPrimitives = value; }
        }

        //Mesh caricate
        private int loadedMeshes;
        public int LoadedMeshes
        {
            get { return loadedMeshes; }
            internal set { loadedMeshes = value; }
        }

        //Modelli caricati
        private int loadedModels;
        public int LoadedModels
        {
            get { return loadedModels; }
            internal set { loadedModels = value; }
        }
    }

}
