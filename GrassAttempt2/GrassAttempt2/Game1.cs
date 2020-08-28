using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GrassAttempt2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Block;
        List<Vector2> ThreePoints = new List<Vector2>();
        List<Vector2> HalfPoints = new List<Vector2>();
        List<Vector2> LineList = new List<Vector2>();
        SpriteFont Font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ThreePoints.Add(new Vector2(500, 200));
            ThreePoints.Add(new Vector2(350, 350));
            ThreePoints.Add(new Vector2(500, 450));

            for (int i = 0; i < 2; i++)
            {
                float Dist = Vector2.Distance(ThreePoints[i], ThreePoints[i + 1]);
                Vector2 Direction = ThreePoints[i] - ThreePoints[i + 1];
                Direction.Normalize();

                ThreePoints.Add(ThreePoints[i] - (Direction * (Dist / 3)));
                ThreePoints.Add(ThreePoints[i + 1] + (Direction * (Dist / 3)));
            }

            for (int i = -50; i < 50; i++)
            {

            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Block = Content.Load<Texture2D>("Block");
            Font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            for (int i = 0; i < ThreePoints.Count; i++)
            {
                spriteBatch.Draw(Block, new Rectangle((int)ThreePoints[i].X, (int)ThreePoints[i].Y, 5, 5), Color.White);
                spriteBatch.DrawString(Font, i.ToString(), new Vector2(ThreePoints[i].X + 5, ThreePoints[i].Y), Color.Yellow);
            }

            foreach (Vector2 halfPoint in HalfPoints)
            {
                spriteBatch.Draw(Block, new Rectangle((int)halfPoint.X, (int)halfPoint.Y, 5, 5), Color.Red);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
