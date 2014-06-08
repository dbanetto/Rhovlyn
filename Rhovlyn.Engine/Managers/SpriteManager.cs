using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Managers;
using System.IO;
using System;

namespace Rhovlyn.Engine.Managers
{
	public class SpriteManager
	{
		private Dictionary< string , Sprite > sprites;
		public ContentManager Content { get; private set; }

		public SpriteManager(ContentManager content)
		{
			sprites = new Dictionary< string , Sprite >();
			Content = content;
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
		public bool Load(Stream stream , TextureMananger textures)
		{
			using ( var reader =  new StreamReader( stream ) )
			{
				try {
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						if (line.IndexOf("#") != -1)
							line = line.Substring(0, line.IndexOf("#") );
						line = line.Trim();
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
							// Sprite Example
							// x,y,name,Texture[,TextureIndex]
							//TODO : Load different Sprite types
							var args = line.Split(',');

							int x = int.Parse(args[0]);
							int y = int.Parse(args[1]);
							var name = args[2];
							var tex = args[3];

							if ( args.Length > 3  )
							{
								var obj = new Sprite( new Vector2( x , y) , textures[tex] );
							  	int index = int.Parse(args[4]);
								obj.Frameindex = index;

								sprites.Add(  name , obj );
							} else
							{
								sprites.Add( name , new Sprite( new Vector2( x , y) , textures[tex] ) );
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

        public bool Load(string path)
        {
			return this.Load(new FileStream(path , FileMode.Open), this.Content.Textures);
        }

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