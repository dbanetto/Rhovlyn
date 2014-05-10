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
	public class Sprite
	{
		#region Feilds
		protected Vector2 postion;
		protected Rectangle area;
		protected Texture2D texture;
		#endregion

		#region Constructor
		public Sprite(Vector2 positon , Texture2D texture)
		{
			this.postion = positon;
			this.texture = texture;
		}
		#endregion

		#region Methods
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			spriteBatch.Draw( this.texture , Vector2.Subtract(this.postion , camera.Position)  , Color.White  );
		}

		public virtual void Update(GameTime gameTime)
		{

		}
		#endregion

		#region Properties
		public Texture2D Texture { get { return this.texture; } }

		public Vector2 Position { 
			get { return this.postion; } 
			set { this.postion = value;
				this.area.X = (int)value.X;
				this.area.Y = (int)value.Y; } 
		}

		public Rectangle Area {
			get { return this.area; }
			set {
				this.area = value;
				this.area.X = (int)this.postion.X;
				this.area.Y = (int)this.postion.Y;
			}
		}
		#endregion 
	}
}

