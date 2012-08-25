using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CarmineEngine
{

    public enum MouseButton { Left, Right, Middle, X1, X2 }

    public class MouseDevice
    {
        public Vector2 Position { get { return position; } }
        public Vector2 Delta { get { return delta; } }
        public float ScrollPosition { get { return scrollPosition; } }
        public float ScrollDelta { get { return scrollDelta; } }
        public List<MouseButton> CurrentlyPressedButtons { get { return currentlyPressedButtons; } }
        public List<MouseButton> PreviousPressedButtons { get { return previousPressedButtons; } }

        MouseState current;
        MouseState previous;
        Vector2 delta;
        float scrollPosition;
        float scrollDelta;
        Sprite cursor;
        GameWindow window;
        Vector2 position;
        List<MouseButton> currentlyPressedButtons;
        List<MouseButton> previousPressedButtons;

        public MouseDevice(string cursorDir, GameWindow window)
        {
            cursor = new Sprite(Engine.Util, cursorDir);
            current = Mouse.GetState();
            this.window = window;
            currentlyPressedButtons = new List<MouseButton>();
            previousPressedButtons = new List<MouseButton>();
            update();
        }

        public void update()
        {
            previousPressedButtons.Clear();
            foreach (MouseButton b in currentlyPressedButtons)
                previousPressedButtons.Add(b);
            previous = current;
            current = Mouse.GetState();
            position = new Vector2(current.X, current.Y);
            delta = new Vector2(current.X - previous.X, current.Y - previous.Y);
            scrollPosition = (float)current.ScrollWheelValue;
            scrollDelta = (float)(current.ScrollWheelValue - previous.ScrollWheelValue);
            currentlyPressedButtons.Clear();

            if (Mouse.GetState().LeftButton.Equals(ButtonState.Pressed))
                currentlyPressedButtons.Add(MouseButton.Left);
            if (Mouse.GetState().RightButton.Equals(ButtonState.Pressed))
                currentlyPressedButtons.Add(MouseButton.Right);
            if (Mouse.GetState().MiddleButton.Equals(ButtonState.Pressed))
                currentlyPressedButtons.Add(MouseButton.Middle);
            if (Mouse.GetState().XButton1.Equals(ButtonState.Pressed))
                currentlyPressedButtons.Add(MouseButton.X1);
            if (Mouse.GetState().XButton2.Equals(ButtonState.Pressed))
                currentlyPressedButtons.Add(MouseButton.X2);

            cursor.Position = position;
            if (window.ClientBounds.Contains(current.X + window.ClientBounds.X, current.Y + window.ClientBounds.Y))
                cursor.Visible = true;
            else
                cursor.Visible = false;
        }

        public bool down(MouseButton button)
        {
            if (currentlyPressedButtons.Contains(button))
                return true;
            return false;
        }

        public bool up(MouseButton button)
        {
            if (!currentlyPressedButtons.Contains(button))
                return true;
            return false;
        }

        public bool held(MouseButton button)
        {
            if (down(button) && previousPressedButtons.Contains(button))
                return true;
            return false;
        }

        public bool pressed(MouseButton button)
        {
            if (down(button) && !previousPressedButtons.Contains(button))
                return true;
            return false;
        }

        public bool released(MouseButton button)
        {
            if (previousPressedButtons.Contains(button) && !down(button))
                return true;
            return false;
        }

    }
}
