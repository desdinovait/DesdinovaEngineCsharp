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
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace SorsAdversa
{
    public class Surface
    {
        //Elementi
        private List<Section> sections;
        public List<Section> Sections
        {
            get { return sections; }
        }
	
        private List<XModel> models;
        private ContentManager contentMan;
        private Camera cam;

        //Sezioni totali
        private int totalSections = 0;
        public int TotalSections
        {
            get { return totalSections; }
        }
	
        //Sezioni visibili
        private int visibleSections = 0;
        public int VisibleSections
        {
            get { return visibleSections; }
        }

	    //Stelle
        private bool useStarField = false;
        private Lines3D stars = null;



        public Surface(bool generateStarfield, int starfieldConsistency, ContentManager contentManager)
        {
            //Elemnti
            contentMan = contentManager;
            models = new List<XModel>();
            sections = new List<Section>();

            //Array di stelle
            useStarField = generateStarfield;
            stars = new Lines3D(starfieldConsistency);
            for (int i = 0; i < starfieldConsistency; i++)
            {
                Vector3 startPosition = Vector3.Zero;
                startPosition.X = RandomHelper.GetRandomFloat(-100, 100);
                startPosition.Y = RandomHelper.GetRandomFloat(-100, 100);
                startPosition.Z = RandomHelper.GetRandomFloat(-100, 100);

                Vector3 endPosition = Vector3.Zero;
                endPosition.X = startPosition.X + RandomHelper.GetRandomFloat(0.05f, 0.25f);
                endPosition.Y = startPosition.Y;
                endPosition.Z = startPosition.Z;

                stars.AddLine(new Line3D(startPosition, endPosition, Color.White, Color.WhiteSmoke));
            }
        }

        public void AddSection(Section newSection)
        {
            //Aggiunge una sezione
            sections.Add(newSection);

            //Crea i modelli
            for (int i = 0; i < newSection.Repetitions; i++)
            {
                XModel newModel = new XModel(this);
                newModel.Initialize(newSection.ElementName, contentMan);
                newModel.PositionY = newSection.PositionY;
                newModel.PositionZ = newSection.PositionZ;
                models.Add(newModel);
                totalSections = totalSections + 1;
            }
        }


        public void Update(GameTime gameTime, Camera camera)
        {
            cam = camera;
            visibleSections = 0;
            for (int i = 0; i < totalSections; i++)
            {
                //Nuova posizione della sezione corrente
                if (i > 0)
                {
                    models[i].PositionX = models[i - 1].PositionX + (models[i - 1].Dimension.Width / 2.0f) + (models[i].Dimension.Width / 2.0f);
                }
                models[i].Update(gameTime, camera);

                //calcolo delle sezioni visibili
                if (models[i].InFrustrum)
                {
                    visibleSections = visibleSections + 1;
                }
            }

            //Stelle
            if (useStarField)
            {
                //Se il gioco è lento allora non disegna le stelle x ottimizzare
                if (gameTime.IsRunningSlowly)
                {
                    stars.ToDraw = false;
                }
                else
                {
                    stars.ToDraw = true;
                }
                stars.Update(gameTime, camera);
            }
        }

        public void Draw(LightEffect lightEffect)
        {
            //Disegna
            for (int i = 0; i < totalSections; i++)
            {
                models[i].Draw(lightEffect);
            }

            //Stelle
            if (useStarField)
            {
                for (int i = 0; i < 10; i++)    //10 ?!?!?
                {
                    stars.WorldMatrix = Matrix.CreateTranslation(200 * i, 0, 0);
                    stars.Update(null, cam);
                    stars.Draw(lightEffect);
                }
            }
        }


    }
}
