using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Rhovlyn.Engine.Input
{
	public class KeyCondition
	{
		public List<Keys> Keys { get; set; }
		public int Modifer {get; set; }
	}

	public class KeyBoardProvider : IInputProvider
    {
		public string SettingsPostfix { get; private set; }

		private IO.Settings settings;
		private Dictionary<string ,  KeyCondition > keys;

        public KeyBoardProvider()
        {
			SettingsPostfix = "keys";
        }

		public bool Load ( string path )
		{
			try {
				settings = new IO.Settings( path );
				ParseSettings ();
				return true;
			} catch {
				return false;
			}
		}

		public void Unload ()
		{

		}
		public bool Exists( string name )
		{
			return keys.ContainsKey(name);
		}

		public bool GetState ( string name )
		{
			return false;
		}

		public bool SetCondition<T> (string name , T condition ) 
		{
			if (typeof(T) != typeof(KeyCondition))
				throw new InvalidDataException("Can only pass KeyCondition to KeyBoardProvider's set");

			if (Exists(name))
			{
				this.keys[name] = condition;
				return true;
			}
			return false;
			
		}

		public bool Add<T> (string name , T condition) 
		{
			if (typeof(T) != typeof(KeyCondition))
				throw new InvalidDataException("Can only pass KeyCondition to KeyBoardProvider's add");

			if (!Exists(name))
			{
				this.keys[name] = condition;
				return true;
			} 
			return false;

		}

		public void ParseSettings ()
		{
			//Go through ALL of the headers
			foreach (var header in settings.Headers)
			{
				//Check all thier values
				foreach (var key in settings[header].Keys )
				{
					//if they have THIS Providers SettingsPostfix then deal with it
					if (key.EndsWith(SettingsPostfix))
					{
						//Remove SettingsPostfix from the key
						//On a unsuccessful Parse, stop
						ParseSetting(header + "." + key.Substring(0, key.Length - SettingsPostfix.Length - 1),
							settings[header][key]);						
					}
				}
			}
		}

		public bool ParseSetting( string name , string value )
		{
			return false;
		}

    }
}

