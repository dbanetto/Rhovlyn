using System;
using System.Collections.Generic;
using Rhovlyn.Engine.Maps;

namespace Rhovlyn.Engine.Managers
{
	public class MapManager
	{
		Dictionary< string , Map > maps;

		public TextureManager Textures { get; private set; }

		public SpriteManager Sprites { get; private set; }

		public MapManager(TextureManager textures, SpriteManager sprites)
		{
			maps = new Dictionary<string, Map>();
			Textures = textures;
			Sprites = sprites;
			Current = "";
		}

		#region Management

		public string Current { get; set; }

		public Map CurrentMap { get { return (this.Exists(Current) ? this[Current] : null); } }

		public Map Get(string name)
		{
			return Exists(name) ? maps[name] : null;
		}

		public bool Add(string name, string path)
		{
			if (!Exists(name))
			{
				maps.Add(name, new PartialMap(path, Textures));
				if (maps.Count == 1)
					Current = name;
				return true;
			}
			return false;
		}

		public bool Add(string name, Map map)
		{
			if (!Exists(name))
			{
				maps.Add(name, map);
				if (maps.Count == 1)
					Current = name;
				return true;
			}
			return false;
		}

		public void Remove(string name)
		{
			if (Exists(name))
			{
				this.maps.Remove(name);
			}
		}

		public Map this [string name]
		{
			get { return Get(name); }
		}

		public bool Exists(string name)
		{
			return this.maps.ContainsKey(name);
		}

		#endregion

	}
}

