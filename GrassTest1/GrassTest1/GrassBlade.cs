using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GrassTest1
{
    class GrassBlade
    {
        public VertexPositionColor[] Vertices = new VertexPositionColor[7];
        public List<Vector2> Tangents = new List<Vector2>();
        public List<Vector2> Normals = new List<Vector2>();
        public List<Vector2> ThreePoints = new List<Vector2>();
        public SpriteFont Font;
        public Texture2D Block;

        public GrassBlade(float variable, Vector2 position, int total, int start, float baseWidth)
        {
            ThreePoints.Add(new Vector2(450, 450));
            ThreePoints.Add(new Vector2(350, 350));
            ThreePoints.Add(new Vector2(450, 200));

            for (float t = 0.333f; t < 0.9; t += 0.333f)
            {
                Vector2 newPoint1 = new Vector2(
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].X + (2 * t * (1 - t)) * (ThreePoints[1].X) + (float)Math.Pow(t, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].Y + (2 * t * (1 - t)) * (ThreePoints[1].Y) + (float)Math.Pow(t, 2) * ThreePoints[2].Y);
                ThreePoints.Add(newPoint1);
            }

            ThreePoints.RemoveAt(1);

            Tangents.Add(new Vector2(Math.Abs(ThreePoints[0].X - ThreePoints[3].X), Math.Abs(ThreePoints[0].Y - ThreePoints[3].Y)));
            Tangents.Add(new Vector2((ThreePoints[1].X - ThreePoints[2].X), (ThreePoints[1].Y - ThreePoints[2].Y)));

            foreach (Vector2 point in Tangents)
            {
                point.Normalize();
            }

            foreach (Vector2 tangent in Tangents)
            {
                double angle = Math.Atan2(tangent.Y, tangent.X) - MathHelper.ToRadians(90);
                Normals.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
                //Normals.Add(new Vector2(1, 1));
            }

            foreach (Vector2 point in Normals)
            {
                point.Normalize();
            }

            Vertices[0].Position = new Vector3(ThreePoints[0].X + MathHelper.Lerp(0, baseWidth, 0.75f), ThreePoints[0].Y, 0);
            Vertices[0].Color = Color.DarkGreen;

            Vertices[1].Position = new Vector3(ThreePoints[0].X - MathHelper.Lerp(0, baseWidth, 0.75f), ThreePoints[0].Y, 0);
            Vertices[1].Color = Color.Red;

            Vertices[2].Position = new Vector3(ThreePoints[2].X + (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].X), ThreePoints[2].Y + (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[2].Color = Color.Yellow;

            Vertices[3].Position = new Vector3(ThreePoints[2].X - (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].X), ThreePoints[2].Y - (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[3].Color = Color.Orange;

            Vertices[4].Position = new Vector3(ThreePoints[3].X - (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].X), ThreePoints[3].Y - (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[4].Color = Color.Purple;

            Vertices[5].Position = new Vector3(ThreePoints[3].X + (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].X), ThreePoints[3].Y + (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[5].Color = Color.Black;

            Vertices[6].Position = new Vector3(ThreePoints[1].X, ThreePoints[1].Y, 0);
            Vertices[6].Color = Color.White;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (float i = -1; i < 1; i += 0.01f)
            {
                for (int p = 0; p < Tangents.Count; p++)
                {
                    spriteBatch.Draw(Block, new Rectangle((int)ThreePoints[p].X + (int)(Tangents[p].X * i), (int)ThreePoints[p].Y + (int)(Tangents[p].Y * i), 1, 1), Color.Red);
                }
            }

            for (float i = -1000; i < 1000; i += 1f)
            {
                spriteBatch.Draw(Block, new Rectangle((int)(ThreePoints[3].X) + (int)(Normals[1].X * i), (int)(ThreePoints[3].Y) + (int)(Normals[1].Y * i), 1, 1), Color.Black);
            }

            for (float i = -1000; i < 1000; i += 1f)
            {
                spriteBatch.Draw(Block, new Rectangle((int)(ThreePoints[2].X) + (int)(Normals[0].X * i), (int)(ThreePoints[2].Y) + (int)(Normals[0].Y * i), 1, 1), Color.White);
            }


            for (int i = 0; i < ThreePoints.Count; i++)
            {
                spriteBatch.Draw(Block, new Rectangle((int)ThreePoints[i].X, (int)ThreePoints[i].Y, 5, 5), Color.White);
            }

            for (int i = 0; i < ThreePoints.Count; i++)
            {
                spriteBatch.Draw(Block, new Rectangle((int)Vertices[i].Position.X, (int)Vertices[i].Position.Y, 3, 3), Color.Red);
            }

            for (int i = 0; i < ThreePoints.Count; i++)
            {
                spriteBatch.DrawString(Font, i.ToString(), ThreePoints[i], Color.Yellow);
            }

            for (int i = 0; i < Vertices.Length; i++)
            {
                spriteBatch.DrawString(Font, i.ToString(), new Vector2(Vertices[i].Position.X, Vertices[i].Position.Y), Color.Purple);
            }
        }
    }
}
