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

		public bool Add (string name , Stream stream)
		{
			if (!Exists(name))
			{
				textures.Add( name , Texture2D.FromStream( this.graphics , stream) );
				return true;
			}
			return false;
		}

		public bool Add (string name , string path)
		{
			return this.Add(name, new FileStream(path, FileMode.Open));
		}

		public bool Exists (string name)
		{
			return textures.ContainsKey(name);
		}

		/// <summary>
		/// Load a local file.
		/// </summary>
		/// <param name="path">Path</param>
		public bool Load(string path)
		{
			using (var fs = new FileStream(path , FileMode.Open))
			{
				return Load(fs);
			}
		}

		/// <summary>
		/// Load the specified stream.
		/// </summary>
		/// <param name="stream">Input Stream</param>
		public bool Load (Stream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				try {
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						if (line.IndexOf("#") != -1)
							line = line.Substring(0, line.IndexOf("#"));
						line.Trim();
						if (string.IsNullOrEmpty(line))
							continue;

						//Process commands
						if (line.StartsWith("@"))
						{
							line = line.Substring(1);

							if (line.StartsWith("include:"))
							{
								var obj = line.Substring(line.IndexOf("include:"));
								this.Load( Engine.IO.Path.ResolvePath(obj) );
							}
						} else {
							//Loads Local file
							var args = line.Split(',');

							var tname = args[0];
							var tpath = args[1];

							this.Add( tname , Engine.IO.Path.ResolvePath(tpath) );
						}
					}
				}	catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}
			return true;
		}

    }
}

