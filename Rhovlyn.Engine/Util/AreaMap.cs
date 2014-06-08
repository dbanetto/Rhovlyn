using System;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Rhovlyn.Engine.Util
{
	public class AreaMap<T>  where T : Graphics.IDrawable
	{
		public const int NodesToSplit = 10;

		public List<T> Nodes { get; private set;}
		public AreaMap<T>[] Children { get; private set;}
		public Rectangle Area { get; private set; }
		public bool HasChildren {get; private set;}

		//Root Node declearation
		public AreaMap()
		{
			Nodes = new List<T>();
			Children = new AreaMap<T>[4];
			Area = Rectangle.Empty;

			Children[0] = new AreaMap<T>(new Rectangle( int.MinValue, int.MinValue , int.MaxValue, int.MaxValue ));
			Children[1] = new AreaMap<T>(new Rectangle( int.MinValue,			0 , int.MaxValue, int.MaxValue ));
			Children[2] = new AreaMap<T>(new Rectangle( int.MinValue,			0 , int.MaxValue, int.MaxValue ));
			Children[3] = new AreaMap<T>(new Rectangle(			0,			0 , int.MaxValue, int.MaxValue ));
			HasChildren = true;
		}

		//Child Node Declearation, can be used a root node
		public AreaMap(Rectangle area)
		{
			Nodes = new List<T>();
			Children = new AreaMap<T>[4];
			Area = area;
			HasChildren = false;
		}

		public T[] Get(Rectangle area)
		{
			List<AreaMap<T>> to_check = new List<AreaMap<T>>();
			List<T> output = new List<T>();
			to_check.Add(this);

			while (to_check.Count != 0)
			{
				var check = to_check[0];
				to_check.RemoveAt(0);

				foreach (var obj in check.Nodes)
				{
					if (area.Intersects(obj.Area))
					{
						output.Add(obj);
					}
				}

				if (!check.HasChildren)
					continue;

				foreach (var child in check.Children)
				{
					if (area.Intersects(child.Area))
					{
						to_check.Add(child);
					}
				}

			}

			return output.ToArray();
		}

		public void Add(T obj)
		{
			if (HasChildren)
			{
				foreach (var child in Children)
				{
					if (child.Area.Contains(obj.Area))
					{
						child.Add(obj);
						return;
					}
				}
			}

			this.Nodes.Add(obj);
			if (!HasChildren && this.Nodes.Count > NodesToSplit)
			{
				this.Split();
			}
		}

		public void Split()
		{
			if (HasChildren)
				throw new Exception("Cannot split when you already have children.");

			HasChildren = true;
			int new_width = this.Area.Width / 2;
			int new_height = this.Area.Height / 2;

			Children[0] = new AreaMap<T>(new Rectangle( this.Area.X, this.Area.Y, new_width, new_height ));
			Children[1] = new AreaMap<T>(new Rectangle( this.Area.X, this.Area.Y + new_height, new_width, new_height ));
			Children[2] = new AreaMap<T>(new Rectangle( this.Area.X + new_width, this.Area.Y + new_height, new_width, new_height ));
			Children[3] = new AreaMap<T>(new Rectangle( this.Area.X + new_width, this.Area.Y, new_width, new_height ));


		}

	}
}

