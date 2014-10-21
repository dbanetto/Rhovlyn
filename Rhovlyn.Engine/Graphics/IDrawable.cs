using Rhovlyn.Engine.Util;
using SharpDL.Graphics;
using SharpDL;

namespace Rhovlyn.Engine.Graphics
{
	public interface IDrawable
	{
		void Draw(GameTime gameTime, Renderer renderer, Camera camera);

		void Update(GameTime gameTime);

		Vector Position { get; set; }

		Rectangle Area { get; }
	}
}

