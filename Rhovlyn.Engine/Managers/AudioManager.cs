using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.IO;

namespace Rhovlyn.Engine.Managers
{
	public class AudioManager
	{
		private Dictionary< string , SoundEffect > sounds;
		private List<SoundEffectInstance> instances;
		private AudioListener listener;

		public Graphics.IDrawable ListenerObject { get; set; }

		public AudioManager()
		{
			sounds = new Dictionary<string, SoundEffect>();
			instances = new List<SoundEffectInstance>();
			listener = new AudioListener();
			listener.Velocity = Vector3.One;
		}

		public void Update()
		{
			if (ListenerObject != null)
				listener.Position = new Vector3(ListenerObject.Position, 0);

			//Remove the dead instances
			for (int i = 0; i < instances.Count; i++)
			{
				if (instances[i].IsDisposed || instances[i].State == SoundState.Stopped)
					instances.RemoveAt(i--);
			}
		}

		public bool Play(string name, bool loop = false)
		{
			if (this.Exists(name))
			{
				var inst = this.sounds[name].CreateInstance();
				instances.Add(inst);
				inst.Play();

				return true;
			}
			return false;
		}

		#region Management

		public SoundEffect Get(string name)
		{
			if (Exists(name))
			{
				return this.sounds[name];
			}
			return null;
		}

		public bool Add(string name, SoundEffect sound)
		{
			if (!Exists(name))
			{
				sounds.Add(name, sound);
				return true;
			}
			return false;
		}

		public bool Add(string name, string path)
		{
			if (!Exists(name))
			{
				using (var fs = new FileStream(path, FileMode.Open))
				{
					sounds.Add(name, SoundEffect.FromStream(fs));
					return true;
				}
			}
			return false;
		}

		public bool Exists(string name)
		{
			return this.sounds.ContainsKey(name);
		}

		#endregion

	}
}

