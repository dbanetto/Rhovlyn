using SharpDL.Graphics;


namespace Rhovlyn.Engine.Util
{
	public class Camera
	{
		private Rectangle bounds;
		private Vector position;

		public Camera(Vector position, Rectangle bounds)
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

		public Vector Position {
			get { return position; }
			set {
				position = value;
				bounds.X = (int)value.X;
				bounds.Y = (int)value.Y;
			}
		}
	}
}

