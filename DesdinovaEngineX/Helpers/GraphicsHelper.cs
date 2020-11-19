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
//Using Desdiniova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline.Helpers
{
    public static class GraphicsHelper
    {


        /// <summary>
        /// Checks whether an object is in the field of view of another
        /// </summary>
        /// <param name="posFirst">Source object 3d's position</param>
        /// <param name="facingFirst">Source object's facing direction vector</param>
        /// <param name="posSecond">Target object 3d's position</param>
        /// <param name="fov">Field of view angle of source object in radians</param>
        /// <returns>True/false whether or not the second object is in the FOV of the first</returns>
        public static bool isSecondInFOVofFirst(Vector3 posFirst, Vector3 facingFirst, Vector3 posSecond, double fov)
        {
            // tempVect will be the difference in position between the source and target 
            Vector3 tempVect = posSecond - posFirst;
            tempVect.Normalize();
            // Returns if the target is in the FOV of the source, here the FOV is the 'fov'
            // parameter. 
            return Vector3.Dot(facingFirst, tempVect) >= Math.Cos(fov);
        }


        public static void RemapModel(Model model, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }


        public static Texture2D CreateColorTexture(GraphicsDevice device, int width, int height, int levels, Color color, TextureUsage usage, SurfaceFormat format)
        {
            Texture2D colorTexture = new Texture2D(device, width, height, levels, usage, format);
            int dim = width * height;
            Color[] colorTextureData = new Color[dim];
            for (int i = 0; i < dim; i++)
            {
                colorTextureData[i] = color;
            }
            colorTexture.SetData<Color>(colorTextureData);
            return colorTexture;
        }


        public static Matrix CreateShadowMatrix(Vector3 lightDirection, Plane plane, Vector3 translation)
        {
            return Matrix.CreateShadow(lightDirection, plane) * Matrix.CreateTranslation(translation);
        }


        public static bool PointInSphere(Vector3 point, Vector3 center, float centerRadius)
        {
            Vector3 resultVector = center - point;
            return resultVector.LengthSquared() < (centerRadius * centerRadius);
        }


        public static bool SphereInSphere(Vector3 point, float pointRadius, Vector3 center, float centerRadius)
        {
            Vector3 resultVector = center - point;
            float centDist = centerRadius + pointRadius;
            return resultVector.LengthSquared() < (centDist*centDist);
        }


        public static Vector3 DirectionVector(Vector3 posVector, float angle)
        {
            Vector3 v = (new Vector3(posVector.X + (float)Math.Cos(MathHelper.ToRadians(angle)), posVector.Y + (float)Math.Sin(MathHelper.ToRadians(angle)), posVector.Z)) - posVector;
            if (v.LengthSquared() != 0)
            v.Normalize();
            return v;
        }


        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }


        public static bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA, Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }



        public static RenderTarget2D CreateRenderTarget(GraphicsDevice device, int numberLevels, SurfaceFormat surface)
        {
            if (!GraphicsAdapter.DefaultAdapter.CheckDeviceFormat(DeviceType.Hardware,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format, TextureUsage.None,
                QueryUsages.None, ResourceType.RenderTarget, surface))
            {
                // Fall back to current display format
                surface = device.DisplayMode.Format;
            }
            return new RenderTarget2D(device,
                device.PresentationParameters.BackBufferWidth,
                device.PresentationParameters.BackBufferHeight,
                1, surface,
                device.PresentationParameters.MultiSampleType,
                device.PresentationParameters.MultiSampleQuality);
        }


        public static DepthStencilBuffer CreateDepthStencil(RenderTarget2D target)
        {
            return new DepthStencilBuffer(target.GraphicsDevice, target.Width,
                target.Height, target.GraphicsDevice.DepthStencilBuffer.Format,
                target.MultiSampleType, target.MultiSampleQuality);
        }
        public static DepthStencilBuffer CreateDepthStencil(RenderTarget2D target, DepthFormat depth)
        {
            if (GraphicsAdapter.DefaultAdapter.CheckDepthStencilMatch(DeviceType.Hardware,
               GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format, target.Format,
                depth))
            {
                return new DepthStencilBuffer(target.GraphicsDevice, target.Width,
                    target.Height, depth, target.MultiSampleType, target.MultiSampleQuality);
            }
            else
                return CreateDepthStencil(target);
        }

    }
}
