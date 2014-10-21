using SharpDL;
using System.Collections.Generic;
using System.IO;
using SDL2;
using System;

namespace Rhovlyn.Engine.Managers
{
	public class AudioManager
	{
		/*
		private Dictionary< string , IntPtr > sounds;
		//FIXME for audio

		public Graphics.IDrawable ListenerObject { get; set; }

		public AudioManager()
		{
			sounds = new Dictionary<string, IntPtr>();
		}

		public void Update()
		{
			//FIXME
		}

		public bool Play(string name, bool loop = false)
		{
			if (Exists(name)) {


				return true;
			}
			return false;
		}

		#region Management


		public SoundEffect Get(string name)
		{
			return Exists(name) ? sounds[name] : null;
		}

		public bool Add(string name, SoundEffect sound)
		{
			if (!Exists(name)) {
				sounds.Add(name, sound);
				return true;
			}
			return false;
		}

		public bool Add(string name, string path)
		{
			if (!Exists(name)) {
				using (var fs = new FileStream(path, FileMode.Open)) {
					sounds.Add(name, SoundEffect.FromStream(fs));
					return true;
				}
			}
			return false;
		}

		public bool Exists(string name)
		{
			return sounds.ContainsKey(name);
		}


		#endregion
		*/

	}
}

