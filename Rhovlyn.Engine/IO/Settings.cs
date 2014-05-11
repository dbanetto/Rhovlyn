using System;
using System.IO;
using System.Collections.Generic;
using MonoGame.Framework.MonoLive;

namespace Rhovlyn.Engine.IO
{
    public class Settings
    {
		// < Header , Contents >
		private Dictionary<string , Dictionary< string , string >  > settings;
		public Settings(string path)
        {
			this.Load(path);
        }

		public bool Load (string path)
		{
			settings = new Dictionary<string, Dictionary< string , string > >();
			using (var reader = new StreamReader(new FileStream( path  , FileMode.Open )))
			{
				settings.Add( "" , new Dictionary< string , string >() );
				var current = settings[""];
				var header = "";
				while (!reader.EndOfStream)
				{
					try {
						var line = reader.ReadLine();
						if (line.IndexOf(';') != -1)
							line = line.Substring(0, line.IndexOf(';')); //removes all comments
						line = line.Trim();
						if (string.IsNullOrEmpty(line))
							continue;

						//Header
						if ( line.StartsWith("[") && line.EndsWith("]") )
						{
							header = line.Substring(1, line.Length - 2);
							if (!Exists(header))
								settings.Add( header , new Dictionary< string , string >() ); //Supports Partial definitaion
							current = settings[header];
						} else if ( line.IndexOf('=') != -1 )
						{
							//Split the line at the ='s
							var key = line.Substring(0, line.IndexOf('=') ).Trim();
							if (!Exists( header , key))
								current.Add( key , line.Substring(line.IndexOf('=') + 1 ).Trim()   );
							else
								Console.WriteLine("WARNING Double definition of " + header + "::" + key + "\nIgnoring new definition");
						}
					} catch (Exception ex)
					{
						Console.WriteLine("Error while reading " + path + " for settings");
						Console.WriteLine(ex);
					}
				}
			}
			return true;
		}

		public bool Exists (string header)
		{
			return this.settings.ContainsKey(header);
		}
		public bool Exists (string header , string key)
		{
			if (Exists(header))
			{
				return this.settings[header].ContainsKey(key);
			}
			return false;
		}

		public Dictionary< string , string > this[string header]
		{
			get { return this.settings[header]; }
		}

		public bool Get (string header , string key , ref string result)
		{
			if (Exists(header, key))
			{
				result = this.settings[header][key];
				return true;
			}
			return false;
		}

		public bool GetBool (string header , string key , ref bool result)
		{
			string val = "";
			if (Get(header, key , ref val))
			{
				result = val.ToLower() == "true";
				return ( result || val.ToLower() == "false"); //Check if it was a vaild entry
			}
			return false;
		}

		public bool GetInt (string header , string key , ref int result)
		{
			string val = "";
			if (Get(header, key , ref val))
			{
				return int.TryParse( val , out result );
			}
			return false;
		}

    }
}

