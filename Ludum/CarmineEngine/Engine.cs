using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CarmineEngine
{
    class ScreenOrderComparer : IComparer<GameScreen> {
        public int Compare(GameScreen a, GameScreen b) {
            return a.DrawOrder.CompareTo(b.DrawOrder);
        }
    }

    public static class Engine
    {
        public const string UTIL_SCREEN = "default_util_screen";
        public const string CURSOR_SPRITE = "cursor";

        public static GameWindow Window;
        public static GameScreen Util;
        public static GraphicsDevice Device;
        public static MouseDevice Mouse;
        public static AudioDevice Audio;
        public static KeyboardDevice Keyboard;
        public static SpriteBatch SpriteBatch;
        public static ContentManager Content;
        public static SamplerState SamplerState = SamplerState.PointClamp;
        public static Dictionary<string, GameScreen> Screens;
        public static GameTime GameTime;
        public static SpriteFont DefaultFont;

        public static void Setup(GraphicsDeviceManager manager, ContentManager content, GameWindow window)
        {
            Device = manager.GraphicsDevice;
            SpriteBatch = new SpriteBatch(Device);
            Engine.Content = content;
            Engine.Screens = new Dictionary<string, GameScreen>();
            Window = window;
            new GameScreen(UTIL_SCREEN, 100);
            Util = Screens[UTIL_SCREEN];
            Audio = new AudioDevice();

            Keyboard = new KeyboardDevice();

            new SpriteTemplate(CURSOR_SPRITE, "cursor");
            Mouse = new MouseDevice(CURSOR_SPRITE, Window);
        }

        public static Dictionary<string, T> LoadFolder<T>(string folderDir)
        {
            DirectoryInfo dir = new DirectoryInfo(Content.RootDirectory + "\\" + folderDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            Dictionary<string, T> result = new Dictionary<string, T>();

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo f in files)
                result[Path.GetFileNameWithoutExtension(f.Name)] = Content.Load<T>(folderDir + "/" + Path.GetFileNameWithoutExtension(f.Name));

            return result;
        }

        public static void Update(GameTime gameTime)
        {
            Engine.GameTime = gameTime;
            Mouse.update();
            Keyboard.update();

            foreach (KeyValuePair<string, GameScreen> screenPair in Screens)
                if (screenPair.Value.AcceptUpdate)
                    screenPair.Value.update();
        }

        public static void Draw(GameTime gameTime)
        {
            Engine.GameTime = gameTime;

            List<GameScreen> drawOrder = new List<GameScreen>();
            foreach (KeyValuePair<string, GameScreen> screenPair in Screens)
                drawOrder.Add(screenPair.Value);
            drawOrder.Sort(new ScreenOrderComparer());

            foreach (GameScreen screen in drawOrder)
                if (screen.AcceptDraw)
                {
                    screen.draw();
                }
        }

        public static void AddScreen(string name, GameScreen screen)
        {
            Screens.Add(name, screen);
        }

        public static void RemoveScreen(string name)
        {
            Screens.Remove(name);
        }
    }
}
