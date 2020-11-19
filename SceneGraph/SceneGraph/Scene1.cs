using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SceneGraph
{
    public class Scene1 : Scene
    {
        //Oggetti
        Camera camera1;
        Camera camera2;
        LightEffect lightEffect;
        Ship player1;
        Ship player2;
        Panel info;
        Movie film;
        Sentinel sentinel;

        public Scene1(string name, GraphicsDevice device, GameServiceContainer service):base(name, device, service)
        {
            //Oggetti
            camera1 = new Camera(this);
            camera2 = new Camera(this);
            lightEffect = new LightEffect(this);
            player1 = new Ship(this);
            player2 = new Ship(this);
            info = new Panel(this);
            film = new Movie(this);
            sentinel = new Sentinel(this);

            //Inziializzazione scena
            base.Camera = camera1;
            base.LightEffect = lightEffect;

            //Aggiunge gli oggetti alla scena
            base.AddObject(player1);
            base.AddObject(player2);
            base.AddObject(info);
            base.AddObject(film);
            base.AddObject(sentinel);

            //Altro
            //film.Play();
            sentinel.Activated += new EngineTriggerEventHandler(sentinel_Activated);
            sentinel.DeActivated += new EngineTriggerEventHandler(sentinel_DeActivated);
            sentinel.IsActive = true;
        }

        public override void Update(GameTime gametime)
        {
            player1.Position = new Vector3(player1.Position.X, player1.Position.Y, player1.Position.Z + 1);
            base.Update(gametime);
        }
 
        public override void Draw()
        {
            base.Draw();
        }

        void sentinel_DeActivated(object source, bool active)
        {
            int act = 0;
        }

        void sentinel_Activated(object source, bool active)
        {
            int act = 1;
        }
    }
}
