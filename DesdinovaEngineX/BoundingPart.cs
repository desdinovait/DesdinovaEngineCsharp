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
    public class BoundingPart
    {
        private Color _colour;
        public Color Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                _colour = value;
            }
        }

        private List<BoundingSphere> _boundingSpheres;
        public List<BoundingSphere> BoundingSpheres
        {
            get
            {
                return _boundingSpheres;
            }
            set
            {
                _boundingSpheres = value;
            }
        }

        private List<BoundingSphere> _boundingSpheresTransformed;
        public List<BoundingSphere> BoundingSpheresTransformed
        {
            get
            {
                return _boundingSpheresTransformed;
            }
            set
            {
                _boundingSpheresTransformed = value;
            }
        }


        private Matrix _boneTransform;
        private Matrix BoneTransform
        {
            get
            {
                return _boneTransform;
            }
            set
            {
                _boneTransform = value;
            }
        }

        public BoundingPart(BoundingSphere[] boundingSpheres, Matrix boneTransform, string name, Color colour)
        {
            _colour = colour;
            _boneTransform = boneTransform;
            _boundingSpheres = new List<BoundingSphere>();
            _boundingSpheresTransformed = new List<BoundingSphere>();

            _boundingSpheres.InsertRange(0, boundingSpheres);
            _boundingSpheresTransformed.InsertRange(0, boundingSpheres);
        }

        public BoundingPart(BoundingSphere boundingSphere, Matrix boneTransform, string name, Color colour)
        {
            _colour = colour;
            _boneTransform = boneTransform;
            _boundingSpheres = new List<BoundingSphere>();
            _boundingSpheresTransformed = new List<BoundingSphere>();

            _boundingSpheres.Add(boundingSphere);
            _boundingSpheresTransformed.Add(new BoundingSphere(boundingSphere.Center, boundingSphere.Radius));
        }

        /// <summary>
        /// Returns True if any of this BoundingPart's BoundingSpheres collide with the given BoundingPart's.
        /// </summary>
        /// <param name="boundingPart"></param>
        /// <returns></returns>
        public bool Intersects(BoundingPart boundingPart)
        {
            foreach (BoundingSphere thisBS in _boundingSpheresTransformed)
            {
                foreach (BoundingSphere otherBS in boundingPart.BoundingSpheresTransformed)
                {
                    if (thisBS.Intersects(otherBS))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns True if any of this BoundingPart's BoundingSpheres collide with the given BoundingSphere.
        /// </summary>
        /// <param name="boundingPart"></param>
        /// <returns></returns>
        public bool Intersects(BoundingSphere boundingSphere)
        {
            foreach (BoundingSphere thisBS in _boundingSpheresTransformed)
            {
                if (thisBS.Intersects(boundingSphere))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Transform each of the part's BoundingSpheres into it's World Space position, scale & orientation. 
        /// The resulting transformed BoundingSphere's can then be used to test for Collisions.
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="rotationMatrix"></param>
        /// <param name="positionMatrix"></param>
        public void Transform(float scale, Matrix rotationMatrix, Matrix positionMatrix)
        {
            int bsIndex = 0;
            foreach (BoundingSphere bs in _boundingSpheres)
            {
                Matrix scaleMatrix = Matrix.CreateScale(new Vector3(scale));

                Matrix localWorld = _boneTransform * rotationMatrix * scaleMatrix * positionMatrix;
                //Matrix localWorld = rotationMatrix * scaleMatrix * positionMatrix;

                BoundingSphere bsTransformed = _boundingSpheresTransformed[bsIndex];
                bsTransformed.Center = Vector3.Transform(bs.Center, localWorld);

                //BoneTransforms can include some scaling, so scaling the radius by our defined scale may not be sufficient.
                //Big thanks to Shawn Hargreaves (http://blogs.msdn.com/shawnhar/default.aspx) for the following solution using localWorld.Forward.Length() instead.
                bsTransformed.Radius = bs.Radius * localWorld.Forward.Length();

                _boundingSpheresTransformed[bsIndex] = bsTransformed;

                bsIndex++;
            }
        }
    }

}
