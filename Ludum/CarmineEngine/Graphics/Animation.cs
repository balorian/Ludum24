using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarmineEngine
{
    public class Animation
    {

        public Sprite Parent;
        public string Name;
        public float Speed = 1f;
        public bool Loops = true;
        public bool Paused = false;

        List<int> frames = new List<int>();
        List<int> times = new List<int>();
        int index = 0;
        int timer = 0;

        public Animation(string name)
        {
            Name = name;
        }
        public Animation(string name, bool loop)
        {
            Name = name;
            Loops = loop;
        }
        public Animation(string name, int[] frames, int[] times, bool loop)
        {
            Name = name;
            Loops = loop;
            foreach (int i in frames)
                this.frames.Add(i);
            foreach (int i in times)
                this.times.Add(i);
        }
        public Animation(string name, int start, int length, int[] times, bool reverse, bool loop)
        {
            Name = name;
            Loops = loop;

            for (int i = 0; i < length; i++)
            {
                frames.Add(i + start);
                this.times.Add(times[i]);
            }
            if (reverse && length > 2)
            {
                for (int i = length - 2; i > 0; i--)
                {
                    frames.Add(i + start);
                    this.times.Add(times[i]);
                }
            }
        }

        public void addFrame(int frame, int time)
        {
            frames.Add(frame);
            times.Add(time);
        }
        public void addFrame(int index, int frame, int time)
        {
            frames.Insert(index, frame);
            times.Insert(index, time);
        }

        public void removeFrameAt(int index)
        {
            frames.RemoveAt(index);
        }
        public void removeFrame(int frame)
        {
            frames.Remove(frame);
        }

        internal void update()
        {
            if (!Paused)
            {
                timer += (int)(Engine.GameTime.ElapsedGameTime.TotalMilliseconds * Speed);
                if (timer > times[index])
                {
                    timer -= times[index];
                    nextFrame(timer);
                }
            }
        }

        internal void play()
        {
            reset();
            Paused = false;
        }
        internal void play(int overflow)
        {
            play();
            timer += overflow;
        }
        internal void reset()
        {
            index = 0;
            timer = 0;
        }

        internal void nextFrame(int overflow)
        {
            if (index == frames.Count - 1)
            {
                if (Loops)
                    index = 0;
                else
                    Parent.playNext(overflow);
            }
            else
            {
                index++;
            }

            timer = overflow;
            if (timer > times[index])
            {
                timer -= times[index];
                nextFrame(timer);
            }
        }

        internal int getFrame()
        {
            return frames[index];
        }

        internal Animation copy()
        {
            Animation returnCopy = new Animation(Name, frames.ToArray(), times.ToArray(), Loops);
            returnCopy.Paused = Paused;
            returnCopy.Speed = Speed;
            return returnCopy;
        }
    }
}
