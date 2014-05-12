using System;
using Rhovlyn.Engine.IO;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Maps;
using Rhovlyn.Engine.States;

namespace Rhovlyn.Engine.Managers
{
    public class ContentManager
    {
		public ContentManager (string settingsPath)
		{
			Settings = new Settings(settingsPath);
		}

		public void Init (GraphicsDevice device)
		{
			Textures = new TextureMananger(device);
			GameStates = new GameStateManager(this);
			Maps = new MapManager();
		}

		public Map CurrnetMap {get { return Maps.CurrentMap; } }
		public IGameState CurrentState { get { return GameStates.CurrnetState; }} 

		public Settings Settings { get; private set; }
		public TextureMananger Textures { get; private set; }
		public MapManager Maps { get; private set; }
		public GameStateManager GameStates { get; private set; }

    }
}

