using System;
using Rhovlyn.Engine.IO;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Util;
using System.IO;
using SharpDL.Graphics;
using SharpDL;

namespace Rhovlyn.Engine.Maps
{
	class MapSection : Rhovlyn.Engine.Graphics.IDrawable
	{
		public MapSection(Rectangle area)
		{
			this.Area = area;
		}

		public void Draw(GameTime gameTime, Renderer renderer, Camera camera)
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

		public Vector Position { get; set; }

		public Rectangle Area { get; set; }

		public bool Loaded { get; set; }
	}

	public class PartialMap : Map
	{
		private PartialFile mapfile;
		private AreaMap<MapSection> sections = new AreaMap<MapSection>();

		private TextureManager textures;

		public PartialMap(string path, TextureManager textures)
			: base()
		{
			this.textures = textures;
			mapfile = new PartialFile(path);
			Load(mapfile.LoadBlock(), textures);
			ParseBlocks();
		}

		private void ParseBlocks()
		{
			foreach (var str in this.mapfile.BlockNames) {
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

		public void LoadBlock(string name)
		{
			var then = DateTime.Now;
			var mem = mapfile.LoadBlock(name);
			if (mem == null)
				throw new IOException("Could not get Block Data");
			Load(mem, this.textures);
			Console.WriteLine("Loaded Block " + name + " in " + (DateTime.Now - then).TotalMilliseconds + "ms");
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var block in sections.Get(lastCamera)) {
				if (!block.Loaded) {
					LoadBlock(block.ToString());
					block.Loaded = true;
				}
			}

			base.Update(gameTime);
		}
	}
}

