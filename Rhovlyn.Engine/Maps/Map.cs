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
		public ContentManager Content { get; private set; }
		public static readonly int TILE_WIDTH = 64;
		public static readonly int TILE_HIGHT = 64;

		#region Constructor
		public Map( string path , ContentManager content )
		{
			mapobjects = new Dictionary<Point, MapObject>();
			Content = content;
			this.Load(path);
		}

		public Map( ContentManager content ) 
		{
			Content = content;
			mapobjects = new Dictionary<Point, MapObject>();
		}

		~Map()
		{
			this.mapobjects.Clear();
		}
		#endregion

		#region Methods
		public bool Load( Stream stream )
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
							//Load a Texture list
							if (line.StartsWith("include:"))
							{
								var obj = line.Substring("include:".Length);
								Content.Textures.Load( IO.Path.ResolvePath( obj ) );
							}

							if (line.StartsWith("sprites:"))
							{
								var obj = line.Substring("sprites:".Length);
								Content.Sprites.Load( IO.Path.ResolvePath( obj ) , Content.Textures );
							}

							//Hard check for a texture
							if (line.StartsWith("require:"))
							{
								var obj = line.Substring("require:".Length);
								foreach (var tex in obj.Split(','))
								{
									if (!Content.Textures.Exists(tex) )
									{
										//TODO : Make a exception class for this
										throw new Exception("Failed to meet the require texture " + tex);
									}
								}
							}
						} else{
							var args = line.Split(',');

							int x = int.Parse(args[0]);
							int y = int.Parse(args[1]);
							var tex = args[2];

							if ( args.Length > 3  )
							{
								var obj = new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HIGHT ) , Content.Textures[tex] );

								int index = int.Parse(args[3]);
								obj.Frameindex = index;

								mapobjects.Add( new Point(x , y) , obj );
							} else
							{
								mapobjects.Add( new Point(x , y) , new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HIGHT ) , Content.Textures[tex] ) );
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

		public bool Load (string path )
		{
			return this.Load( IO.Path.ResolvePath(path));
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			//Get a "Rectangle" of all the possible sprites 
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

