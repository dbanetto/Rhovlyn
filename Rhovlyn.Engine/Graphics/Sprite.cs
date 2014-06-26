#region usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;
using System.Collections.Generic;
using Rhovlyn.Engine.Controller;
using C3.XNA;


#endregion

namespace Rhovlyn.Engine.Graphics
{
	public class Sprite : Graphics.IDrawable
	{
		#region Feilds

		protected Vector2 position;
		protected Rectangle area;
		protected float rotation;
		protected int frameindex;
		protected IController controller;

		#endregion

		#region Constructor

		public Sprite(Vector2 position, SpriteMap spritemap)
		{
			this.Position = position;
			this.SpriteMap = spritemap;
			this.area.Width = SpriteMap.Frames[0].Width;
			this.area.Height = SpriteMap.Frames[0].Height;
			this.frameindex = 0;

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
			{
				spriteBatch.Draw(SpriteMap.Texture, Vector2.Subtract(this.position, camera.Position),
					SpriteMap.Frames[Frameindex], Colour, Rotation, Origin, Scale, Effect, Depth);

				#if RENDER_SPRITE_AREA
				Primitives2D.DrawRectangle(spriteBatch, new Rectangle((int)Vector2.Subtract(this.position, camera.Position).X, (int)Vector2.Subtract(this.position, camera.Position).Y
					, SpriteMap.Frames[Frameindex].Width, SpriteMap.Frames[Frameindex].Height), Color.Blue);
				#endif
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (HasController)
				this.controller.Update(gameTime);
		}

		#endregion

		#region Properties

		public SpriteMap SpriteMap { get; private set; }

		public int Frameindex
		{
			get { return frameindex; }
			set
			{
				this.frameindex = value;
				this.area.Width = SpriteMap.Frames[value].Width;
				this.area.Height = SpriteMap.Frames[value].Height;
			} 
		}

		public float Depth { get; set; }

		public Vector2 Origin { get; set; }

		public Vector2 Scale { get; set; }

		public SpriteEffects Effect { get; set; }

		public Color Colour { get ; set; }

		public Vector2 Position
		{ 
			get { return this.position; } 
			set
			{ 
				this.position = value;
				this.area.X = (int)value.X;
				this.area.Y = (int)value.Y;
			} 
		}

		public Rectangle Area
		{
			get
			{
				return this.area;
			}
		}

		public bool HasController { get; private set; }

		public IController Controller
		{
			get { return this.controller; } 
			set
			{ 
				this.controller = value;
				if (this.controller != null)
					HasController = true;
			}
		}

		public float Rotation
		{
			get { return this.rotation; }
			set { this.rotation = value % MathHelper.TwoPi; }
		}

		public string TextureName
		{
			get { return this.SpriteMap.TextureName; }
		}

		#endregion
	}
}

