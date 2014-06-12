using System;
using Rhovlyn.Engine.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Rhovlyn.Engine.Util
{
	public class AreaMap<T>  where T : Graphics.IDrawable
	{
		//Node Count to split on
		public const int NodesToSplit = 10;
		//Maximum Depth
		public const int DepthLimit = 10;
		//Minimum area of a Child
		public static readonly Rectangle MinimumArea = new Rectangle(0,0,64,64);

		public List<T> Nodes { get; private set;}
		public AreaMap<T>[] Children { get; private set;}
		public Rectangle Area { get; private set; }
		public bool HasChildren {get; private set;}
		public int Depth { get; private set;}


		//Root Node declearation
		public AreaMap()
		{
			Nodes = new List<T>();
			Children = new AreaMap<T>[4];
			Area = Rectangle.Empty;
			Depth = 0;

			Children[0] = new AreaMap<T>(new Rectangle( int.MinValue, int.MinValue, int.MaxValue, int.MaxValue ), Depth + 1);
			Children[1] = new AreaMap<T>(new Rectangle( int.MinValue,			0 , int.MaxValue, int.MaxValue ), Depth + 1);
			Children[2] = new AreaMap<T>(new Rectangle( int.MinValue,			0 , int.MaxValue, int.MaxValue ), Depth + 1);
			Children[3] = new AreaMap<T>(new Rectangle(	            0,          0 , int.MaxValue, int.MaxValue ), Depth + 1);
			HasChildren = true;
		}

		//Child Node Declearation, can be used a root node
		public AreaMap(Rectangle area , int depth = 0)
		{
			Nodes = new List<T>();
			Children = new AreaMap<T>[4];
			Area = area;
			Depth = depth;
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

		public bool Add(T obj)
		{
			if (this.Area != Rectangle.Empty && !this.Area.Contains(obj.Area))
				return false;

			if (HasChildren)
			{
				foreach (var child in Children)
				{
					if (child.Area.Contains(obj.Area))
					{
						if (child.Add(obj))
							return true;
					}
				}
			}

			this.Nodes.Add(obj);
			if (!HasChildren && this.Nodes.Count > NodesToSplit)
			{
				this.Split();
			}
			return true;
		}

		public void Split()
		{
			if (HasChildren)
				throw new Exception("Cannot split when you already have children.");

			if (Depth == DepthLimit)
				return;


			int new_width = this.Area.Width / 2;
			int new_height = this.Area.Height / 2;

			if (!new Rectangle(0, 0, new_width, new_height).Contains(MinimumArea))
				return;

			Children[0] = new AreaMap<T>(new Rectangle( this.Area.X, this.Area.Y, new_width, new_height ), Depth + 1);
			Children[1] = new AreaMap<T>(new Rectangle( this.Area.X, this.Area.Y + new_height, new_width, new_height), Depth + 1);
			Children[2] = new AreaMap<T>(new Rectangle( this.Area.X + new_width, this.Area.Y + new_height, new_width, new_height ), Depth + 1);
			Children[3] = new AreaMap<T>(new Rectangle( this.Area.X + new_width, this.Area.Y, new_width, new_height), Depth + 1);
			HasChildren = true;

			var toRemove = new List<T>();
			foreach (var obj in this.Nodes)
			{
				foreach (var child in Children)
				{
					if (child.Area.Contains(obj.Area))
					{
						if (child.Add(obj))
							toRemove.Add(obj);
					}
				}
			}

			foreach (var obj in toRemove)
			{
				this.Nodes.Remove(obj);
			}
		}

		public int Count { 
			get { 
				int count = this.Nodes.Count;
				if (HasChildren)
				{
					foreach (var child in this.Children)
					{
						count += child.Count;
					}
				}
				return count;
			} 
		}

		#if DEBUG
		public void Draw (SpriteBatch spritebatch , Camera camera)
		{
			if (HasChildren)
			{
				foreach (var child in this.Children)
				{
					child.Draw(spritebatch, camera);
				}
			}
			if (camera.Bounds.Intersects(this.Area))
			{
				Primitives2D.DrawRectangle(spritebatch, 
					new Rectangle(this.Area.X - (int)camera.Position.X, this.Area.Y - (int)camera.Position.Y, this.Area.Width, this.Area.Height)
				, Color.Green);
			}
		}
		#endif
	}
}

