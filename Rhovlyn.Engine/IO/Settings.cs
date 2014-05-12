using System;
using System.IO;
using System.Collections.Generic;
using MonoGame.Framework.MonoLive;

namespace Rhovlyn.Engine.IO
{
	/// <summary>
	/// <remarks>All Internal string must be in lower case</remarks>
	/// </summary>
    public class Settings
    {
		// < Header , Contents >
		private Dictionary<string , Dictionary< string , string >  > settings;

		public Settings(string path)
        {
			this.Load(path);
        }

		/// <summary>
		/// Load the specified path.
		/// </summary>
		/// <param name="path">Local path</param>
		public bool Load (string path)
		{
			using (var f = new FileStream( path  , FileMode.Open ))
			{
				return Load(f);
			}
		}

		/// <summary>
		/// Load a stream of Data assuming it is in the INI Format
		/// </summary>
		/// <param name="stream">Stream.</param>
		public bool Load (Stream stream)
		{
			settings = new Dictionary<string, Dictionary< string , string > >();
			settings.Add( "" , new Dictionary< string , string >() );
			using (var reader = new StreamReader(stream))
			{
				var current = settings[""];
				var header = "";
				while (!reader.EndOfStream)
				{
					try
					{
						//In INI Format ;'s are comments
						var line = reader.ReadLine();
						if (line.IndexOf(';') != -1)
							line = line.Substring(0, line.IndexOf(';')); //removes all comments

						//We do not take kindly to empty lines
						line = line.Trim();
						if (string.IsNullOrEmpty(line))
							continue;

						//Header
						if (line.StartsWith("[") && line.EndsWith("]"))
						{
							header = line.Substring(1, line.Length - 2).ToLower();
							if (!Exists(header))
								settings.Add(header, new Dictionary< string , string >()); //Supports Partial definitaion
							current = settings[header];
						}
						//Key and Value
						else if (line.IndexOf('=') != -1)
						{
							//Split the line at the ='s
							var key = line.Substring(0, line.IndexOf('=')).Trim().ToLower();
							if (!Exists(header, key))
								current.Add(key, line.Substring(line.IndexOf('=') + 1).Trim());
							else
								Console.WriteLine("WARNING Double definition of " + header + "::" + key + "\nIgnoring new definition");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error while reading settings");
						Console.WriteLine(ex);
					}
				}
			}
			return true;
		}

		public bool Exists (string header)
		{
			return this.settings.ContainsKey(header.ToLower());
		}
		public bool Exists (string header , string key)
		{
			if (Exists(header))
			{
				return this.settings[header.ToLower()].ContainsKey(key.ToLower());
			}
			return false;
		}
		/// <summary>
		/// Gets the <see cref="Rhovlyn.Engine.IO.Settings"/> with the specified header.
		/// </summary>
		/// <remark>Can throw expecptions</remark>
		/// <param name="header">Header.</param>
		public Dictionary< string , string > this[string header]
		{
			get { return this.settings[header.ToLower()]; }
		}

		/// <summary>
		/// Get the specified header, key and return the value.
		/// </summary>
		/// <returns>
		/// True on successful retrival of value
		/// When false, result is not changed
		/// </returns>
		/// <param name="header">Header of the section</param>
		/// <param name="key">Key name</param>
		/// <param name="result">Result</param>
		public bool Get (string header , string key , ref string result)
		{
			if (Exists(header, key))
			{
				result = this.settings[header.ToLower()][key.ToLower()];
				return true;
			}
			return false;
		}
		/// <seealso cref="Get"></seealso>
		public bool GetBool (string header , string key , ref bool result)
		{
			string val = "";
			if (Get(header, key , ref val))
			{
				return bool.TryParse( val , out result );
			}
			return false;
		}

		/// <seealso cref="Get"></seealso>
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

