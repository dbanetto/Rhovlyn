using System;
using System.Collections.Generic;
using Rhovlyn.Engine.Maps;

namespace Rhovlyn.Engine.Managers
{
	public class MapManager
    {
		Dictionary< string , Map > maps;

        public MapManager()
        {
			maps = new Dictionary<string, Map>();
        }

		#region Management
		public string Current { get; set; }
		public Map CurrentMap { get {return maps[Current]; } }

		public Map Get(string name)
		{
			if (Exists(name))
			{
				return maps[name];
			}
			return null;
		} 

		public bool Add (string name , string path , TextureMananger textures )
		{
			if (!Exists(name))
			{
				maps.Add(name, new Map( path , textures ));
				if (maps.Count == 1)
					Current = name;
				return true;
			}
			return false;
		}

		public bool Add (string name , Map Map )
		{
			if (!Exists(name))
			{
				maps.Add(name, Map);
				if (maps.Count == 1)
					Current = name;
				return true;
			}
			return false;
		}

		public void Remove (string name)
		{
			if (Exists(name))
			{
				this.maps.Remove(name);
			}
		}

		public Map this[string name]
		{
			get {return Get(name);}
		}

		public bool Exists ( string name)
		{
			return this.maps.ContainsKey(name);
		}
		#endregion
    }
}

