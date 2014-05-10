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


#endregion
namespace Rhovlyn.Engine.Maps
{
	public class Map : Graphics.IDrawable
    {
		private Dictionary< Point , MapObject > mapobjects;

		#region Contructor
		public Map(string path , TextureMananger textures)
		{
			mapobjects = new Dictionary<Point, MapObject>();
			this.Load(path , textures);
		}
		public Map() {}
		#endregion

		#region Methods
		public bool Load (string path , TextureMananger textures)
		{
			using ( var reader =  new StreamReader( new FileStream ( path , FileMode.Open) ) )
			{
				mapobjects.Clear();
				try {
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						if (line.IndexOf("#") > 0)
							line = line.Substring(0, line.IndexOf("#") - 1);
						line.Trim();
						if (string.IsNullOrEmpty(line))
							continue;

						if (line.StartsWith("load>"))
						{
							var obj = line.Substring("load>".Length);
							var args = obj.Split(';');

							var tname = args[0];
							var tpath = args[1];

							textures.Add( tname , tpath );
						}

						if (line.StartsWith("tile>"))
						{
							var obj = line.Substring("tile>".Length);
							var args = obj.Split(';');

							int x = int.Parse(args[0]);
							int y = int.Parse(args[1]);
							var tex = args[2];

							mapobjects.Add( new Point(x , y) , new MapObject( new Vector2( x , y ) , textures[tex] ) );
						}

					}
				} catch (Exception ex) {
					Console.Beep();
					Console.WriteLine(ex);
				}
			}
			return true;
		}

		public void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera)
		{
			foreach (var obj in mapobjects.Values)
			{
				obj.Draw(gameTime , spriteBatch , camera);
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

