using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGameLibrary1
{
    public class AudioSound
    {
        //Variabili
        private SoundEffect sound;
        private SoundEffectInstance soundInstance;

        //Stato corrente
        public SoundState State
        {
            get { return soundInstance.State; }
        }

        //Loop
        private bool isLooped;
        public bool IsLooped
        {
            get { return isLooped; }
            set 
            { 
                isLooped = value;
                
                //Termina il souno se non si vuole più il loop
                if ((isLooped == false) && (isCreated) && (soundInstance!=null))
                {
                    soundInstance.Stop(false);
                }
            }
        }

        //Durata
        public TimeSpan Duration
        {
            get { return sound.Duration; }
        }

        //Nome
        public string Name
        {
            get { return sound.Name; }
            set { sound.Name = value; }
        }

        //Creazione
        private bool isCreated = false;
        public bool IsCreated
        {
            get { return isCreated; }
        }

        //Tag
        private Object tag = null;
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public AudioSound(string soundName, ContentManager content)
        {
            try
            {
                sound = content.Load<SoundEffect>(soundName);
                isCreated = true;
            }
            catch
            {
                isCreated = false;
            }
        }

        public bool Play()
        {
            try
            {
                if (isCreated)
                {
                    if (soundInstance == null)
                        soundInstance = sound.CreateInstance();

                    soundInstance.IsLooped = isLooped;

                    if ((soundInstance.State == SoundState.Stopped) || (soundInstance.State == SoundState.Stopped))
                        soundInstance.Play();
                    else
                        soundInstance.Resume();


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Play3D(AudioListener listener, AudioEmitter emitter)
        {
            try
            {
                if (isCreated)
                {
                    if (soundInstance == null)
                        soundInstance = sound.CreateInstance();

                    soundInstance.IsLooped = isLooped;

                    soundInstance.Apply3D(listener, emitter);

                    if ((soundInstance.State == SoundState.Stopped) || (soundInstance.State == SoundState.Stopped))
                        soundInstance.Play();
                    else
                        soundInstance.Resume();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void Pause()
        {
            if (soundInstance != null)
            {
                soundInstance.Pause();
            }
        }

        public void Stop()
        {
            if (soundInstance != null)
            {
                soundInstance.Stop();
            }
        }
        public void Stop(bool immediate)
        {
            if (soundInstance != null)
            {
                soundInstance.Stop(immediate);
            }
        }

        public void Resume()
        {
            if (soundInstance != null)
            {
                soundInstance.Resume();
            }
        }

        public void Dispose()
        {
            try
            {

                if (soundInstance != null)
                {
                    soundInstance.Dispose();
                    soundInstance = null;
                }

                if (sound != null)
                {
                    sound.Dispose();
                    sound = null;
                }
            }
            catch
            { }

            isCreated = false;
        }
    }


    public class AudioSoundImmediate
    {
        //Variabili
        private SoundEffect sound;

        //Creazione
        private bool isCreated = false;
        public bool IsCreated
        {
            get { return isCreated; }
        }

        //Durata
        public TimeSpan Duration
        {
            get { return sound.Duration; }
        }

        //Nome
        public string Name
        {
            get { return sound.Name; }
            set { sound.Name = value; }
        }

        //Tag
        private Object tag = null;
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public AudioSoundImmediate(string soundName, ContentManager content)
        {
            try
            {
                sound = content.Load<SoundEffect>(soundName);
                isCreated = true;
            }
            catch
            {
                isCreated = false;
            }
        }

        public bool Play()
        {
            try
            {
                if (isCreated)
                    return sound.Play();
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Play(float volume, float pitch, float pan)
        {
            try
            {
                if (isCreated)
                    return sound.Play(volume, pitch, pan);
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                if (sound != null)
                {
                    sound.Dispose();
                    sound = null;
                }
            }
            catch
            { }

            isCreated = false;
        }
    }


    public class AudioSong
    {
        //Variabili
        private Song song;

        //Stato corrente
        public MediaState State
        {
            get { return MediaPlayer.State; }
        }

        //Tempo corrente
        public TimeSpan PlayPosition
        {
            get { return MediaPlayer.PlayPosition; }
        }

        //Creazione
        private bool isCreated = false;
        public bool IsCreated
        {
            get { return isCreated; }
        }

        //Tag
        private Object tag = null;
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public AudioSong(string songName, ContentManager content)
        {
            try
            {
                song = content.Load<Song>(songName);
                isCreated = true;
            }
            catch
            {
                isCreated = false;
            }
        }

        public void Play()
        {
            if (song != null)
            {
                MediaPlayer.Play(song);
            }
        }

        public void Play(bool loop)
        {
            if (song != null)
            {
                MediaPlayer.Play(song);

                if (loop)
                    MediaPlayer.MediaStateChanged += new EventHandler(MediaPlayer_MediaStateChanged);
            }
        }

        void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            //Loop
            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (song != null)
                {
                    MediaPlayer.Play(song);
                }
            }
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Dispose()
        {
            try
            {

                if (song != null)
                    song.Dispose();
            }
            catch
            { }

            isCreated = false;
        }
    }
}
