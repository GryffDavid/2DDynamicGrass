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

namespace GrassTest1
{    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect BasicEffect;
        SpriteFont Font;
        List<GrassBlade> GrassList = new List<GrassBlade>();
        Texture2D Block;
        RasterizerState RState = new RasterizerState()
        {
            CullMode = CullMode.None
        };
        static Random Random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2, 1080 / 2), new Vector2(-100, -150), new Vector2(0, -150), 20));
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2 - 25, 1080 / 2), new Vector2(-80, -250), new Vector2(-25, -150), 20));
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2 + 50, 1080 / 2), new Vector2(0, -300), new Vector2(-25, -150), 30));
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2 - 5, 1080 / 2), new Vector2(30, -150), new Vector2(-25, -75), 30));
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2 - 10, 1080 / 2), new Vector2(-50, -100), new Vector2(0, -25), 30));
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920 / 2 + 20, 1080 / 2), new Vector2(50, -100), new Vector2(0, -25), 30));

            for (int x = 0; x < 1920; x += 5)
            {
                for (int y = 800; y < 980; y += 5)
                {
                    GrassList.Add(new GrassBlade(0.09f, new Vector2(x + Random.Next(-20, 20), y + Random.Next(-20, 20)), new Vector2(Random.Next(-10, 10), -(15 + Random.Next(0, 15))), new Vector2(0, -10), Random.Next(1,3)));
                }
            }
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            BasicEffect = new BasicEffect(GraphicsDevice);
            BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            BasicEffect.VertexColorEnabled = true;

            Font = Content.Load<SpriteFont>("Font");

            Block = Content.Load<Texture2D>("Block");

            //foreach (GrassBlade blade in GrassList)
            //{
            //    blade.Block = Block;
            //    blade.Font = Font;
            //}
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            foreach (GrassBlade blade in GrassList)
            {
                blade.Update(gameTime);
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.RasterizerState = RState;
            GraphicsDevice.Clear(Color.DarkGreen);
            
            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (GrassBlade blade in GrassList)
                {
                    graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, blade.Vertices, 0, 5, VertexPositionColor.VertexDeclaration);
                }
            }

            spriteBatch.Begin();
            foreach (GrassBlade blade in GrassList)
            {
                blade.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
