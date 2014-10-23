using System;
using System.Collections.Generic;
using SharpDL.Graphics;
using SharpDL;


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

		private Vector[] points;

		public List<Line> Lines { get; set; }

		public Vector Position { get; set; }

		public Rectangle Area { get; set; }

		public Graph(Vector position, int width, int height)
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

		public Graph(Vector position, int width, int height, bool zeroIsMinimum, int maxDataPoints, MaxType type, Color colour)
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

		public void Draw(GameTime gameTime, Renderer renderer, Camera camera)
		{
			if (points == null)
				Update(gameTime);

			for (int i = 0; i < points.Length - 1; i++) {
				renderer.RenderLine(points[i], points[i + 1], Colour);
			}

			//Draw 
			foreach (var l in Lines) {
				var y = CalculateY(l.point);
				if (y > Position.Y && y < Area.Bottom)
					renderer.RenderLine(new Vector(Position.X, y), new Vector(Position.X + Area.Width, y), l.colour);
			}
			//Draw Axies
			renderer.RenderLine(Position, Position + new Vector(0, Area.Height), Colour);
			renderer.RenderLine(Position + new Vector(Area.Width, Area.Height) 
				, Position + new Vector(0, Area.Height), Colour);

		}

		public void Update(GameTime gameTime)
		{
			while (Data.Count > MaxDataPoints)
				Data.RemoveAt(0);

			points = new Vector[Data.Count];
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
				points[i] = new Vector((float)(Position.X + i * intervals), CalculateY(Data[i]));

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

