using System;
using System.Collections.Generic;
using System.Linq;
using CarmineEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ludum
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GameScreen testScreen;
        Player player;

        TextField field;
        public static string Text;

        Entity[] ground;
        Entity[] wall;
        Entity slope;
        Entity box;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (1024);
            graphics.PreferredBackBufferHeight = (768);
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Evolution";

            Engine.Setup(graphics, Content, Window);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            testScreen = new GameScreen("testScreen");
            SpriteTemplate.LoadFolder("tiles/templates");
            SpriteTemplate.LoadFolder("sprites/templates");

            Engine.DefaultFont = Content.Load<SpriteFont>("default_font");

            player = new Player(testScreen, new Rectangle(0, 0, 64, 64), "slim_box");
            player.moveTo(64, 64);
            field = new TextField(testScreen, new Rectangle(0, 0, 1024, 768));
            field.Tint = Color.Red;
            field.DrawShadow = true;
            player.DrawOrder = 100;

            ground = new Entity[50];
            wall = new Entity[50];

            for (int i = 0; i < 50; i++)
            {
                Entity tile = new Entity(testScreen, new Rectangle(0, 0, 64, 64), "tile");
                tile.moveTo(i*64, 700);
                ground[i] = tile;
                Entity wallTile = new Entity(testScreen, new Rectangle(0, 0, 64, 64), "tile");
                wallTile.moveTo(500, i * 64);
            }

            slope = new Entity(testScreen, "slope", "slope");
            slope.moveTo(200, 200);
            slope.UseGeometry = true;

            box = new Entity(testScreen, new Rectangle(0, 0, 64, 64), "box");

            Text = "";
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Engine.Keyboard.pressed(Key.Escape))
                this.Exit();

            if (Engine.Keyboard.pressed(Key.R))
                player.moveTo(64, 64);

            if (Engine.Keyboard.held(Key.Up))
            {
                box.moveTo(box.Position.X, box.Position.Y - 1);
            }
            if (Engine.Keyboard.held(Key.Down))
            {
                box.moveTo(box.Position.X, box.Position.Y + 1);
            }
            if (Engine.Keyboard.held(Key.Left))
            {
                box.moveTo(box.Position.X -1 , box.Position.Y);
            }
            if (Engine.Keyboard.held(Key.Right))
            {
                box.moveTo(box.Position.X + 1, box.Position.Y);
            }

            Text += "\r\n";
            //Text += player.Velocity.ToString() + "\r\n" + player.Position.ToString() + "\r\n" + player.Acceleration.ToString();
            Text = box.collidesWith(slope).ToString();

            field.Text = Text;

            Text = "";

            Engine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Engine.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
