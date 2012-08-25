using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    class ComponentOrderComparer : IComparer<Component>
    {
        public int Compare(Component a, Component b)
        {
            return a.DrawOrder.CompareTo(b.DrawOrder);
        }
    }

    public class GameScreen
    {

        public bool AcceptUpdate = true;
        public bool AcceptDraw = true;
        public int DrawOrder;
        public Camera Camera = new Camera();
        public SpriteSortMode SpriteSortMode = SpriteSortMode.Immediate;
        public string Name;

        List<Component> components;
        Dictionary<string, GameScreen> screens;

        public GameScreen(string name)
        {
            initialize(name, 0, null);
        }
        public GameScreen(string name, int order)
        {
            initialize(name, order, null);
        }
        public GameScreen(string name, int order, GameScreen screen)
        {
            initialize(name, order, screen);
        }

        public virtual void initialize(string name, int order, GameScreen screen)
        {
            components = new List<Component>();
            screens = new Dictionary<string, GameScreen>();
            Name = name;
            DrawOrder = order;
            if (screen != null)
                screen.addScreen(Name, this);
            else
                Engine.AddScreen(Name, this);
            
        }

        public virtual void update()
        {
            if(screens != null)
            foreach (KeyValuePair<string, GameScreen> screenPair in screens)
                if (screenPair.Value.AcceptUpdate)
                    screenPair.Value.update();

            Camera.update();
            if (AcceptUpdate)
                foreach (Component c in components)
                    if(!c.OverrideUpdate && !c.BlockUpdate)
                        c.update();
        }

        public virtual void draw()
        {
            List<GameScreen> drawOrder = new List<GameScreen>();
            foreach (KeyValuePair<string, GameScreen> screenPair in screens)
                drawOrder.Add(screenPair.Value);
            drawOrder.Sort(new ScreenOrderComparer());

            foreach (GameScreen screen in drawOrder)
                if (screen.AcceptDraw)
                {
                    screen.draw();
                }

            Engine.SpriteBatch.Begin(SpriteSortMode, null, Camera.SampleState, null, null, null, Camera.crateTransformationMatrix());

            components.Sort(new ComponentOrderComparer());

            if (AcceptDraw)
                foreach (Component c in components)
                    if (c.Visible && !c.OverrideDraw)
                        c.draw();

            Engine.SpriteBatch.End();
        }

        public virtual void disable()
        {
            components.Clear();
            Engine.RemoveScreen(Name);
        }

        public void removeComponent(Component component)
        {
            components.Remove(component);
        }

        public void addComponent(Component component)
        {
            components.Add(component);
        }

        public void removeScreen(string name)
        {
            screens.Remove(name);
        }

        public void addScreen(string name, GameScreen screen)
        {
            screens.Add(name, screen);
        }
    }
}
