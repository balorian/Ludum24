using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace CarmineEngine
{
    public class AudioDevice
    {
        float masterVolume = 1;
        public float MasterVolume { get { return masterVolume; } set { if (value >= 0 && value <= 1) masterVolume = value; SoundEffect.MasterVolume = masterVolume; } }

        public static void LoadSound(string waveDir)
        {
            Sound.Sounds[waveDir] = Engine.Content.Load<SoundEffect>(waveDir);
        }
        public static void LoadFolder(string waveFolder)
        {
            Dictionary<string, SoundEffect> loadedFolder = Engine.LoadFolder<SoundEffect>(waveFolder);
            foreach (KeyValuePair<string, SoundEffect> pair in loadedFolder)
                Sound.Sounds.Add(pair.Key, pair.Value);
        }

        public AudioDevice()
        {

        }

    }
}
