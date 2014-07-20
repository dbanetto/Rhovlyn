using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Managers
{
	public class TextureManager
	{
		private static GraphicsDevice Graphics;
		private Dictionary< string , SpriteMap > textures;

		public TextureManager(GraphicsDevice graphics)
		{
			Graphics = graphics;
			textures = new Dictionary<string, SpriteMap>();
			if (!Parser.Exists<SpriteMap>())
				Parser.Add<SpriteMap>(ParseTexture);
		}

		#region Management

		public SpriteMap this [string index]
		{
			get { return textures[index]; }
			set { textures[index] = value; }
		}

		public bool Add(SpriteMap spritemap)
		{
			if (String.IsNullOrEmpty(spritemap.Texture.Name))
				throw new Exception("Cannot add a Texture with a empty name");

			if (!Exists(spritemap.Texture.Name))
			{
				this.textures.Add(spritemap.Texture.Name, spritemap);
				return true;
			}
			return false;
		}

		public bool Add(string name, SpriteMap spritemap)
		{
			if (!Exists(name))
			{
				spritemap.Texture.Name = name;
				this.textures.Add(name, spritemap);
				return true;
			}
			return false;
		}

		public bool Add(string name, Stream stream, List<Rectangle> frames)
		{

			if (!Exists(name))
			{
				this.textures.Add(name, new SpriteMap(Texture2D.FromStream(Graphics, stream), name, frames));
				return true;
			}
			return false;
		}

		public bool Add(string name, Stream stream)
		{
			if (!Exists(name))
			{
				this.textures.Add(name, new SpriteMap(Texture2D.FromStream(Graphics, stream), name));
				return true;
			}
			return false;
		}

		public bool Add(string name, string path)
		{
			return this.Add(name, new FileStream(path, FileMode.Open));
		}

		public bool Exists(string name)
		{
			return this.textures.ContainsKey(name);
		}

		/// <summary>
		/// Load a local file.
		/// </summary>
		/// <param name="path">Path</param>
		public bool Load(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open))
			{
				return Load(fs);
			}
		}

		/// <summary>
		/// Load the specified stream.
		/// </summary>
		/// <param name="stream">Input Stream</param>
		public bool Load(Stream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				try
				{
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
								this.Load(Engine.IO.Path.ResolvePath(obj));
							}
						}
						else
						{
							this.Add(Parser.Parse<SpriteMap>(line));
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Remove the SpriteMap with the specified name safely.
		/// </summary>
		/// <param name="name">Name of the sprite</param>
		public bool Remove(string name)
		{
			if (Exists(name))
			{
				textures[name].Texture.Dispose();
				textures.Remove(name);
			}
			return false;
		}

		public static object ParseTexture(string text)
		{
			//Loads Local file
			var args = text.Split(',');

			var tname = args[0];
			var tpath = args[1];
			var tex = Texture2D.FromStream(Graphics, Engine.IO.Path.ResolvePath(tpath));

			if (args.Length > 2)
			{
				if (args[2].Contains("@"))
				{
					var rects = new List<Rectangle>();
					// Explict Rectangle Declearations
					// x:y:w:h@....
					// 0:0:64:64@0:0:128:32
					foreach (var rect in args[2].Split( '@'))
					{
						var parms = rect.Split(':');
						if (parms.Length != 4)
							continue;

						int x = 0, y = 0, w = 0, h = 0; 
						x = int.Parse(parms[0]);
						y = int.Parse(parms[1]);
						w = int.Parse(parms[2]);
						h = int.Parse(parms[3]);

						rects.Add(new Rectangle(x, y, w, h));
					}
					return new SpriteMap(tex, tname, rects);
				} 
				// Semi-Implicit Bounds 
				// rows*colms
				// Divides up the texture into a set number of columns and rows
				// 2*2
				else if (args[2].Contains("*"))
				{
					var parms = args[2].Split('*');
					if (parms.Length != 2)
						throw new InvalidDataException(String.Format("Sprite map semi-implicit bounds format error {0}", text));
					int row = int.Parse(parms[0]);
					int col = int.Parse(parms[1]);

					return new SpriteMap(tex, tname, row, col);
				}
			}
			return new SpriteMap(tex, tname);
		}

		#endregion

	}
}

