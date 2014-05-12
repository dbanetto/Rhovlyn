using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Managers
{
	public class SpriteManager : Graphics.IDrawable
	{
		private Dictionary< string , Sprite > sprites;

		public SpriteManager()
		{
			sprites = new Dictionary< string , Sprite >();
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			foreach (Sprite sp in sprites.Values)
				sp.Draw(gameTime, spriteBatch, camera);
		}

		public void Update (GameTime gameTime)
		{
			foreach (Sprite sp in sprites.Values)
				sp.Update(gameTime);
		}

		#region Management
		public Sprite Get(string name)
		{
			if (Exists(name))
			{
				return sprites[name];
			}
			return null;
		} 

		public bool Add (string name , Sprite sprite )
		{
			if (!Exists(name))
			{
				sprites.Add(name, sprite);
				return true;
			}
			return false;
		}

		public Sprite this[string name]
		{
			get {return Get(name);}
		}

		public bool Exists ( string name)
		{
			return this.sprites.ContainsKey(name);
		}
		#endregion
	}
}

