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


		public Camera(Vector2 position, Rectangle bounds )
		{
			this.bounds = bounds;
			this.Position = position;
		}


		public void UpdateBounds (Rectangle bounds)
		{
			this.bounds = bounds;
		}

		public Rectangle Bounds
		{
			get{ return this.bounds; }
		}

		public Vector2 Position
		{
			get{ return this.position; }
			set
			{
				this.position = value;
				this.bounds.X = (int)value.X;
				this.bounds.Y = (int)value.Y;
			}
		}
	}
}

