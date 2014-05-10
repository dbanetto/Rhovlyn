using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO;

namespace Rhovlyn.Engine.Util
{
    public class TextureMananger
    {
		private GraphicsDevice graphics;
		private Dictionary< string , Texture2D > textures;

		public TextureMananger(GraphicsDevice graphics )
        {
			this.graphics = graphics;
			textures = new Dictionary<string, Texture2D>();
        }

		public Texture2D this[string index]
		{
			get { return textures[index]; }
			set { textures[index] = value; }
		}

		public bool Add (string name , string path)
		{
			if (!Exists(name))
			{
				var tex = Texture2D.FromStream( this.graphics , new System.IO.FileStream( path , FileMode.Open ));
				textures.Add( name , tex );
				return true;
			}
			return false;
		}

		public bool Exists (string name)
		{
			return textures.ContainsKey(name);
		}

    }
}

