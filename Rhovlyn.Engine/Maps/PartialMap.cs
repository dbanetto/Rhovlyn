using System;
using Rhovlyn.Engine.IO;
using Rhovlyn.Engine.Managers;


namespace Rhovlyn.Engine.Maps
{
	public class PartialMap : Map
    {
		private PartialFile mapfile;

		public PartialMap( string path , ContentManager content )
			: base( content )
        {
			this.mapfile = new PartialFile(path);
			base.Load(mapfile.LoadBlock());
        }

		public bool LoadBlock (string name)
		{
			var mem = mapfile.LoadBlock(name);
			if (mem == null)
				return false;
			return base.Load(mem);
		}

    }
}

