using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DesdinovaModelPipeline
{
    public class AudioSound : EngineSceneObject
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
                if ((isLooped == false) && (IsCreated) && (soundInstance != null))
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

        public AudioSound(string soundName, Scene parentScene):base(parentScene)
        {
            try
            {
                sound = this.ParentScene.SceneContent.Load<SoundEffect>(soundName);
                IsCreated = true;
            }
            catch
            {
                IsCreated = false;
            }
        }

        public bool Play()
        {
            try
            {
                if (IsCreated)
                {
                    if (soundInstance == null)
                        soundInstance = sound.CreateInstance();

                    soundInstance.IsLooped = isLooped;

                    if ((soundInstance.State == SoundState.Stopped) || (soundInstance.State == SoundState.Paused))
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
                if (IsCreated)
                {
                    if (soundInstance == null)
                        soundInstance = sound.CreateInstance();

                    soundInstance.IsLooped = isLooped;

                    soundInstance.Apply3D(listener, emitter);

                    if ((soundInstance.State == SoundState.Stopped) || (soundInstance.State == SoundState.Paused))
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

            IsCreated = false;
        }
    }


    public class AudioSoundImmediate : EngineSceneObject
    {
        //Variabili
        private SoundEffect sound;

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
    

        public AudioSoundImmediate(string soundName, Scene parentScene):base(parentScene)
        {
            try
            {
                sound = this.ParentScene.SceneContent.Load<SoundEffect>(soundName);
                IsCreated = true;
            }
            catch
            {
                IsCreated = false;
            }
        }

        public bool Play()
        {
            try
            {
                if (IsCreated)
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
                if (IsCreated)
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

            IsCreated = false;
        }
    }


    public class AudioSong : EngineSceneObject
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

        public AudioSong(string songName,Scene parentScene):base(parentScene)
        {
            try
            {
                song = this.ParentScene.SceneContent.Load<Song>(songName);
                IsCreated = true;
            }
            catch
            {
                IsCreated = false;
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

            IsCreated = false;
        }
    }
}
