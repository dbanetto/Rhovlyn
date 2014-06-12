#region Usings
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Graphics;
#endregion
namespace Rhovlyn.Engine.Maps
{
	public class Map 
	{
		private Dictionary< Point , MapObject > mapobjects;
		public ContentManager Content { get; private set; }
		public static readonly int TILE_WIDTH = 64;
		public static readonly int TILE_HEIGHT = 64;
		public Color Background { get; set; }

		public Rectangle MapArea {get { return mapArea; }}
		Rectangle mapArea;

		private AreaMap<MapObject> areamap;
		private MapObject[] last_tiles;
		private Rectangle last_camera= Rectangle.Empty;

		private bool ReqestAreaMapUpdate = false;

		#region Constructor
		public Map( string path , ContentManager content )
		{
			areamap = new AreaMap<MapObject>();
			mapobjects = new Dictionary<Point, MapObject>();
			last_tiles = new MapObject[0];
			Content = content;
			this.Load(path);
		}

		public Map( ContentManager content ) 
		{
			Content = content;
			areamap = new AreaMap<MapObject>();
			last_tiles = new MapObject[0];
			mapobjects = new Dictionary<Point, MapObject>();
		}

		~Map()
		{
			this.mapobjects.Clear();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Load map from a stream, does not override current Map
		/// </summary>
		/// <param name="stream">Stream.</param>
		public bool Load( Stream stream )
		{
			mapArea = Rectangle.Empty;
			using ( var reader =  new StreamReader( stream ) )
			{
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
							// Load a sprite list
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


							if (line.StartsWith("background:"))
							{
								var obj = line.Substring("background:".Length);
								int r = 0, g = 0, b = 0;
								var rgb = obj.Split(',');

								//Parse a RGB comma sperated string
								if ( rgb.Length != 3)
									throw new InvalidDataException("Expected a comma sperated RGB value");
								r = byte.Parse( rgb[0] );
								g = byte.Parse( rgb[1] );
								b = byte.Parse( rgb[2] );

								Background = new Color( r , g , b );
							}
						} else if ( line.StartsWith("<") && line.EndsWith(">") )
						{
							continue;
						} else {
							//Load a Map object
							// Example x,y,Texture Name[,Texture Index]
							// Eg. 0,0,wood
							// Creates an object at 0,0 with texture "wood"
							var args = line.Split(',');

							int x = int.Parse(args[0]);
							int y = int.Parse(args[1]);
							var tex = args[2];

							//Check if Texture index is defined
							if ( args.Length > 3  )
							{
								var obj = new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HEIGHT ) , Content.Textures[tex] );

								int index = int.Parse(args[3]);
								obj.Frameindex = index;

								this.Add( new Point(x , y) , obj );
							} else
							{
								this.Add( new Point(x , y) , new MapObject( new Vector2( x*TILE_WIDTH , y*TILE_HEIGHT ) , Content.Textures[tex] ) );
							} 
						}

					}
				} catch (IOException ex) {
					Console.WriteLine(ex);
					return false;
				}
			}

			this.UpdateAreaMap(false);
			return true;
		}

		public bool Save( string filepath , int blocksize = 16 )
		{
			List<string> required = new List<string>();

			this.UpdateAreaMap(true);
			int rect_width = blocksize * TILE_WIDTH;
			int rect_height = blocksize * TILE_HEIGHT;

			int counter = 0;
			using (StreamWriter writer = new StreamWriter(new FileStream(filepath , FileMode.Create)))
			{
				writer.WriteLine(String.Format("@background:{0},{1},{2}",
					new Object[] {this.Background.R , this.Background.G , this.Background.B }));

				for (int x = this.MapArea.Left; x < this.MapArea.Right; x += rect_width)
				{
					for (int y = this.MapArea.Top; y < this.MapArea.Bottom; y += rect_height)
					{

						var area = new Rectangle(x, y , rect_width , rect_height);
						var objs = this.areamap.Get(area);

						if (objs.Length == 0)
							continue;

						writer.WriteLine(String.Format("<{0},{1},{2},{3}>" ,
							new Object[] { area.X / TILE_WIDTH, area.Y /TILE_HEIGHT , blocksize , blocksize }));
						foreach ( var obj in  objs )
						{
							// x,y,Texture Name[,Texture Index]
							writer.WriteLine(String.Format("{0},{1},{2},{3}" ,
								new Object[] { obj.Position.X / TILE_WIDTH , obj.Position.Y / TILE_HEIGHT , obj.SpriteMap.Texture.Name , obj.Frameindex  }));

							counter++;

							if (!required.Contains(obj.TextureName))
								required.Add(obj.TextureName);
						}
					}
				}
				writer.Flush();
			}
			Console.WriteLine("Wrote " + counter + " out of " + this.mapobjects.Count);
			return true;
		}

		/// <summary>
		/// Add the given Map Object to the map
		/// </summary>
		/// <param name="pt">Point.</param>
		/// <param name="obj">Object.</param>
		public bool Add (Point pt , MapObject obj)
		{
			if (mapobjects.ContainsKey(pt))
				return false;

			this.mapobjects.Add(pt, obj);

			if (!this.MapArea.Contains(obj.Area))
			{
				ReqestAreaMapUpdate = true;
			}

			areamap.Add(obj);
			return true;
		}

		/// <summary>
		/// Load a map from a file
		/// </summary>
		/// <remark>
		/// Does clear current map.
		/// </remark>
		/// <param name="path">Path.</param>
		public bool Load (string path )
		{
			return this.Load( IO.Path.ResolvePath(path));
		}

		/// <summary>
		/// Unload all Map Tiles in an area
		/// </summary>
		/// <param name="area">Area.</param>
		public void Unload(Rectangle area)
		{
			foreach (var t in PointsUnderArea(area))
			{
				this.mapobjects.Remove(t);
			}
			ReqestAreaMapUpdate = true;
		}

		/// <summary>
		/// Draw the Map
		/// </summary>
		/// <param name="gameTime">Game time</param>
		/// <param name="spriteBatch">Sprite batch to draw to</param>
		/// <param name="camera">Camera of the map</param>
		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			foreach (var obj in last_tiles)
			{
				obj.Draw(gameTime, spriteBatch, camera);
			}
			#if DEBUG
			areamap.Draw(spriteBatch, camera);
			#endif	
		}

		/// <summary>
		/// Updates the bounds of the map
		/// </summary>
		public void UpdateAreaOfMap()
		{
			mapArea = Rectangle.Empty;
			foreach (var obj in mapobjects.Values)
			{
				mapArea = Rectangle.Union(MapArea, obj.Area);
			}
		}

		/// <summary>
		/// Updates the Area Map object.
		/// </summary>
		/// <remark>
		/// Should be used after editting the Map objects
		/// </remark>
		public void UpdateAreaMap(bool UpdateAreaOfMap = true)
		{
			if (UpdateAreaOfMap)
				this.UpdateAreaOfMap();

			areamap = new AreaMap<MapObject>(this.MapArea);
			foreach (var obj in mapobjects.Values)
			{
				if (!areamap.Add(obj))
				{
					throw new Exception("Didn't add object");
				}
			}
			if (this.areamap.Count != mapobjects.Count)
			{
				throw new Exception("Not all Map Objects are contained in AreaMap and MapObjects");
			}
		}

		public void Update (GameTime gameTime)
		{
			if (last_camera != Content.Camera.Bounds)
			{
				last_tiles = areamap.Get(Content.Camera.Bounds);
				last_camera = Content.Camera.Bounds;
			}

			foreach (var obj in last_tiles)
			{
				obj.Update(gameTime);
			}

			if (ReqestAreaMapUpdate)
			{
				this.UpdateAreaMap(true);
				ReqestAreaMapUpdate = false;
			}
		}

		/// <summary>
		/// Get all Map Objects in the Area
		/// </summary>
		/// <returns>The Map Objects in the area</returns>
		/// <param name="area">Area</param>
		public MapObject[] TilesInArea (Rectangle area)
		{
			List<MapObject> output = new List<MapObject>();
			foreach (var pt in PointsUnderArea(area))
			{
				if (mapobjects.ContainsKey(pt))
				{
					output.Add(mapobjects[pt]);
				}
			}
			return output.ToArray();
		}

		/// <summary>
		/// Determines if the given area in completely on the map
		/// </summary>
		/// <returns><c>true</c> if area give is completely on the map ; otherwise, <c>false</c>.</returns>
		/// <param name="area">Area to check</param>
		public bool IsOnMap( Rectangle area )
		{
			foreach (var pt in PointsUnderArea(area))
			{
				if (!mapobjects.ContainsKey(pt))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Get all of the Map Points in the given area
		/// </summary>
		/// <returns>The Map Points in the given area</returns>
		/// <param name="area">Area</param>
		public static Point[] PointsUnderArea(Rectangle area)
		{
			List<Point> output = new List<Point>();
			//Get the X,Y,W,H in terms of Map Objects
			double ax = (double)area.X / (double)TILE_WIDTH;
			double ay = (double)area.Y / (double)TILE_HEIGHT;
			double aw = ((double)area.Width / (double)TILE_WIDTH) + ax;
			double ah = ((double)area.Height / (double)TILE_HEIGHT) + ay;

			for (double x = ax; x < aw; x++ )
			{
				for (double y = ay; y < ah; y++ )
				{
					//Check both rounded and floored values
					//Both are check because of a bug occurs when only 1 is checked
					var p_xs = new int[] { (int)Math.Round(x) , (int)Math.Floor(x) };
					var p_ys = new int[] { (int)Math.Round(y), (int)Math.Floor(y) };

					//Check all the values for a fail
					foreach (var p_x in p_xs){
						foreach (var p_y in p_ys){
							var pt = new Point( p_x , p_y );
							if (!output.Contains(pt))
								output.Add(pt);
						}
					}
				}
			}
			return output.ToArray();
		}
		#endregion
	}
}

