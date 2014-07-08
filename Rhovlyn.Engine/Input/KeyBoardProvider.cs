using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Rhovlyn.Engine.Input
{
	public struct KeyCondition
	{
		public KeyCondition(List<Keys> keys = null)
		{
			if (keys != null)
				this.keys = keys;
			else
				this.keys = new List<Keys>();
		}

		List<Keys> keys;

		public List<Keys> Keys { get { return keys; } set { keys = value; } }
	}

	public class KeyBoardProvider : IInputProvider
	{
		public string SettingsPostfix { get; private set; }

		private IO.Settings settings;
		private Dictionary<string ,  KeyCondition > keys;

		public KeyBoardProvider()
		{
			SettingsPostfix = "keys";
			keys = new Dictionary<string ,  KeyCondition >();
		}

		public bool Load(string path)
		{
			try
			{
				settings = new IO.Settings(path);
				ParseSettings();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void Unload()
		{

		}

		public bool Exists(string name)
		{
			return keys.ContainsKey(name);
		}

		public bool GetState(string name)
		{
			if (this.Exists(name))
			{
				var state = Keyboard.GetState();
				foreach (var k in keys[name].Keys)
				{
					if (state.IsKeyDown(k) == false)
						return false;
				}
				return true;
			}
			return false;
		}

		public bool SetCondition<T>(string name, T condition)
		{
			if (typeof(T) != typeof(KeyCondition))
				throw new InvalidDataException("Can only pass KeyCondition to KeyBoardProvider's set");

			if (Exists(name))
			{
				object key = condition;
				this.keys[name] = (KeyCondition)(key);
				return true;
			}
			return false;
			
		}

		public bool Add<T>(string name, T condition)
		{
			if (typeof(T) != typeof(KeyCondition))
				throw new InvalidDataException("Can only pass KeyCondition to KeyBoardProvider's add");

			if (!Exists(name))
			{
				object key = condition;
				this.keys[name] = (KeyCondition)(key);
				return true;
			} 
			return false;
		}

		public void ParseSettings()
		{
			//Go through ALL of the headers
			foreach (var header in settings.Headers)
			{
				//Check all thier values
				foreach (var key in settings[header].Keys)
				{
					//if they have THIS Providers SettingsPostfix then deal with it
					if (key.EndsWith(SettingsPostfix))
					{
						KeyCondition result = new KeyCondition(null);
						if (ParseKeyBinding(settings[header][key], ref result))
						{
							//Remove SettingsPostfix from the key
							//On a unsuccessful Parse, stop
							this.Add(header + "." + key.Substring(0, key.Length - SettingsPostfix.Length - 1),
								result);
						}
						else
						{
							throw new InvalidDataException("Input Settings failed to load");
						}
					}
				}
			}
		}

		/// <summary>
		/// Parses a Key binding
		/// </summary>
		/// <returns><c>true</c>, if setting was parsed, <c>false</c> otherwise error has occured.</returns>
		/// <param name="keystring">Key string.</param>
		/// <remarks>Key String is a + sperated list containing English letters or scan-codes
		/// refer to https://github.com/mono/MonoGame/blob/develop/MonoGame.Framework/Input/Keys.cs for scan-codes
		/// </remark>
		/// <param name="key">Key.</param>
		public static bool ParseKeyBinding(string keystring, ref KeyCondition key)
		{
			var segs = keystring.Split('+');
			if (key.Keys == null)
				key.Keys = new List<Keys>();

			foreach (var seg in segs)
			{
				//Convert a singel letter to scancode
				if (seg.Length == 1)
				{
					//XNA Scan codes for letters are mapped to UTF-8's upper case english letters
					var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(seg.ToUpper());
					if (bytes.Length == 1)
					{
						if (bytes[0] >= 65 && bytes[0] <= 90)
						{
							key.Keys.Add((Keys)bytes[0]);
							continue;
						}
					}
				}
				//Read in as Scancodes
				int code;
				if (int.TryParse(seg, out code))
				{
					key.Keys.Add((Keys)code);
				}
				else
				{
					return false;
				}
			}
			return true;
		}

	}
}

