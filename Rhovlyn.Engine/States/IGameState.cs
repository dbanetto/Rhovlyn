using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Managers;

namespace Rhovlyn.Engine.States
{
	public interface IGameState
	{
		/// <summary>
		/// Initialize the GameState
		/// </summary>
		/// <remark>
		/// Is called on "Add" to Manager 
		/// </remark>
		void Initialize();

		/// <summary>
		/// Loads content to ContentManager
		/// </summary>
		/// <remarks>
		/// Is called when Current state changes to this state
		/// </remarks>
		/// <param name="content">Content.</param>
		void LoadContent(ContentManager content);

		/// <summary>
		/// Unloads content to ContentManager
		/// </summary>
		/// <remarks>
		/// Is called when Current state changes away from this state
		/// </remarks>
		/// <param name="content">Content.</param>
		void UnLoadContent(ContentManager content);


		void Draw (GameTime gameTime , SpriteBatch spriteBatch , Camera camera);
		void Update (GameTime gameTime);

	}
}

