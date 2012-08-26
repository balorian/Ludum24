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
using Ludum.Level;

namespace Ludum
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GameScreen testScreen;
        Player player;

        TextField field;
        string text;

        Entity[] ground;
        Entity slope;

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
            testScreen.AcceptUpdate = false;
            testScreen.AcceptDraw = false;
            
            SpriteTemplate.LoadFolder("tiles/templates");
            SpriteTemplate.LoadFolder("sprites/templates");

            Engine.DefaultFont = Content.Load<SpriteFont>("default_font");

            player = new Player(testScreen, new Rectangle(0, 0, 64, 64), "box");
            player.moveTo(64, 64);
            field = new TextField(testScreen, new Rectangle(0, 0, 600, 600));
            field.Tint = Color.Red;
            field.DrawShadow = true;
            player.DrawOrder = 100;

            GameState state = new GameState();
            state.Player = player;
            state.loadLevels();

            ground = new Entity[50];

            for (int i = 0; i < 50; i++)
            {
                Entity tile = new Entity(testScreen, new Rectangle(0, 0, 64, 64), "tile");
                tile.moveTo(i*64, 700);
                ground[i] = tile;
            }

            slope = new Entity(testScreen, "slope", "slope");
            slope.moveTo(200, 200);
            slope.UseGeometry = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Engine.Keyboard.pressed(Key.Escape))
                this.Exit();

            text = player.Velocity.ToString() + "\r\n" + player.Position.ToString();
            field.Text = text;

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
