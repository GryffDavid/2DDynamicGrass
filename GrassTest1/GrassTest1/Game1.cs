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
            for (int i = -10; i < 10; i++)
            {
                GrassList.Add(new GrassBlade(i *0.01f, new Vector2(1920 / 2, 1080 / 2)));
            }

            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920/2, 1080/2)));
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
            GrassList[0].Block = Block;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

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

                //foreach (Vector2 point in blade.Normalized)
                //{
                //    spriteBatch.Draw(Block, new Rectangle((int)point.X, (int)point.Y, 2, 2), Color.White);
                //}

                foreach (Vector2 point in blade.Parabola)
                {
                    spriteBatch.Draw(Block, new Rectangle((int)point.X, (int)point.Y, 2, 2), Color.White);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
