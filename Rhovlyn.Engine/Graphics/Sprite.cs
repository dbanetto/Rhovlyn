#region usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;
using System.Collections.Generic;
#endregion

namespace Rhovlyn.Engine.Graphics
{
	public class Sprite : IDrawable
	{
		#region Feilds
		protected Vector2 position;
		protected Rectangle area;
		protected Texture2D texture;
		protected float rotation;
		protected List<Rectangle> frames; //Animation Frames or Sprite sheets
		#endregion

		#region Constructor
		public Sprite(Vector2 position , Texture2D texture)
		{
			this.Position = position;
			this.texture = texture;
			this.area.Width = texture.Width;
			this.area.Height = texture.Height;

			frames = new List<Rectangle>();
			frames.Add(texture.Bounds);
			Frameindex = 0;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			Colour = Color.White;
		}

		public Sprite(Vector2 position , Texture2D texture , List<Rectangle> frames)
		{
			this.Position = position;
			this.texture = texture;
			this.area.Width = texture.Width;
			this.area.Height = texture.Height;
			this.frames = frames;

			Frameindex = 0;
			Origin = Vector2.Zero;
			Scale = Vector2.One;
			Effect = SpriteEffects.None;
			Colour = Color.White;
		}
		#endregion

		#region Methods
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			//Check if on screen
			if (camera.Bounds.Intersects(this.area))
				spriteBatch.Draw(this.texture, Vector2.Subtract(this.position , camera.Position) , frames[Frameindex] ,  Colour, Rotation , Origin , Scale, Effect, Depth );
		}

		public virtual void Update(GameTime gameTime)
		{

		}
		#endregion

		#region Properties
		public Texture2D Texture { get { return this.texture; } }
		public int Frameindex { get; set; }
		public int Depth { get; set;}
		public Vector2 Origin { get; set; }
		public Vector2 Scale { get; set; }
		public SpriteEffects Effect { get; set; }
		public Color Colour { get ; set; }

		public Vector2 Position { 
			get { return this.position; } 
			set { this.position = value;
				this.area.X = (int)value.X;
				this.area.Y = (int)value.Y; } 
		}

		public Rectangle Area {
			get { return this.area; }
			set {
				this.area = value;
				this.area.X = (int)this.position.X;
				this.area.Y = (int)this.position.Y;
			}
		}

		public List<Rectangle> Frames
		{
			get {return this.frames;}
		}

		public float Rotation {
			get { return this.rotation;}
			set { this.rotation = value % MathHelper.TwoPi; }
		}

		#endregion 
	}
}

