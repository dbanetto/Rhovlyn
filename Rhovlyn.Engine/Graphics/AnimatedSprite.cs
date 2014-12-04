using SharpDL.Graphics;
using SharpDL;

namespace Rhovlyn.Engine.Graphics
{
	public class AnimatedSprite : Sprite
	{
		public string CurrentAnimationName { get; private set; }

		public double AnimationSpeed { get; set; }

		private int index;
		private double currentDelta;
		private int loopCount;

		public bool AnimationInProgress { get; private set; }

		public AnimatedSprite(Vector position, SpriteMap spritemap)
			: base(position, spritemap)
		{
			AnimationSpeed = 1.0;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (CurrentAnimationName == null)
				return;

			//Update Timer for the frame
			currentDelta -= gameTime.ElapsedGameTime.TotalSeconds * AnimationSpeed;
			if (currentDelta < 0) {
				if (SpriteMap.Animations[CurrentAnimationName].Frames.Count == index + 1) {
					loopCount++;
					if (SpriteMap.Animations[CurrentAnimationName].Loop > loopCount) {
						index = 0;
						Frameindex = SpriteMap.Animations[CurrentAnimationName].Frames[index];
						currentDelta = SpriteMap.Animations[CurrentAnimationName].Times[index];
						SpriteMap.Animations[CurrentAnimationName].OnFrameChanged(this, index);
					} else {
					SpriteMap.Animations[CurrentAnimationName].OnAnimationEnded(this);
					AnimationInProgress = false;
					}
				} else {
					index++;
					Frameindex = SpriteMap.Animations[CurrentAnimationName].Frames[index];
					currentDelta = SpriteMap.Animations[CurrentAnimationName].Times[index];
					SpriteMap.Animations[CurrentAnimationName].OnFrameChanged(this, index);
				}
			}
		}

		public bool SetAnimation(string name)
		{
			if (SpriteMap.ExistsAnimation(name)) {

				//End the last animation
				if (AnimationInProgress && CurrentAnimationName != null) {
					if (CurrentAnimationName != name)
						SpriteMap.Animations[CurrentAnimationName].OnAnimationEnded(this);
					else
						return true;
				}

				//Set up for the new animation
				CurrentAnimationName = name;
				index = 0;
				loopCount = 0;
				Frameindex = SpriteMap.Animations[CurrentAnimationName].Frames[index];
				currentDelta = SpriteMap.Animations[CurrentAnimationName].Times[index];
				SpriteMap.Animations[CurrentAnimationName].OnAnimationStarted(this);
				SpriteMap.Animations[CurrentAnimationName].OnFrameChanged(this, index);
				AnimationInProgress = true;
				return true;
			}
			return false;
		}

		
	}
}

