using System;
using Rhovlyn.Engine.IO;
using Rhovlyn.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rhovlyn.Engine.Util;

namespace Rhovlyn.Engine.Maps
{
	class MapSection : Rhovlyn.Engine.Graphics.IDrawable
	{
		public MapSection(Rectangle area)
		{
			this.Area = area;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
		{
			throw new NotImplementedException();
		}

		public void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return string.Format(String.Format("{0},{1},{2},{3}", this.Area.X, this.Area.Y, this.Area.Width, this.Area.Height));
		}

		public Vector2 Position { get; set; }

		public Rectangle Area { get; set; }

		public bool Loaded { get; set; }
	}

	public class PartialMap : Map
	{
		private PartialFile mapfile;
		private AreaMap<MapSection> sections = new AreaMap<MapSection>();

		public PartialMap(string path, ContentManager content)
			: base(content)
		{
			this.mapfile = new PartialFile(path);
			base.Load(mapfile.LoadBlock());
			ParseBlocks();
		}

		private void ParseBlocks()
		{
			foreach (var str in this.mapfile.BlockNames)
			{
				var parts = str.Split(',');
				if (parts.Length != 4)
					continue;

				int x = int.Parse(parts[0]);
				int y = int.Parse(parts[1]);
				int w = int.Parse(parts[2]);
				int h = int.Parse(parts[3]);

				sections.Add(new MapSection(new Rectangle(x, y, w, h)));
			}
		}

		public bool LoadBlock(string name)
		{
			var then = DateTime.Now;
			var mem = mapfile.LoadBlock(name);
			if (mem == null)
				return false;
			var loaded = base.Load(mem);
			Console.WriteLine("Loaded Block " + name + " in " + (DateTime.Now - then).TotalMilliseconds + "ms");
			return loaded;
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var block in this.sections.Get(this.Content.Camera.Bounds))
			{
				if (!block.Loaded)
				{
					LoadBlock(block.ToString());
					block.Loaded = true;
				}
			}

			base.Update(gameTime);
		}
	}
}

