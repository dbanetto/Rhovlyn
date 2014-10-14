using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Win32;
using System.Runtime.Remoting.Channels;


namespace Rhovlyn.Engine.Util
{
	public class Camera
	{
		private Rectangle bounds;
		private Vector2 position;

		public Camera(Vector2 position, Rectangle bounds)
		{
			this.bounds = bounds;
			Position = position;
		}

		public void UpdateBounds(Rectangle bounds)
		{
			this.bounds = bounds;
			this.bounds.X = (int)position.X;
			this.bounds.Y = (int)position.Y;
		}

		public Rectangle Bounds {
			get { return bounds; }
		}

		public Vector2 Position {
			get { return position; }
			set {
				position = value;
				bounds.X = (int)value.X;
				bounds.Y = (int)value.Y;
			}
		}
	}
}

