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
        public int[] indeces = new int[15];
        public List<Vector2> Tangents = new List<Vector2>();
        public List<Vector2> Normals = new List<Vector2>();
        public List<Vector2> Curve = new List<Vector2>();
        public List<Vector2> ThreePoints = new List<Vector2>();
        public float BaseWidth, Time;
        public GrassState OriginalState, CurrentState, PreviousState;
        static Random Random = new Random();
        float Offset;
        float time;

        float lerp75;
        float lerp50;
        float lerp25;

        public GrassBlade(float variable, Vector2 basePosition, Vector2 tipPosition, Vector2 controlPoint, float baseWidth)
        {
            BaseWidth = baseWidth;

            lerp75 = MathHelper.Lerp(0, BaseWidth, 0.75f);
            lerp50 = MathHelper.Lerp(0, BaseWidth, 0.5f);
            lerp25 = MathHelper.Lerp(0, BaseWidth, 0.255f);

            #region Set up the 3 points of the Bezier curve
            //The base of the blade
            ThreePoints.Add(new Vector2(basePosition.X, basePosition.Y));
            //Control point
            ThreePoints.Add(new Vector2(basePosition.X + controlPoint.X, basePosition.Y + controlPoint.Y));
            //The tip of the blade
            ThreePoints.Add(new Vector2(basePosition.X + tipPosition.X, basePosition.Y + tipPosition.Y));
            #endregion           
            
            OriginalState = new GrassState() 
            { 
                basePosition = ThreePoints[0], 
                controlPoint = ThreePoints[1], 
                tipPosition = ThreePoints[2] 
            };

            CurrentState = OriginalState;
            PreviousState = CurrentState;

            #region Set up the actual curve

            Curve.Add(ThreePoints[0]);

            for (float t = 0.33f; t < 0.9; t += 0.33f)
            {
                Vector2 newPoint = new Vector2(
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].X + (2 * t * (1 - t)) * (ThreePoints[1].X) + (float)Math.Pow(t, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - t, 2) * ThreePoints[0].Y + (2 * t * (1 - t)) * (ThreePoints[1].Y) + (float)Math.Pow(t, 2) * ThreePoints[2].Y);

                Curve.Add(newPoint);
            }

            Curve.Add(ThreePoints[2]);
            #endregion

            #region Create the tangents
            for (int i = 0; i < Curve.Count - 2; i++)
            {
                Tangents.Add(new Vector2((Curve[i].X - Curve[i + 2].X), (Curve[i].Y - Curve[i + 2].Y)));
            }

            foreach (Vector2 point in Tangents)
            {
                point.Normalize();
            }
            #endregion

            #region Create the normals
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
            #endregion

            #region Set up the vertices that determine the shape of the blade
            Vertices[0].Position = new Vector3(Curve[0].X + MathHelper.Lerp(0, BaseWidth, 0.75f), Curve[0].Y, 0);
            Vertices[0].Color = Color.DarkGreen;

            Vertices[1].Position = new Vector3(Curve[0].X - MathHelper.Lerp(0, BaseWidth, 0.75f), Curve[0].Y, 0);
            Vertices[1].Color = Color.DarkGreen;

            Vertices[2].Position = new Vector3(Curve[1].X + (MathHelper.Lerp(0, BaseWidth, 0.5f) * Normals[0].X), Curve[1].Y + (MathHelper.Lerp(0, BaseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[2].Color = Color.Green;

            Vertices[3].Position = new Vector3(Curve[1].X - (MathHelper.Lerp(0, BaseWidth, 0.5f) * Normals[0].X), Curve[1].Y - (MathHelper.Lerp(0, BaseWidth, 0.5f) * Normals[0].Y), 0);
            Vertices[3].Color = Color.Green;

            Vertices[4].Position = new Vector3(Curve[2].X + (MathHelper.Lerp(0, BaseWidth, 0.25f) * Normals[1].X), Curve[2].Y + (MathHelper.Lerp(0, BaseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[4].Color = Color.LawnGreen;

            Vertices[5].Position = new Vector3(Curve[2].X - (MathHelper.Lerp(0, BaseWidth, 0.25f) * Normals[1].X), Curve[2].Y - (MathHelper.Lerp(0, BaseWidth, 0.25f) * Normals[1].Y), 0);
            Vertices[5].Color = Color.LawnGreen;

            Vertices[6].Position = new Vector3(Curve[3].X, Curve[3].Y, 0);
            Vertices[6].Color = Color.LightGreen;
            #endregion

            indeces[0] = 2;
            indeces[1] = 0;
            indeces[2] = 1;
            indeces[3] = 2;
            indeces[4] = 1;
            indeces[5] = 3;
            indeces[6] = 4;
            indeces[7] = 2;
            indeces[8] = 3;
            indeces[9] = 4;
            indeces[10] = 3;
            indeces[11] = 5;
            indeces[12] = 6;
            indeces[13] = 4;
            indeces[14] = 5;

        }

        public void Update(GameTime gameTime)
        {
            //time = (float)gameTime.TotalGameTime.TotalMilliseconds/100;
            //Offset = MathHelper.Lerp((float)(Math.Sin(time)*10), Offset, 0.02f);

            //CurrentState.tipPosition.X = OriginalState.tipPosition.X + Offset;
            

            if (CurrentState.tipPosition != PreviousState.tipPosition)
                CreateCurve();            

            PreviousState = CurrentState;
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

        public void CreateCurve()
        {
            //-----------Set up all the points along the curve--------------//            
            ThreePoints[0] = CurrentState.basePosition;
            ThreePoints[1] = CurrentState.controlPoint;
            ThreePoints[2] = CurrentState.tipPosition;

            //New basePoint from CurrentState input
            Curve[0] = ThreePoints[0];

            Curve[1] = new Vector2(
                    (float)Math.Pow(1 - 0.33f, 2) * ThreePoints[0].X + (2 * 0.33f * (1 - 0.33f)) * (ThreePoints[1].X) + (float)Math.Pow(0.33f, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - 0.33f, 2) * ThreePoints[0].Y + (2 * 0.33f * (1 - 0.33f)) * (ThreePoints[1].Y) + (float)Math.Pow(0.33f, 2) * ThreePoints[2].Y);

            Curve[2] = new Vector2(
                    (float)Math.Pow(1 - 0.66f, 2) * ThreePoints[0].X + (2 * 0.66f * (1 - 0.66f)) * (ThreePoints[1].X) + (float)Math.Pow(0.66f, 2) * ThreePoints[2].X,
                    (float)Math.Pow(1 - 0.66f, 2) * ThreePoints[0].Y + (2 * 0.66f * (1 - 0.66f)) * (ThreePoints[1].Y) + (float)Math.Pow(0.66f, 2) * ThreePoints[2].Y);

            //New tipPoint from CurrentState input
            Curve[3] = ThreePoints[2];

            //Set up the vertices based on CurrentState input
            Vertices[0].Position = new Vector3(Curve[0].X + lerp75, Curve[0].Y, 0);
            //Vertices[0].Color = Color.DarkGreen;

            Vertices[1].Position = new Vector3(Curve[0].X - lerp75, Curve[0].Y, 0);
            //Vertices[1].Color = Color.DarkGreen;

            Vertices[2].Position = new Vector3(Curve[1].X + (lerp50 * Normals[0].X), Curve[1].Y + (lerp50 * Normals[0].Y), 0);
            //Vertices[2].Color = Color.Green;

            Vertices[3].Position = new Vector3(Curve[1].X - (lerp50 * Normals[0].X), Curve[1].Y - (lerp50 * Normals[0].Y), 0);
            //Vertices[3].Color = Color.Green;

            Vertices[4].Position = new Vector3(Curve[2].X + (lerp25 * Normals[1].X), Curve[2].Y + (lerp25 * Normals[1].Y), 0);
            //Vertices[4].Color = Color.LawnGreen;

            Vertices[5].Position = new Vector3(Curve[2].X - (lerp25 * Normals[1].X), Curve[2].Y - (lerp25 * Normals[1].Y), 0);
            //Vertices[5].Color = Color.LawnGreen;

            Vertices[6].Position = new Vector3(Curve[3].X, Curve[3].Y, 0);
            //Vertices[6].Color = Color.LightGreen;
        }
    }
}
