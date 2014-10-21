#region usings
using SharpDL.Graphics;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Controller;
using SDL2;
using SharpDL;
using System;

#endregion

namespace Rhovlyn.Engine.Graphics
{
	public class Sprite : Graphics.IDrawable
	{
		#region Feilds

		protected Vector position;
		protected Rectangle area;
		protected float rotation;
		protected int frameindex;
		protected IController controller;

		#endregion

		#region Constructor

		public Sprite(Vector position, SpriteMap spritemap)
		{
			this.Position = position;
			this.SpriteMap = spritemap;
			this.area.Width = SpriteMap.Frames[0].Width;
			this.area.Height = SpriteMap.Frames[0].Height;
			this.frameindex = 0;

			Origin = Vector.Zero;
			Scale = Vector.One;
			Colour = new Color(255, 255, 255);
		}

		#endregion

		#region Methods

		public virtual void Draw(GameTime gameTime, Renderer renderer, Camera camera)
		{
			//Check if on screen
			if (camera.Bounds.Intersects(this.area)) {
				//FIXME
				renderer.RenderTexture(SpriteMap.Texture, Position, Area, Rotation, Origin);
				#if RENDER_SPRITE_AREA
				Primitives2D.DrawRectangle(spriteBatch, new Rectangle((int)Vector.Subtract(this.position, camera.Position).X, (int)Vector.Subtract(this.position, camera.Position).Y
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

		public int Frameindex {
			get { return frameindex; }
			set {
				frameindex = value;
				area.Width = SpriteMap.Frames[value].Width;
				area.Height = SpriteMap.Frames[value].Height;
			} 
		}

		public float Depth { get; set; }

		public Vector Origin { get; set; }

		public Vector Scale { get; set; }

		public Color Colour { get ; set; }

		public Vector Position { 
			get { return this.position; } 
			set { 
				this.position = value;
				this.area.X = (int)value.X;
				this.area.Y = (int)value.Y;
			} 
		}

		public Rectangle Area {
			get {
				return this.area;
			}
		}

		public bool HasController { get; private set; }

		public IController Controller {
			get { return this.controller; } 
			set { 
				this.controller = value;
				if (this.controller != null)
					HasController = true;
			}
		}

		public float Rotation {
			get { return this.rotation; }
			set { this.rotation = value % 6.24f; } //HACK FIXME temp value of 2pi
		}

		public string TextureName {
			get { return this.SpriteMap.Name; }
		}

		#endregion
	}
}

