#region usings
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Rhovlyn.Engine.Util;


#endregion
namespace Rhovlyn.Engine.Graphics
{
	public class Sprite : IDrawable
	{
		#region Feilds
		protected Vector2 position;
		protected Rectangle area;
		protected Texture2D texture;
		#endregion

		#region Constructor
		public Sprite(Vector2 position , Texture2D texture)
		{
			this.Position = position;
			this.texture = texture;
			this.area.Width = texture.Width;
			this.area.Height = texture.Height;
		}
		#endregion

		#region Methods
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			//Check if on screen
			if (camera.Bounds.Intersects(this.area))
				spriteBatch.Draw( this.texture , Vector2.Subtract(this.position , camera.Position)  , Color.White  );
		}

		public virtual void Update(GameTime gameTime)
		{

		}
		#endregion

		#region Properties
		public Texture2D Texture { get { return this.texture; } }

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
		#endregion 
	}
}

