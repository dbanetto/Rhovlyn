using System;
using System.IO;
using System.Collections.Generic;
using Rhovlyn.Engine.Util;
using SharpDL.Input;
using SDL2;

namespace Rhovlyn.Engine.Input
{
	public struct KeyCondition
	{
		public KeyCondition(List<SDL.SDL_Scancode> keys = null)
		{
			this.keys = keys ?? new List<SDL.SDL_Scancode>();

		}

		List<SDL.SDL_Scancode> keys;

		public List<SDL.SDL_Scancode> Keys { get { return keys; } set { keys = value; } }
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
			if (!Parser.Exists<KeyCondition>())
				Parser.Add<KeyCondition>(ParseKeyBinding);
		}

		public bool Load(string path)
		{
			try {
				settings = new IO.Settings(path);
				ParseSettings();
				return true;
			} catch {
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
			if (Exists(name)) {
				var state = Keyboard.GetState(); //FIXME
				foreach (var k in keys[name].Keys) {
					if (state.Keys[(int)k].State == 0)
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

			if (Exists(name)) {
				object key = condition;
				keys[name] = (KeyCondition)(key);
				return true;
			}
			return false;
			
		}

		public bool Add<T>(string name, T condition)
		{
			if (typeof(T) != typeof(KeyCondition))
				throw new InvalidDataException("Can only pass KeyCondition to KeyBoardProvider's add");

			if (!Exists(name)) {
				keys[name] = (KeyCondition)(object)(condition);
				return true;
			} 
			return false;
		}

		public void ParseSettings()
		{
			//Go through ALL of the headers
			foreach (var header in settings.Headers) {
				//Check all thier values
				foreach (var key in settings[header].Keys) {
					//if they have THIS Providers SettingsPostfix then deal with it
					if (key.EndsWith(SettingsPostfix)) {
						var result = new KeyCondition(null);
						if (Parser.TryParse<KeyCondition>(settings[header][key], ref result)) {
							//Remove SettingsPostfix from the key
							//On a unsuccessful Parse, stop
							Add(header + "." + key.Substring(0, key.Length - SettingsPostfix.Length - 1),
								result);
						} else {
							throw new InvalidDataException("Input Settings failed to load");
						}
					}
				}
			}
		}

		/// <summary>
		/// Parses a Key binding
		/// </summary>
		/// <returns><c>non-null</c>, if setting was parsed, <c>null</c> otherwise error has occured.</returns>
		/// <param name="keystring">Key string.</param>
		/// <remarks>Key String is a + sperated list containing English letters or scan-codes
		/// refer to https://github.com/mono/MonoGame/blob/develop/MonoGame.Framework/Input/Keys.cs for scan-codes
		/// </remarks>
		/// <note>This is to be a parser for Rhovlyn.Engine.Util.Parser and follows the delegate ObjectParser</note>
		public static object ParseKeyBinding(string keystring)
		{
			var key = new KeyCondition();
			key.Keys = new List<SDL.SDL_Scancode>();
			var segs = keystring.Split('+');

			foreach (var seg in segs) {
				var scancode = SDL.SDL_GetScancodeFromName(seg);
				if (scancode != SDL.SDL_Scancode.SDL_SCANCODE_UNKNOWN)
					key.Keys.Add(scancode);
				else
					return null;
			}
			return key;
		}

	}
}

