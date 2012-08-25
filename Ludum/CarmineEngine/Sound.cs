using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace CarmineEngine
{
    public class Sound
    {
        public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string,SoundEffect>();

        bool loops = false;
        public bool Loops{ get { return loops; } set { loops = value; cue.IsLooped = true; }}
        float pitch = 0;
        public float Pitch 
        { 
            get { return pitch; }
            set
            {
                if (value >= 0 && value <= 1)
                    pitch = value;
                else if(value < 0)
                    pitch = 0;
                else
                    pitch = 1;
            }
        }
        float volume = 1;
        public float Volume
        {
            get { return volume; }
            set
            {
                if (value >= 0 && value <= 1)
                    volume = value;
                else if (value < 0)
                    volume = 0;
                else
                    volume = 1;
            }
        }
        float pan = 0;
        public float Pan
        {
            get { return pan; }
            set
            {
                if (value >= -1 && value <= 1)
                    pitch = value;
                else if (value < -1)
                    pitch = -1;
                else
                    pitch = 1;
            }
        }
        bool paused = false;
        public bool Paused { get { return paused; } }


        string effect;
        string Effect
        {
            get { return effect; }
            set 
            { 
                effect = value;
                if (!Sounds.ContainsKey(effect))
                    AudioDevice.LoadSound(effect);
            }
        }
        SoundEffectInstance cue;

        public Sound(string sound)
        {
            Effect = sound;
            cue = Sounds[effect].CreateInstance();
        }

        public void play()
        {
            reset();
            cue.Play();
            paused = false;
        }

        public void pause()
        {
            paused = true;
            cue.Pause();
        }

        public void resume()
        {
            paused = false;
            cue.Resume();
        }

        void reset() {
            cue.Dispose();
            cue = Sounds[effect].CreateInstance();
            cue.IsLooped = Loops;
            cue.Pitch = Pitch;
            cue.Pan = Pan;
            cue.Volume = Volume;
        }

    }
}
