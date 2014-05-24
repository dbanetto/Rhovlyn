using System;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Controller
{
	public class CameraController : IController
    {
		public Camera Camera { get; private set; }

		public CameraController(Camera camera)
        {
			Camera = camera;
        }

		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public void LoadContent(ContentManager content)
		{
			throw new NotImplementedException();
		}

		public void UnLoadContent(ContentManager content)
		{
			throw new NotImplementedException();
		}

		public void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
    }
}

