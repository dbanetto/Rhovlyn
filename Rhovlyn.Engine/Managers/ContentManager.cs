using Rhovlyn.Engine.IO;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.States;
using Rhovlyn.Engine.Util;
using Microsoft.Xna.Framework.Graphics;

namespace Rhovlyn.Engine.Managers
{
	public class ContentManager
	{
		public ContentManager(string settingsPath)
		{
			Parser.Init();
			Settings = new Settings(settingsPath);
		}

		public void Init(GraphicsDevice device)
		{
			GraphicsDevice = device;
			Textures = new TextureManager(device);
			GameStates = new GameStateManager(this);
			Sprites = new SpriteManager(Textures);
			Maps = new MapManager(Textures, Sprites);
			Audio = new AudioManager();
			Input = new InputManager();
		}

		public Map CurrnetMap { get { return Maps.CurrentMap; } }

		public IGameState CurrentState { get { return GameStates.CurrnetState; } }

		public Camera Camera { get; set; }

		public AudioManager Audio { get; private set; }

		public SpriteManager Sprites { get; private set; }

		public Settings Settings { get; private set; }

		public TextureManager Textures { get; private set; }

		public MapManager Maps { get; private set; }

		public GameStateManager GameStates { get; private set; }

		public InputManager Input { get; private set; }

		public GraphicsDevice GraphicsDevice { get; private set; }
	}
}

