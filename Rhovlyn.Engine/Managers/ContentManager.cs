using System;
using Rhovlyn.Engine.IO;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.States;
using System.IO;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Managers
{
	public class ContentManager
	{
		public ContentManager(string settingsPath)
		{
			Parser.init();
			Settings = new Settings(settingsPath);
			string path = "";
			if (Settings.Get<String>("core", "input.settings", ref path))
			{
				Input = new InputManager(path);
			}
			else
			{
				throw new IOException("Cannot find input settings");
			}
		}

		public void Init(GraphicsDevice device)
		{
			GraphicsDevice = device;
			Textures = new TextureMananger(device);
			GameStates = new GameStateManager(this);
			Sprites = new SpriteManager(this);
			Maps = new MapManager(this);
			Audio = new AudioManager();

		}

		public Map CurrnetMap { get { return Maps.CurrentMap; } }

		public IGameState CurrentState { get { return GameStates.CurrnetState; } }

		public Camera Camera { get; set; }

		public AudioManager Audio { get; private set; }

		public SpriteManager Sprites { get; private set; }

		public Settings Settings { get; private set; }

		public TextureMananger Textures { get; private set; }

		public MapManager Maps { get; private set; }

		public GameStateManager GameStates { get; private set; }

		public InputManager Input { get; private set; }

		public GraphicsDevice GraphicsDevice { get; private set; }
	}
}

