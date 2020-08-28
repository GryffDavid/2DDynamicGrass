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
        List<Vector2> ParabolaPointList = new List<Vector2>();
        List<Vector2> ParabolaPointList2 = new List<Vector2>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            for (float x = -25; x < 0; x+=0.1f)
            {
                //ParabolaPointList.Add(
                //    new Vector2(
                //    x + (1280/2), //The X Value, offset by 1280/2
                //    (float)Math.Pow(x, 2) + (720/2) //The Y value - X ^ 2 offset by 720/2
                //    ));

                //Parabola 1
                ParabolaPointList.Add(
                    new Vector2(
                    x, //The X Value
                    0.05f*(float)Math.Pow(x, 2) + (x*2) //The Y value
                    ));
            }

            for (float x = 0; x < 25; x += 0.1f)
            {
                //Parabola 2
                ParabolaPointList2.Add(
                    new Vector2(
                    x, //The X Value
                    0.1f * (float)Math.Pow(x, 2) - (x * 2) //The Y value
                    ));
            }
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Block = Content.Load<Texture2D>("Block");
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
            foreach (Vector2 point in ParabolaPointList)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Block, new Rectangle((int)point.X + (1280 / 2), (int)point.Y + (720 / 2), 1, 1), Color.White);
                spriteBatch.End();
            }

            foreach (Vector2 point in ParabolaPointList2)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Block, new Rectangle((int)point.X + (1280/2), (int)point.Y + (720/2), 1, 1), Color.Red);
                spriteBatch.End();
            }    
            base.Draw(gameTime);
        }
    }
}
