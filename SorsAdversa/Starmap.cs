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
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;

namespace SorsAdversa
{
    public class Starmap
    {
        Dictionary<string, Quad3D> elements;
        Lines3D elementsList;
        Font elementFontname;
        Camera mainCamera;

        public Starmap(int capacity, ContentManager contentManager)
        {
            elements = new Dictionary<string, Quad3D>(capacity);
            elementsList = new Lines3D(capacity);
            elementsList.WorldMatrix = Matrix.Identity;
            elementFontname = new Font("Content\\Font\\Courier", contentManager);
            elementFontname.Scale = 0.75f;
        }

        public void Add(string fileName, string name, Vector3 position, float scale, Color color, ContentManager contentManager)
        {
            Quad3D newQuad = new Quad3D(fileName, Quad3D.Quad3DGeneration.Center, contentManager);
            newQuad.Position = position;
            newQuad.Scale = new Vector2(scale);
            newQuad.IsBillboard = true;
            newQuad.Color = color;
            newQuad.Tag = name;
            elements.Add(name, newQuad);
        }

        public void Connect(string elementA, string elementB, Color colorA, Color colorB)
        {
            Quad3D quadA;
            Quad3D quadB;
            elements.TryGetValue(elementA, out quadA);
            elements.TryGetValue(elementB, out quadB);
            elementsList.AddLine(new Line3D(quadA.Position, quadB.Position, colorA, colorB));
        }

        public void Update(GameTime gameTime, Camera camera)
        {
            Dictionary<string, Quad3D>.ValueCollection keyColl = elements.Values;
            foreach (Quad3D quad in keyColl)
            {
                quad.Update(gameTime, camera);
            }

            elementsList.Update(gameTime, camera);

            mainCamera = camera;
            elementFontname.Update(gameTime);
        }

        public void Draw(LightEffect lightEffect)
        {
            elementsList.Draw(lightEffect);

            Dictionary<string, Quad3D>.ValueCollection valueColl = elements.Values;
            foreach (Quad3D quad in valueColl)
            {
                elementFontname.Text = quad.Tag;
                elementFontname.Position = new Vector2(quad.Position2D.X - elementFontname.Width / 2.0f, quad.Position2D.Y - elementFontname.Height / 2.0f);
                elementFontname.Draw();
                quad.Draw(lightEffect);
            }

        }
    }


    //starmap = new Starmap(50);
    //starmap.Add("Content\\Texture\\Flare04", "Sun", new Vector3(10, 10, 10), 15, base.SceneContent);
    //starmap.Add("Content\\Texture\\Flare04", "Terra", new Vector3(30, 30, 0), 15, base.SceneContent);
    //starmap.Add("Content\\Texture\\Flare04", "Giove", new Vector3(-30, -30, -30), 15, base.SceneContent);
    //starmap.Connect("Sun", "Terra", Color.Blue, Color.BlueViolet);
    //starmap.Connect("Sun", "Giove", Color.Red, Color.Crimson);
}
