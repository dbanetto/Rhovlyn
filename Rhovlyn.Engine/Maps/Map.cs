#region Usings
using System;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;
using System.Net.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Net.Mime;
using Rhovlyn.Engine.Managers;


#endregion
namespace Rhovlyn.Engine.Maps
{
	public class Map : Graphics.IDrawable
    {
		private Dictionary< Point , MapObject > mapobjects;
		public SpriteManager Sprites { get; private set; }
		public static readonly int TILE_WIDTH = 64;
		public static readonly int TILE_HIGHT = 64;

		#region Contructor
		public Map(string path , TextureMananger textures)
		{
			mapobjects = new Dictionary<Point, MapObject>();
			Sprites = new SpriteManager();
			this.Load(path , textures);
		}
		public Map() {}
		#endregion

		#region Methods
		public bool Load(Stream stream , TextureMananger textures)
		{
			using ( var reader =  new StreamReader( stream ) )
			{
				mapobjects.Clear();
				try {
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						if (line.IndexOf("#") != -1)
							line = line.Substring(0, line.IndexOf("#") );
						line.Trim();
						if (string.IsNullOrEmpty(line))
							continue;

						//Loads Texture File
						if (line.StartsWith("@"))
						{
							line = line.Substring(1);
							if (line.StartsWith("include:"))
							{
								var obj = line.Substring("include:".Length);
								textures.Load( IO.Path.ResolvePath( obj ) );
							}
						} else{
							var args = line.Split(',');

							int x = int.Parse(args[0]);
							int y = int.Parse(args[1]);
							var tex = args[2];

							if ( args.Length > 3  )
							{
								var obj = new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HIGHT ) , textures[tex] );

								int index = int.Parse(args[3]);
								obj.Frameindex = index;

								mapobjects.Add( new Point(x , y) , obj );
							} else
							{
								mapobjects.Add( new Point(x , y) , new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HIGHT ) , textures[tex] ) );
							} 

						}

					}
				} catch (Exception ex) {
					Console.WriteLine(ex);
					return false;
				}
			}
			return true;
		}

		public bool Load (string path , TextureMananger textures)
		{
			using ( var f = new FileStream ( path , FileMode.Open) )
			{
				return this.Load(f , textures);
			}
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			int tile_x = (camera.Bounds.X / TILE_WIDTH);
			int tile_y = (camera.Bounds.Y / TILE_HIGHT);
			int tile_w = (camera.Bounds.Width / TILE_WIDTH) + 1;
			int tile_h = (camera.Bounds.Height / TILE_HIGHT) + 1;
			//Check which method is cheaper
			if (mapobjects.Count < (tile_w * tile_h))
			{
				foreach (var obj in mapobjects.Values)
				{
					obj.Draw(gameTime, spriteBatch, camera);
				}
			} else {
				for (int x = tile_x; x < tile_w + tile_x; x++) {
					for (int y = tile_y; y < tile_h + tile_y; y++) {
						if (this.mapobjects.ContainsKey(new Point(x, y))) {
							mapobjects[new Point(x, y)].Draw(gameTime, spriteBatch, camera);
						}
					}
				}
			}
		}

		public void Update (GameTime gameTime)
		{
			foreach (var obj in mapobjects.Values)
			{
				obj.Update(gameTime);
			}
		}
		#endregion
    }
}

