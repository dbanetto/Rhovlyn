using System;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Managers;

namespace Rhovlyn.Engine.Controller
{
    public interface IController
    {
		/// <summary>
		/// Initialize the GameState
		/// </summary>
		void Initialize();

		/// <summary>
		/// Load content to ContentManager
		/// </summary>
		/// <param name="content">Content.</param>
		void LoadContent(ContentManager content);

		/// <summary>
		/// Unload content to ContentManager
		/// </summary>
		/// <param name="content">Content.</param>
		void UnLoadContent(ContentManager content);


		void Update (GameTime gameTime);

    }
}

