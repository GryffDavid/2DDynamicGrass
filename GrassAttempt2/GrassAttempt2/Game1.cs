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
            ThreePoints.Add(new Vector2(500, 450));
            ThreePoints.Add(new Vector2(350, 350));
            ThreePoints.Add(new Vector2(500, 100));

            for (float t = 0.333f; t < 0.9; t += 0.333f)
            {
                Vector2 newPoint1 = new Vector2(
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].X + (2 * t * (1 - t)) * (ThreePoints[1].X) + (float)Math.Pow(t, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].Y + (2 * t * (1 - t)) * (ThreePoints[1].Y) + (float)Math.Pow(t, 2) * ThreePoints[2].Y);
                ThreePoints.Add(newPoint1);
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
            //ThreePoints[0] = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
