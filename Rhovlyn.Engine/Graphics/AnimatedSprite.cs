using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;

namespace Rhovlyn.Engine.Graphics
{
	public delegate void AnimationFrameChangedHandler(AnimatedSprite sprite,int index);
	public delegate void AnimationEndedHandler(AnimatedSprite sprite);
	public delegate void AnimationStartedHandler(AnimatedSprite sprite);

	public class Animation
	{
		public Animation(List<int> frames = null, List<double> times = null, int loop = 1)
		{ 
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

		public List<int> Frames { get { return frames; } }
		// Frame to use for index of animation

		List<double> times;

		public List<double> Times { get { return  times; } }
		//Times for each frames

		int loop;

		public int Loop { get { return this.loop; } }

		//Events for Starting, Ending and changes in animation
		public event AnimationStartedHandler AnimationStarted;
		public event AnimationEndedHandler AnimationEnded;
		public event AnimationFrameChangedHandler FrameChanged;

		public void OnAnimationStarted(AnimatedSprite sprite)
		{
			if (AnimationStarted != null)
				AnimationStarted(sprite);
		}

		public void OnAnimationEnded(AnimatedSprite sprite)
		{
			if (AnimationEnded != null)
				AnimationEnded(sprite);
		}

		public void OnFrameChanged(AnimatedSprite sprite, int index)
		{
			if (FrameChanged != null)
				FrameChanged(sprite, index);
		}

	};

	public class AnimatedSprite : Sprite
	{
		private Dictionary < string , Animation > animations;

		public string CurrentAnimationName { get; private set; }

		public double AnimationSpeed { get; set; }

		private int index;
		private double currentDelta;
		private int loopCount = 0;

		public bool AnimationInProgress { get; private set; }

		public AnimatedSprite(Vector2 position, SpriteMap spritemap)
			: base(position, spritemap)
		{
			animations = new Dictionary < string , Animation >();
			AnimationSpeed = 1.0;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (CurrentAnimationName == null)
				return;

			//Update Timer for the frame
			currentDelta -= gameTime.ElapsedGameTime.TotalSeconds * AnimationSpeed;
			if (currentDelta < 0)
			{
				if (animations[CurrentAnimationName].Frames.Count == index + 1)
				{
					loopCount++;
					if (animations[CurrentAnimationName].Loop > loopCount)
					{
						index = 0;
						this.Frameindex = animations[CurrentAnimationName].Frames[index];
						currentDelta = animations[CurrentAnimationName].Times[index];
						animations[CurrentAnimationName].OnFrameChanged(this, index);
					}
					else
					{
						animations[CurrentAnimationName].OnAnimationEnded(this);
						AnimationInProgress = false;
					}
				}
				else
				{
					index++;
					this.Frameindex = animations[CurrentAnimationName].Frames[index];
					currentDelta = animations[CurrentAnimationName].Times[index];
					animations[CurrentAnimationName].OnFrameChanged(this, index);
				}
			}
		}

		public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Rhovlyn.Engine.Util.Camera camera)
		{
			base.Draw(gameTime, spriteBatch, camera);
		}

		#region Animation Management

		public bool AddAnimation(string name, Animation animation)
		{
			if (!ExistsAnimation(name))
			{
				animations.Add(name, animation);
				if (this.animations.Count == 1)
					this.CurrentAnimationName = name;
				return true;
			}
			return false;
		}

		public bool ExistsAnimation(string name)
		{
			return this.animations.ContainsKey(name);
		}

		public Animation GetAnimation(string name)
		{
			if (this.animations.ContainsKey(name))
			{
				return this.animations[name];
			}
			return null;
		}

		public bool SetAnimation(string name)
		{
			if (ExistsAnimation(name))
			{
				//End the last animation
				if (AnimationInProgress && CurrentAnimationName != null)
				{
					if (CurrentAnimationName != name)
						animations[CurrentAnimationName].OnAnimationEnded(this);
					else
						return true;
				}
				//Set up for the new animation
				CurrentAnimationName = name;
				this.index = 0;
				this.loopCount = 0;
				this.Frameindex = animations[CurrentAnimationName].Frames[index];
				currentDelta = animations[CurrentAnimationName].Times[index];
				animations[CurrentAnimationName].OnAnimationStarted(this);
				animations[CurrentAnimationName].OnFrameChanged(this, index);
				AnimationInProgress = true;
				return true;
			}
			return false;
		}

		public static object ParseAnimation(string text)
		{
			List<int> frames = new List<int>();
			List<double> times = new List<double>();
			int loops = 0;
			//Example
			// 1,2:0.1,3:1.0
			// #loops,[FrameIndex:Time]...
			// Read as : 1 loop , 1st index frame=2 for 0.1sec, 2nd index frame=3 for 1.0sec
			foreach (var seg in text.Split(','))
			{
				var opt = seg.Split(':');
				if (opt.Length == 1)
				{
					loops = int.Parse(opt[0]);
				}
				else if (opt.Length == 2)
				{
					frames.Add(int.Parse(opt[0]));
					times.Add(double.Parse(opt[1]));
				}
				else
				{
					throw new InvalidDataException("Invalid Animation segment : " + seg);
				}
			}
			return new Animation(frames, times, loops);
		}

		#endregion

	}
}

