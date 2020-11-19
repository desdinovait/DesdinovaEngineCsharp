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
    /*Init curve
    float time = 0;
    cameraCurvePosition.AddPoint(new Vector3(27.5f, 150, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(3f, 50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(12f, 50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(3f, 50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(12f, -50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(47.5f, 50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(-6, 50, 0), time);
    time += 3000;
    cameraCurvePosition.AddPoint(new Vector3(-27, 150, 0), time);
    time += 3000;
    cameraCurvePosition.AddPoint(new Vector3(-6, 250, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(17.5f, 50, 0), time);
    time += 2000;
    cameraCurvePosition.AddPoint(new Vector3(21, 50, 0), time);
    time += 3000;
    cameraCurvePosition.AddPoint(new Vector3(142, 50, 0), time);
    time += 3000;
    cameraCurvePosition.AddPoint(new Vector3(21, 50, 0), time);

    cameraCurvePosition.SetTangents();

    //Set curve
    pathValue += (float)framerate.ElapsedTime;
    Vector3 prova = cameraCurvePosition.GetPointOnCurve(pathValue);*/


    public class Curve3D : EngineSceneObject
    {
        //Curve XYZ
        public Curve curveX = null;
        public Curve curveY = null;
        public Curve curveZ = null;

        //Tempo massimo
        private float maxTime = 0.0f;
        public float MaxTime
        {
            get { return maxTime; }
        }

        //Tempo attuale
        private float currentTime = 0.0f;
        public float CurrentTime
        {
            get { return currentTime; }
        }

        public Curve3D(Scene parentScene):base(parentScene)
        {
            try
            {
                curveX = new Curve();
                curveY = new Curve();
                curveZ = new Curve();

                curveX.PostLoop = CurveLoopType.Oscillate;
                curveY.PostLoop = CurveLoopType.Oscillate;
                curveZ.PostLoop = CurveLoopType.Oscillate;

                curveX.PreLoop = CurveLoopType.Oscillate;
                curveY.PreLoop = CurveLoopType.Oscillate;
                curveZ.PreLoop = CurveLoopType.Oscillate;

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public void SetTangents()
        {
            if (IsCreated)
            {
                CurveKey prev;
                CurveKey current;
                CurveKey next;
                int prevIndex;
                int nextIndex;
                for (int i = 0; i < curveX.Keys.Count; i++)
                {
                    prevIndex = i - 1;
                    if (prevIndex < 0) prevIndex = i;

                    nextIndex = i + 1;
                    if (nextIndex == curveX.Keys.Count) nextIndex = i;

                    prev = curveX.Keys[prevIndex];
                    next = curveX.Keys[nextIndex];
                    current = curveX.Keys[i];
                    SetCurveKeyTangent(ref prev, ref current, ref next);
                    curveX.Keys[i] = current;
                    prev = curveY.Keys[prevIndex];
                    next = curveY.Keys[nextIndex];
                    current = curveY.Keys[i];
                    SetCurveKeyTangent(ref prev, ref current, ref next);
                    curveY.Keys[i] = current;

                    prev = curveZ.Keys[prevIndex];
                    next = curveZ.Keys[nextIndex];
                    current = curveZ.Keys[i];
                    SetCurveKeyTangent(ref prev, ref current, ref next);
                    curveZ.Keys[i] = current;
                }
            }
        }

        static void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur, ref CurveKey next)
        {
            float dt = next.Position - prev.Position;
            float dv = next.Value - prev.Value;
            if (Math.Abs(dv) < float.Epsilon)
            {
                cur.TangentIn = 0;
                cur.TangentOut = 0;
            }
            else
            {
                // The in and out tangents should be equal to the slope between the adjacent keys.
                cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
                cur.TangentOut = dv * (next.Position - cur.Position) / dt;
            }
        }

        public void AddPoint(Vector3 point, float time)
        {
            if (IsCreated)
            {
                curveX.Keys.Add(new CurveKey(time, point.X));
                curveY.Keys.Add(new CurveKey(time, point.Y));
                curveZ.Keys.Add(new CurveKey(time, point.Z));
                if (time > maxTime)
                {
                    maxTime = time;
                }
            }
        }

        public Vector3 GetPointOnCurve(float time)
        {
            if (IsCreated)
            {
                Vector3 point = new Vector3();
                point.X = curveX.Evaluate(time);
                point.Y = curveY.Evaluate(time);
                point.Z = curveZ.Evaluate(time);
                currentTime = time;
                return point;
            }
            else
            {
                return Vector3.Zero;
            }
        }


        public Vector3 GetPointOnCurveByIncrement(float increment)
        {
            if (IsCreated)
            {
                currentTime = currentTime + increment;
                Vector3 point = new Vector3();
                point.X = curveX.Evaluate(currentTime);
                point.Y = curveY.Evaluate(currentTime);
                point.Z = curveZ.Evaluate(currentTime);
                return point;
            }
            else
            {
                return Vector3.Zero;
            }
        }

        ~Curve3D()
        {
            //Rilascia le risorse
        }

    }
}
