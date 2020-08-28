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
    struct GrassState 
    { 
        public Vector2 tipPosition;
        public Vector2 controlPoint;
        public Vector2 basePosition;
    }

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
        RasterizerState MultiSamplingState = new RasterizerState()
        {
            MultiSampleAntiAlias = true,
            CullMode = CullMode.CullCounterClockwiseFace,
            FillMode = FillMode.Solid,
            ScissorTestEnable = false,
            DepthBias = 0,
            SlopeScaleDepthBias = 0            
        };
        float Offset, time;

        RenderTarget2D GrassRenderTarget;
        MouseState CurrentMouseState, PreviousMouseState;
        KeyboardState CurrentKeyboardState, PreviousKeyboardState;
        VertexBuffer myVertexBuffer;
        IndexBuffer myIndexBuffer;

        FrameRateCounter FPSCounter = new FrameRateCounter();

        int XDensity = 10;
        int YDensity = 10;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            
        }
        
        protected override void Initialize()
        {
            for (int x = 0; x < 1280; x += XDensity)
            {
                for (int y = 0; y < 720; y += YDensity)
                {
                    GrassList.Add(new GrassBlade(0.09f, new Vector2(x + Random.Next(-20, 20), y + Random.Next(-20, 20)), new Vector2(Random.Next(-10, 10), -(15 + Random.Next(0, 50))), new Vector2(Random.Next(-3, 3), -(10+Random.Next(0, 8))), Random.Next(1, 4)));
                }
            }

            GrassRenderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720,
                false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8, 16, RenderTargetUsage.DiscardContents);
            //GrassList.Add(new GrassBlade(0.09f, new Vector2(1920/2, 1080/2), new Vector2(-150, -250), new Vector2(-200, -100), 30));
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

            FPSCounter.spriteFont = Font;
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            FPSCounter.Update(gameTime);

            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyUp(Keys.A) && PreviousKeyboardState.IsKeyDown(Keys.A))
            {
                XDensity -= 1;
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.S) && PreviousKeyboardState.IsKeyDown(Keys.S))
            {
                XDensity += 1;
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.Z) && PreviousKeyboardState.IsKeyDown(Keys.Z))
            {
                YDensity -= 1;
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.X) && PreviousKeyboardState.IsKeyDown(Keys.X))
            {
                YDensity += 1;
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.F5) && PreviousKeyboardState.IsKeyDown(Keys.F5))
            {
                ResetGrass(XDensity, YDensity);
            }

            foreach (GrassBlade blade in GrassList)
            {
                if (CurrentMouseState.X > PreviousMouseState.X)
                {
                    if (Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), blade.OriginalState.basePosition) < 100)
                    {
                        float PercentDist = 1 - ((100 / 100) * Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), blade.OriginalState.basePosition)) / 100;
                        if (blade.CurrentState.tipPosition.X > Mouse.GetState().X)
                        {
                            //blade.CurrentState.controlPoint = Vector2.Lerp(blade.OriginalState.controlPoint, blade.OriginalState.controlPoint + (Vector2.One * (20 * PercentDist)), 0.8f * PercentDist);
                            blade.CurrentState.tipPosition.X = MathHelper.Lerp(blade.OriginalState.tipPosition.X, blade.OriginalState.tipPosition.X + (50 * PercentDist), 0.8f * PercentDist);
                        }
                    }
                    blade.Update(gameTime);
                }
                else
                {
                    //blade.CurrentState.controlPoint = Vector2.Lerp(blade.CurrentState.controlPoint, blade.OriginalState.controlPoint, (float)(0.05f * Random.NextDouble() * 2));
                    blade.CurrentState.tipPosition.X = MathHelper.Lerp(blade.CurrentState.tipPosition.X, blade.OriginalState.tipPosition.X, (float)(0.05f * Random.NextDouble() * 2));
                    blade.Update(gameTime);
                }

                if (CurrentMouseState.X < PreviousMouseState.X)
                {
                    if (Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), blade.OriginalState.basePosition) < 100)
                    {
                        float PercentDist = 1 - ((100 / 100) * Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), blade.OriginalState.basePosition)) / 100;
                        if (blade.CurrentState.tipPosition.X < Mouse.GetState().X)
                        {
                            //blade.CurrentState.controlPoint = Vector2.Lerp(blade.OriginalState.controlPoint, blade.OriginalState.controlPoint - (Vector2.One * (20 * PercentDist)), 0.8f * PercentDist);
                            blade.CurrentState.tipPosition.X = MathHelper.Lerp(blade.OriginalState.tipPosition.X, blade.OriginalState.tipPosition.X - (50 * PercentDist), 0.8f * PercentDist);
                        }
                    }
                    blade.Update(gameTime);
                }
                else
                {
                    //blade.CurrentState.controlPoint = Vector2.Lerp(blade.CurrentState.controlPoint, blade.OriginalState.controlPoint, (float)(0.05f * Random.NextDouble() * 2));
                    blade.CurrentState.tipPosition.X = MathHelper.Lerp(blade.CurrentState.tipPosition.X, blade.OriginalState.tipPosition.X, (float)(0.05f * Random.NextDouble() * 2));
                    blade.Update(gameTime);
                }
            }

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.SetRenderTarget(GrassRenderTarget);
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.RasterizerState = RState;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, MultiSamplingState);
            foreach (EffectPass pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (GrassBlade blade in GrassList)
                {
                    graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, blade.Vertices, 0, 5, VertexPositionColor.VertexDeclaration);
                }
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
                spriteBatch.Draw(GrassRenderTarget, new Rectangle(0, 0, 1280, 720), Color.White);
                FPSCounter.Draw(spriteBatch);
                spriteBatch.DrawString(Font, "Grass X Spacing [A/S to change]: " + XDensity.ToString(), new Vector2(32, 48), Color.White);
                spriteBatch.DrawString(Font, "Grass Y Spacing [Z/X to change]: " + YDensity.ToString(), new Vector2(32, 64), Color.White);
                spriteBatch.DrawString(Font, "Press F5 to recreate grass", new Vector2(32, 80), Color.White);
                spriteBatch.DrawString(Font, "Total Blades: " + GrassList.Count.ToString(), new Vector2(32, 96), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void ResetGrass(int xDensity, int yDensity)
        {
            GrassList.Clear();

            for (int x = 0; x < 1280; x += xDensity)
            {
                for (int y = 0; y < 720; y += yDensity)
                {
                    GrassList.Add(new GrassBlade(0.09f, new Vector2(x + Random.Next(-20, 20), y + Random.Next(-20, 20)), new Vector2(Random.Next(-10, 10), -(15 + Random.Next(0, 50))), new Vector2(Random.Next(-3, 3), -(10 + Random.Next(0, 8))), Random.Next(1, 4)));
                }
            }
        }
    }
}
