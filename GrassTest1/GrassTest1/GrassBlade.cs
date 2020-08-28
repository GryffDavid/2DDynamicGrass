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
        public List<Vector2> Parabola = new List<Vector2>();
        public List<Vector2> Tangents = new List<Vector2>();
        public List<Vector2> Normals = new List<Vector2>();

        public Texture2D Block;

        public GrassBlade(float variable, Vector2 position, int total, int start, float baseWidth)
        {
            for (int x = start; x < start+total; x += 1)
            {
                Parabola.Add(new Vector2((x*50) + position.X, (float)Math.Pow((variable * x)*50, 2) + position.Y));
                //Parabola.Add(new Vector2(x*50, (x*50) ^ 2));
            }

            for (int i = 0; i < Parabola.Count - 2; i++)
            {
                if (Parabola[i].X - position.X >= 0)
                {
                    Tangents.Add(new Vector2(Math.Abs(Parabola[i].X - Parabola[i + 2].X), Math.Abs(Parabola[i].Y - Parabola[i + 2].Y)));
                }
                else
                {
                    Tangents.Add(new Vector2((Parabola[i].X - Parabola[i + 2].X), (Parabola[i].Y - Parabola[i + 2].Y)));
                }
            }

            foreach (Vector2 point in Tangents)
            {
                point.Normalize();
            }

            foreach (Vector2 tangent in Tangents)
            {
                double angle = Math.Atan2(tangent.Y, tangent.X) - MathHelper.ToRadians(90);
                Normals.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
            }

            foreach (Vector2 point in Normals)
            {
                point.Normalize();
            }

            Vertices[0].Position = new Vector3(Parabola[0].X, Parabola[0].Y, 0);
            Vertices[0].Color = Color.DarkGreen;

            Vertices[1].Position = new Vector3(Parabola[1].X - (MathHelper.Lerp(0, baseWidth, 0.33f) * Normals[0].X), Parabola[1].Y - (MathHelper.Lerp(0, baseWidth, 0.33f) * Normals[0].Y), 0);
            Vertices[1].Color = Color.DarkGreen;

            Vertices[2].Position = new Vector3(Parabola[1].X + (MathHelper.Lerp(0, baseWidth, 0.33f) * Normals[0].X), Parabola[1].Y + (MathHelper.Lerp(0, baseWidth, 0.33f) * Normals[0].Y), 0);
            Vertices[2].Color = Color.DarkGreen;

            Vertices[3].Position = new Vector3(Parabola[2].X - (MathHelper.Lerp(0, baseWidth, 0.66f) * Normals[1].X), Parabola[2].Y - (MathHelper.Lerp(0, baseWidth, 0.66f) * Normals[1].Y), 0);
            Vertices[3].Color = Color.DarkGreen;

            Vertices[4].Position = new Vector3(Parabola[2].X + (MathHelper.Lerp(0, baseWidth, 0.66f) * Normals[1].X), Parabola[2].Y + (MathHelper.Lerp(0, baseWidth, 0.66f) * Normals[1].Y), 0);
            Vertices[4].Color = Color.DarkGreen;

            Vertices[5].Position = new Vector3(Parabola[3].X - (MathHelper.Lerp(0, baseWidth, 1f)), Parabola[3].Y, 0);
            Vertices[5].Color = Color.DarkGreen;

            Vertices[6].Position = new Vector3(Parabola[3].X + (MathHelper.Lerp(0, baseWidth, 1f)), Parabola[3].Y, 0);
            Vertices[6].Color = Color.DarkGreen;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //for (float f = -10; f < 10; f += 0.001f)
            //{
            //    foreach (Vector2 point in Normalized)
            //    {
            //        spriteBatch.Draw(Block, new Rectangle((int)(point.X + (f * Normalized.IndexOf(point))), (int)(point.Y - (f * Normalized.IndexOf(point))), 1, 1), Color.Red);
            //    }
            //}
            for (float i = -1; i < 1; i += 0.01f)
            {
                for (int p = 0; p < Tangents.Count; p++)
                {
                    spriteBatch.Draw(Block, new Rectangle((int)Parabola[p].X + (int)(Tangents[p].X * i), (int)Parabola[p].Y + (int)(Tangents[p].Y * i), 1, 1), Color.Red);
                }
            }

            //for (float i = -1000; i < 1000; i += 1f)
            //{
            //    for (int p = 0; p < Normals.Count; p++)
            //    {
            //        spriteBatch.Draw(Block, new Rectangle(100 + (int)Tangents[p].X + (int)(Normals[p].X * i), 100 + (int)Tangents[p].Y + (int)(Normals[p].Y * i), 1, 1), Vertices[Math.Min(p, 6)].Color);
            //    }
            //}
        }
    }
}
