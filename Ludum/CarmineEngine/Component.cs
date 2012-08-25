using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CarmineEngine
{
    public class Component
    {
        public GameScreen Screen { get { return screen; } set { screen.removeComponent(this); screen = value; screen.addComponent(this); } }
        public bool Visible = true;
        public bool BlockUpdate = false;
        public bool OverrideUpdate = false;
        public bool OverrideDraw = false;
        public int DrawOrder;

        GameScreen screen;

        public Component(GameScreen parent)
        {
            initialize(parent, 0);
        }

        public Component(GameScreen parent, int order)
        {
            initialize(parent, order);
        }

        protected virtual void initialize(GameScreen parent, int order)
        {
            screen = parent;
            DrawOrder = order;
            screen.addComponent(this);
        }

        public virtual void update()
        {
        }

        public virtual void draw()
        {
        }

        public virtual void disable()
        {
            Screen.removeComponent(this);
        }
    }
}
