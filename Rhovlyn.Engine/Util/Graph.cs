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
		private double min;
		private double intervals;

		private Vector2[] points;

		public List<Line> Lines { get; set; }

		public Vector2 Position { get; set; }

		public Rectangle Area { get; set; }

		public Graph(Vector2 position, int width, int height)
		{
			Data = new List<double>();
			Lines = new List<Line>();
			ZeroIsMinimum = false;
			MaxDataPoints = 100;
			Position = position;
			Area = new Rectangle((int)Position.X, (int)Position.Y, width, height);
			MaxMode = MaxType.Auto;
			Colour = Colour;
		}

		public Graph(Vector2 position, int width, int height, bool zeroIsMinimum, int maxDataPoints, MaxType type, Color colour)
		{
			Data = new List<double>();
			Lines = new List<Line>();
			ZeroIsMinimum = zeroIsMinimum;
			MaxDataPoints = maxDataPoints;
			Position = position;
			Area = new Rectangle((int)Position.X, (int)Position.Y, width, height);
			MaxMode = type;
			Colour = colour;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			if (points == null)
				Update(gameTime);

			for (int i = 0; i < points.Length - 1; i++) {
				Primitives2D.DrawLine(spriteBatch, points[i], points[i + 1], Colour);
			}

			//Draw 
			foreach (var l in Lines) {
				var y = CalculateY(l.point);
				if (y > Position.Y && y < Area.Bottom)
					Primitives2D.DrawLine(spriteBatch, new Vector2(Position.X, y) 
						, new Vector2(Position.X + Area.Width, y), l.colour);
			}
			//Draw Axies
			Primitives2D.DrawLine(spriteBatch, Position, Position + new Vector2(0, Area.Height), Colour);
			Primitives2D.DrawLine(spriteBatch, Position + new Vector2(Area.Width, Area.Height) 
				, Position + new Vector2(0, Area.Height), Colour);

		}

		public void Update(GameTime gameTime)
		{
			while (Data.Count > MaxDataPoints)
				Data.RemoveAt(0);

			points = new Vector2[Data.Count];
			intervals = (double)Area.Width / (double)MaxDataPoints;

			min = (ZeroIsMinimum ? 0 : double.MaxValue);

			if (MaxMode == MaxType.Auto)
				max = double.MinValue;

			foreach (var pt in Data) {
				if (pt > max)
					max = pt;

				if (!ZeroIsMinimum && pt < min)
					min = pt;
			}

			for (int i = Data.Count - 1; i >= 0; i--)
				points[i] = new Vector2((float)(Position.X + i * intervals), CalculateY(Data[i]));

		}

		private float CalculateY(double pt)
		{
			return (float)(Position.Y - (pt - min) / (max - min) * (double)Area.Height + Area.Height);
		}

		public void Clear()
		{
			Data.Clear();
		}
	}
}

