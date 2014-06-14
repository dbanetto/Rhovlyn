using System;
using System.Collections.Generic;
using Rhovlyn.Engine.States;

namespace Rhovlyn.Engine.Managers
{
	public class GameStateManager
	{
		Dictionary< string , IGameState > states;
		string currnet;
		ContentManager content;

		public GameStateManager(ContentManager content)
		{
			states = new Dictionary<string, IGameState>();
			this.content = content; 
		}

		#region Management

		public IGameState CurrnetState { get { return this.states[Current]; } }

		public string Current
		{
			get
			{
				return this.currnet;
			}
			set
			{
				if (currnet != null)
					states[currnet].UnLoadContent(content);
				this.currnet = value;
				states[currnet].LoadContent(content);
			}
		}

		public bool Add(string name, IGameState state)
		{
			if (!Exists(name))
			{
				state.Initialize();
				states.Add(name, state);
				if (states.Count == 1)
					Current = name;
				return true;
			}
			return false;
		}

		public bool Exists(string name)
		{
			return this.states.ContainsKey(name);
		}

		#endregion

	}
}

