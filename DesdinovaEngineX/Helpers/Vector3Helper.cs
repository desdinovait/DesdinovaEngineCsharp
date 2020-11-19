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
using System.Globalization;

namespace DesdinovaModelPipeline.Helpers
{
	/// <summary>
	/// Vector 3 helper
	/// </summary>
	public class Vector3Helper
	{
        public static Vector3 VectorFromString(string vector)
        {
            try
            {
                Vector3 newVector = Vector3.Zero;
                vector = vector.TrimStart('{').TrimEnd('}');
                string[] splittedVector = vector.Split(' ');
                newVector.X = (float)Convert.ToSingle(splittedVector[0].ToString().TrimStart('X').TrimStart(':'));
                newVector.Y = (float)Convert.ToSingle(splittedVector[1].ToString().TrimStart('Y').TrimStart(':'));
                newVector.Z = (float)Convert.ToSingle(splittedVector[2].ToString().TrimStart('Z').TrimStart(':'));
                return newVector;
            }
            catch
            {
                return Vector3.Zero;
            }
        }

        public static string VectorToString(Vector3 vector)
        {
            try
            {
                return vector.ToString().TrimStart('{').TrimEnd('}');
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool CompareVectors(Vector3 vec1, Vector3 vec2, int decimals)
        {
            if ((Math.Round(vec1.X, decimals) == Math.Round(vec2.X, decimals)) &&
             (Math.Round(vec1.Y, decimals) == Math.Round(vec2.Y, decimals)) &&
             (Math.Round(vec1.Z, decimals) == Math.Round(vec2.Z, decimals)))
            {
                return true;
            }
            return false;
        }



		#region Get angle between vectors
		/// <summary>
		/// Return angle between two vectors. Used for visbility testing and
		/// for checking angles between vectors for the road sign generation.
		/// </summary>
		/// <param name="vec1">Vector 1</param>
		/// <param name="vec2">Vector 2</param>
		/// <returns>Float</returns>
		public static float GetAngleBetweenVectors(Vector3 vec1, Vector3 vec2)
		{
			// See http://en.wikipedia.org/wiki/Vector_(spatial)
			// for help and check out the Dot Product section ^^
			// Both vectors are normalized so we can save deviding through the
			// lengths.
			return (float)Math.Acos(Vector3.Dot(vec1, vec2));
		} // GetAngleBetweenVectors(vec1, vec2)
		#endregion

		#region Distance to line
		/// <summary>
		/// Distance from our point to the line described by linePos1 and linePos2.
		/// </summary>
		/// <param name="point">Point</param>
		/// <param name="linePos1">Line position 1</param>
		/// <param name="linePos2">Line position 2</param>
		/// <returns>Float</returns>
		public static float DistanceToLine(Vector3 point,
			Vector3 linePos1, Vector3 linePos2)
		{
			// For help check out this article:
			// http://mathworld.wolfram.com/Point-LineDistance3-Dimensional.html
			Vector3 lineVec = linePos2-linePos1;
			Vector3 pointVec = linePos1-point;
			return Vector3.Cross(lineVec, pointVec).Length() / lineVec.Length();
		} // DistanceToLine(point, linePos1, linePos2)
		#endregion

		#region Signed distance to plane
		/// <summary>
		/// Signed distance to plane
		/// </summary>
		/// <param name="point">Point</param>
		/// <param name="planePosition">Plane position</param>
		/// <param name="planeNormal">Plane normal</param>
		/// <returns>Float</returns>
		public static float SignedDistanceToPlane(Vector3 point,
			Vector3 planePosition, Vector3 planeNormal)
		{
			Vector3 pointVec = planePosition - point;
			return Vector3.Dot(planeNormal, pointVec);
		} // SignedDistanceToPlane(point, planePosition, planeNormal)
		#endregion

		#region FromArray
		/// <summary>
		/// From array
		/// </summary>
		/// <param name="floatArray">Float array</param>
		/// <returns>Vector 3</returns>
		public static Vector3 FromArray(float[] floatArray)
		{
			return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
		} // FromArray(floatArray)
		#endregion
	} // class Vector3Helper
} // namespace DungeonQuest.Helpers
