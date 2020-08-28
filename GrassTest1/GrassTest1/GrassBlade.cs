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
        public List<Vector2> Curve = new List<Vector2>();
        public List<Vector2> ThreePoints = new List<Vector2>();
        //public SpriteFont Font;
        //public Texture2D Block;

        public GrassBlade(float variable, Vector2 basePosition, Vector2 tipPosition, Vector2 controlPoint, float baseWidth)
        {
            //The base of the blade
            ThreePoints.Add(new Vector2(basePosition.X, basePosition.Y));
            //Control point
            ThreePoints.Add(new Vector2(basePosition.X + controlPoint.X, basePosition.Y + controlPoint.Y));            
            //The tip of the blade
            ThreePoints.Add(new Vector2(basePosition.X + tipPosition.X, basePosition.Y + tipPosition.Y));

            Curve.Add(ThreePoints[0]);

            for (float t = 0.33f; t < 0.9; t += 0.33f)
            {
                Vector2 newPoint = new Vector2(
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].X + (2 * t * (1 - t)) * (ThreePoints[1].X) + (float)Math.Pow(t, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].Y + (2 * t * (1 - t)) * (ThreePoints[1].Y) + (float)Math.Pow(t, 2) * ThreePoints[2].Y);

                Curve.Add(newPoint);
            }

            Curve.Add(ThreePoints[2]);


            for (int i = 0; i < Curve.Count - 2; i++)
            {
                Tangents.Add(new Vector2((Curve[i].X - Curve[i + 2].X), (Curve[i].Y - Curve[i + 2].Y)));
            }

            foreach (Vector2 point in Tangents)
            {
                point.Normalize();
            }

            foreach (Vector2 tangent in Tangents)
            {
                Normals.Add(new Vector2(
                    (float)Math.Cos(Math.Atan2(tangent.Y, tangent.X) - MathHelper.ToRadians(90)), 
                    (float)Math.Sin(Math.Atan2(tangent.Y, tangent.X) - MathHelper.ToRadians(90))));
            }

            foreach (Vector2 point in Normals)
            {
                point.Normalize();
            }

            Vertices[0].Position = new Vector3(Curve[0].X + MathHelper.Lerp(0, baseWidth, 0.75f), Curve[0].Y, 0);
            Vertices[0].Color = Color.DarkGreen;

            Vertices[1].Position = new Vector3(Curve[0].X - MathHelper.Lerp(0, baseWidth, 0.75f), Curve[0].Y, 0);
            Vertices[1].Color = Color.DarkGreen;

            Vertices[2].Position = new Vector3(Curve[1].X + (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].X), Curve[1].Y + (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[2].Color = Color.Green;

            Vertices[3].Position = new Vector3(Curve[1].X - (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].X), Curve[1].Y - (MathHelper.Lerp(0, baseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[3].Color = Color.Green;

            Vertices[4].Position = new Vector3(Curve[2].X + (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].X), Curve[2].Y + (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[4].Color = Color.LawnGreen;

            Vertices[5].Position = new Vector3(Curve[2].X - (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].X), Curve[2].Y - (MathHelper.Lerp(0, baseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[5].Color = Color.LawnGreen;

            Vertices[6].Position = new Vector3(Curve[3].X, Curve[3].Y, 0);
            Vertices[6].Color = Color.LightGreen;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ////Draw the line showing the tangents to the curve/lines connecting the points on the curve
            //for (float i = -1; i < 1; i += 0.01f)
            //{
            //    for (int p = 0; p < Tangents.Count; p++)
            //    {
            //        spriteBatch.Draw(Block, new Rectangle((int)Curve[p].X + (int)(Tangents[p].X * i), (int)Curve[p].Y + (int)(Tangents[p].Y * i), 1, 1), Color.Red);
            //    }
            //}

            ////Draw the line of the normals
            //for (float i = -1000; i < 1000; i++)
            //{
            //    spriteBatch.Draw(Block, new Rectangle((int)(ThreePoints[1].X) + (int)(Normals[1].X * i), (int)(ThreePoints[1].Y) + (int)(Normals[1].Y * i), 1, 1), Color.Black);
            //    spriteBatch.Draw(Block, new Rectangle((int)(ThreePoints[1].X) + (int)(Normals[0].X * i), (int)(ThreePoints[1].Y) + (int)(Normals[0].Y * i), 1, 1), Color.Black);

            //}


            ////The points representing the points of the curve
            //for (int i = 0; i < Curve.Count(); i++)
            //{
            //    spriteBatch.Draw(Block, new Rectangle((int)Curve[i].X, (int)Curve[i].Y, 5, 5), Color.White);
            //}

            ////The indeces of the points on the curve
            //for (int i = 0; i < Curve.Count(); i++)
            //{
            //    spriteBatch.DrawString(Font, i.ToString(), Curve[i], Color.Yellow);
            //}

            ////The indeces of the points on the curve
            //for (int i = 0; i < ThreePoints.Count(); i++)
            //{
            //    spriteBatch.Draw(Block, new Rectangle((int)ThreePoints[i].X, (int)ThreePoints[i].Y, 5, 5), Color.Red);
            //}
        }
    }
}
