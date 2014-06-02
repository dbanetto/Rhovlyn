using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;

namespace Rhovlyn.Engine.Graphics
{
	public delegate void AnimationFrameChangedHandler ( AnimatedSprite sprite , int index );
	public delegate void AnimationEndedHandler ( AnimatedSprite sprite );
	public delegate void AnimationStartedHandler ( AnimatedSprite sprite );

	public struct Animation
	{
		public Animation(List<int> frames = null , List<double> times = null , int loop = 1 ) { 
			if (frames != null)
				this.frames = frames;
			else
				this.frames = new List<int>();

			if (times != null)
				this.times = times;
			else
				this.times = new List<double>();

			AnimationStarted = null;
			AnimationEnded = null;
			FrameChanged = null;

			this.loop = loop;
		}

		List<int> frames;
		public List<int> Frames { get { return frames; } }   // Frame to use for index of animation

		List<double> times;
		public List<double> Times { get { return  times; } } //Times for each frames

		int loop;
		public int Loop { get{ return this.Loop; } }

		//Events for Starting, Ending and changes in animation
		public event AnimationStartedHandler AnimationStarted;
		public event AnimationEndedHandler AnimationEnded;
		public event AnimationFrameChangedHandler FrameChanged;

		public void OnAnimationStarted (AnimatedSprite sprite)
		{
			if (AnimationStarted != null)
				AnimationStarted(sprite);
		}
		public void OnAnimationEnded (AnimatedSprite sprite)
		{
			if (AnimationEnded != null)
				AnimationEnded(sprite);
		}
		public void OnFrameChanged (AnimatedSprite sprite , int index)
		{
			if (FrameChanged != null)
				FrameChanged(sprite , index);
		}

	};

	public class AnimatedSprite : Sprite
	{
		private Dictionary < string , Animation > animations;
		public string CurrentAnimation { get; private set; }
		private int index;
		private double current_delta;
		private int loop_count = 0;

		public AnimatedSprite(Vector2 position, SpriteMap spritemap)
			: base ( position , spritemap )
		{
			animations = new Dictionary < string , Animation >();
		}

		public override void Update (GameTime gameTime )
		{
			//Update Timer for the frame
			current_delta -= gameTime.ElapsedGameTime.TotalSeconds;
			if (current_delta < 0)
			{
				if (animations[CurrentAnimation].Frames.Count > index + 1)
				{
					loop_count++;
					if (animations[CurrentAnimation].Loop < loop_count)
					{
						index = 0;
						this.Frameindex = animations[CurrentAnimation].Frames[index];
						current_delta = animations[CurrentAnimation].Times[index];
						animations[CurrentAnimation].OnFrameChanged(this, index);
					}
					else
					{
						animations[CurrentAnimation].OnAnimationEnded(this);
					}
				}
				else
				{
					index++;
					this.Frameindex = animations[CurrentAnimation].Frames[index];
					current_delta = animations[CurrentAnimation].Times[index];
					animations[CurrentAnimation].OnFrameChanged(this , index);
				}

			}
		}

		#region Animation Management

		public bool AddAnimation (string name , Animation animation )
		{
			if (!ExistsAnimation(name))
			{
				animations.Add(name, animation);
				return true;
			}
			return false;
		}

		public bool ExistsAnimation ( string name)
		{
			return this.animations.ContainsKey(name);
		}

		public bool SetAnimation ( string name )
		{
			if (ExistsAnimation(name))
			{
				//End the last animation
				if (CurrentAnimation != null)
					animations[CurrentAnimation].OnAnimationEnded(this);

				//Set up for the new animation
				CurrentAnimation = name;
				this.index = 0;
				this.loop_count = 0;
				this.Frameindex = animations[CurrentAnimation].Frames[index];
				current_delta = animations[CurrentAnimation].Times[index];
				animations[CurrentAnimation].OnAnimationStarted(this);
				animations[CurrentAnimation].OnFrameChanged(this , index);

				return true;
			}
			return false;
		}

		public static bool LoadAnimation (string raw , ref Animation ani)
		{
			List<int> frames = new List<int>();
			List<double> times = new List<double>();
			int loops = 0;
			//Example
			// 1,2:0.1,3:1.0
			// Read as : 1 loop , 1st index frame=2 for 0.1sec, 2nd index frame=3 for 1.0sec
			foreach (var seg in raw.Split(','))
			{
				var opt = seg.Split(':');
				if (opt.Length == 1)
				{
					loops = int.Parse(opt[0]);
				}
				else if (opt.Length == 2)
				{
					frames.Add( int.Parse(opt[0]));
					times.Add( double.Parse(opt[1]));
				}
				else
				{
					throw new InvalidDataException("Invalid Animation segment : " + seg);
				}
			}
			ani = new Animation(frames, times, loops);
			return true;
		}
		#endregion

	}
}

