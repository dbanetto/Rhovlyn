using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Rhovlyn.Engine.Util
{
	public enum MaxType
	{
		Auto = 0,
		AllTimeHigh = 1}

	;

	public struct Line
	{
		public Line(double point, Color colour)
		{
			this.point = point;
			this.colour = colour;
		}

		public double point;
		public Color colour;
	}

	public class Graph : Engine.Graphics.IDrawable
	{
		public List<double> Data { get; set; }

		public bool ZeroIsMinimum { get ; set; }

		public int MaxDataPoints { get; set; }

		public Color Colour { get; set; }

		public MaxType MaxMode { get; set; }

		private double max = double.MinValue;
		private double min = 0;
		private double intervals = 0;

		private Vector2[] points;

		public List<Line> Lines { get; set; }
		//Lines arcoss the

		public Vector2 Position { get; set; }

		public Rectangle Area { get; set; }

		public Graph(Vector2 position, int width, int height)
		{
			this.Data = new List<double>();
			this.Lines = new List<Line>();
			this.ZeroIsMinimum = false;
			this.MaxDataPoints = 100;
			this.Position = position;
			this.Area = new Rectangle((int)this.Position.X, (int)this.Position.Y, width, height);
			this.MaxMode = MaxType.Auto;
			this.Colour = Colour;
		}

		public Graph(Vector2 position, int width, int height, bool zeroIsMinimum, int maxDataPoints, MaxType type, Color colour)
		{
			this.Data = new List<double>();
			this.Lines = new List<Line>();
			this.ZeroIsMinimum = zeroIsMinimum;
			this.MaxDataPoints = maxDataPoints;
			this.Position = position;
			this.Area = new Rectangle((int)this.Position.X, (int)this.Position.Y, width, height);
			this.MaxMode = type;
			this.Colour = colour;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			if (points == null)
				Update(gameTime);

			for (int i = 0; i < points.Length - 1; i++)
			{
				C3.XNA.Primitives2D.DrawLine(spriteBatch, points[i], points[i + 1], Colour);
			}

			//Draw 
			foreach (var l in Lines)
			{
				var y = calculateY(l.point);
				if (y > this.Position.Y && y < this.Area.Bottom)
					C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(this.Position.X, y) 
						, new Vector2(this.Position.X + this.Area.Width, y), l.colour);
			}
			//Draw Axies
			C3.XNA.Primitives2D.DrawLine(spriteBatch, Position, Position + new Vector2(0, this.Area.Height), Colour);
			C3.XNA.Primitives2D.DrawLine(spriteBatch, Position + new Vector2(this.Area.Width, this.Area.Height) 
				, Position + new Vector2(0, this.Area.Height), Colour);

		}

		public void Update(GameTime gameTime)
		{
			while (Data.Count > MaxDataPoints)
				Data.RemoveAt(0);

			points = new Vector2[Data.Count];
			intervals = (double)this.Area.Width / (double)MaxDataPoints;

			min = (ZeroIsMinimum ? 0 : double.MaxValue);

			if (this.MaxMode == MaxType.Auto)
				max = double.MinValue;

			foreach (var pt in Data)
			{
				if (pt > max)
					max = pt;

				if (!ZeroIsMinimum && pt < min)
					min = pt;
			}

			for (int i = Data.Count - 1; i >= 0; i--)
				points[i] = new Vector2((float)(this.Position.X + i * intervals), calculateY(Data[i]));

		}

		private float calculateY(double pt)
		{
			return (float)(this.Position.Y - (pt - min) / (max - min) * (double)this.Area.Height + this.Area.Height);
		}

		public void Clear()
		{
			Data.Clear();
		}
	}
}

