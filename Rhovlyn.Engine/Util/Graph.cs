using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Rhovlyn.Engine.Util
{
	public class Graph : Engine.Graphics.IDrawable
    {
		public List<double> Data { get; set; }
		public bool ZeroIsMinimum { get ; set; }
		public int MaxDataPoints { get; set; }

		private double max = double.MinValue;
		private double min = 0;
		private double intervals = 0;

		private Vector2[] points;

		public Vector2 Position { get; set; }
		public Rectangle Area { get; set; }

		public Graph(Vector2 position , int width , int height)
		{
			this.Data = new List<double>();
			this.ZeroIsMinimum = false;
			this.MaxDataPoints = 100;
			this.Position = position;
			this.Area = new Rectangle((int)this.Position.X , (int)this.Position.Y, width, height);
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				C3.XNA.Primitives2D.DrawLine( spriteBatch , points[i] , points[i+1] , Color.White );
			}
		}

		public void Update (GameTime gameTime)
		{
			while (Data.Count > MaxDataPoints)
				Data.RemoveAt(0);

			points = new Vector2[Data.Count];
			intervals = this.Area.Width / MaxDataPoints;

			min = ( ZeroIsMinimum ? 0 : double.MaxValue);
			int n = 0;
			foreach (var pt in Data)
			{
				if (pt > max)
					max = pt;

				if (!ZeroIsMinimum && pt < min)
					min = pt;

				points[n] = new Vector2( 
					(float)(this.Position.X + n*intervals), 
					(float)(this.Position.Y + (pt-min)/(max-min)*this.Area.Height - this.Area.Height));
				n++;
			}
		}
    }
}

