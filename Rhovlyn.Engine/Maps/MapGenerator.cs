using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rhovlyn.Engine.Maps
{
	public static class MapGenerator
	{
		public static void GenerateMap (string outPath , int seed)
		{

			using (var writer = new StreamWriter(new FileStream(outPath , FileMode.Create)))
			{
				GenerateMap( writer, seed);
			}


		}

		public static void GenerateMap( StreamWriter writer , int seed )
		{
			var rnd = new Random(seed);
			var tiles = new Dictionary<Point, int>();
			var nodes = new List<Point>();

			tiles.Add(new Point(0, 0), 2);

			//Initial room
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					if (!tiles.ContainsKey(new Point(x, y))) {
						tiles.Add(new Point(x, y), 2);
						nodes.Add(new Point(x, y));
					}
				}
			}

			while (nodes.Count != 0 && tiles.Count < 1000) {
				var node = nodes[0];
				nodes.RemoveAt(0);
				//const type due to stone
				int type = 2;

				double sumX = 0, sumY = 0, sumN = 0;
				for (int x = (int)node.X - 1; x <= (int)node.X + 1; x++) {
					for (int y = (int)node.Y - 1; y <= (int)node.Y + 1; y++) {
						if (y == (int)node.Y && x == (int)node.X)
							continue;

						if (!tiles.ContainsKey(new Point(x, y)))
							continue;

						//RNG gods may want to change directions
						if (rnd.NextDouble() < 0.2) {
							sumX += y - node.X;
							sumY += x - node.Y;
						}
						else {
							sumX += x - node.X;
							sumY += y - node.Y;
						}
						sumN++;
					}
				}
				//Get a value from the "average flow" of the tiles
				int outX = (int)MathHelper.Clamp((int)Math.Round(-sumX / (sumN * (0.5 + 1.5 * rnd.NextDouble()))), -1, 1);
				int outY = (int)MathHelper.Clamp((int)Math.Round(-sumY / (sumN * (0.5 + 1.5 * rnd.NextDouble()))), -1, 1);

				//Change it up if the RNG gods wish it
				//But if it means going back onto another tile, then do not bother swapping it
				if (rnd.NextDouble() < 0.2 && !tiles.ContainsKey(new Point(node.X - outX, node.Y + outY)))
					outX = -outX;
				if (rnd.NextDouble() < 0.2 && !tiles.ContainsKey(new Point(node.X + outX, node.Y - outY)))
					outY = -outY;


				//Make a room
				if (rnd.NextDouble() < 0.05) {
					//Make a room with the dimensions (size*2 -1) by (size*2 -1)

					//Tends towards 3 by 3 rooms but can go up to 9 by 9
					int size = (int)Math.Max(rnd.NextDouble() * 4.0 - 1.0, 1);

					for (int x = (int)node.X - size; x <= (int)node.X + size; x++) {
						for (int y = (int)node.Y - size; y <= (int)node.Y + size; y++) {
							if (!tiles.ContainsKey(new Point(x, y))) {
								tiles.Add(new Point(x, y), 2);
								//Only Edge nodes need to be checked
								if (x == (int)node.X - size || x == (int)node.X + size ||
									y == (int)node.Y - size || y == (int)node.Y + size) {
									nodes.Add(new Point(x, y));
								}
							}
						}
					}
				}

				//Helps to make all tiles are touching another tile
				if (outX != 0 && outY != 0) {
					if (rnd.NextDouble() > 0.5) {
						outX = 0;
					}
					else {
						outY = 0;
					}
				}

				outX += (int)node.X;
				outY += (int)node.Y;

				if (!tiles.ContainsKey(new Point(outX, outY))) {
					tiles.Add(new Point(outX, outY), type);
					nodes.Add(new Point(outX, outY));
				}
			}

			//Write out all the tiles to file
			foreach (var t in tiles) {
				writer.WriteLine(t.Value + "," + (int)t.Key.X + "," + (int)t.Key.Y);
			}
		} 

	}
}

