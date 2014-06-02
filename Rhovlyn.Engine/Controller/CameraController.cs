using System;
using Microsoft.Xna.Framework;
using Rhovlyn.Engine.Managers;
using Rhovlyn.Engine.Util;
using Rhovlyn.Engine.Graphics;
using System.Collections.Generic;

namespace Rhovlyn.Engine.Controller
{
	public struct CameraCommand {
		public CameraCommand ( double MovementTimerTotal, Vector2 Target, bool TargetStartDiffer , Vector2 TargetStart) {
			this.movementTimer = MovementTimerTotal;
			this.movementTimerTotal = MovementTimerTotal;
			this.target = Target;
			this.targetStart = TargetStart;
			this.spriteTarget = null;
			this.targetStartDiffer = TargetStartDiffer;
		}

		public Vector2 target;
		public Vector2 targetStart;
		public double movementTimer;
		public double movementTimerTotal;
		public bool targetStartDiffer;
		public Graphics.IDrawable spriteTarget;
	}

	public class CameraController : IController
	{
		private const double INF_TIME = double.MinValue;


		public Camera Camera { get; private set; }
		private List<CameraCommand> commands;
		public int CommandsCount { get { return commands.Count; } }
		public bool InMotion { get; private set; }
		private CameraCommand current;

		public CameraController(Camera camera)
		{
			Camera = camera;
			commands = new List<CameraCommand>();
		}

		public void Initialize()
		{

		}

		public void LoadContent(ContentManager content)
		{

		}

		public void UnLoadContent(ContentManager content)
		{

		}

		public void Update(GameTime gameTime)
		{
			//An infinite timed command is used until another command is issued
			if (current.movementTimerTotal == INF_TIME) {
				this.Camera.Position = current.target;
			}

			//If the target is a sprite, update the target position
			if (current.spriteTarget != null) {
				current.target.X = (current.spriteTarget.Position.X - Camera.Bounds.Width / 2 + current.spriteTarget.Area.Width / 2);
				current.target.Y = (current.spriteTarget.Position.Y - Camera.Bounds.Height / 2 + current.spriteTarget.Area.Height / 2);
			}

			if (commands.Count > 0 && current.movementTimer <= 0) {
				//Pop the next command in line
				InMotion = false;
				current = commands[0];
				commands.RemoveAt(0);

				//If the target start is Vector.Zero then start where the Camera is now
				current.targetStart = ( !current.targetStartDiffer ? Camera.Position : current.targetStart);
				this.Camera.Position = current.targetStart;
			}

			if (current.movementTimer > 0) {
				current.movementTimer -= gameTime.ElapsedGameTime.TotalSeconds;

				if (current.movementTimer < 0) {
					//Corrects for any calculation error that occurred during the movement
					Camera.Position = current.target;
				} else {
					//Get the Percentage of how far the movement should be through
					Camera.Position = new Vector2 ( (float)((current.target.X - current.targetStart.X) * (current.movementTimerTotal - current.movementTimer) / current.movementTimerTotal + current.targetStart.X),
													(float)((current.target.Y - current.targetStart.Y) * (current.movementTimerTotal - current.movementTimer) / current.movementTimerTotal + current.targetStart.Y) );
					InMotion = true;
				}
			} 
		}

		/// <summary>
		/// Clears all Camera Commands that are queued
		/// Current Command is not stopped
		/// </summary>
		public void ClearCommands() {
			commands.Clear();
		}

		public void StopCurrentCommand() {
			this.current.movementTimer = -1;

			//Swap out the current to the next command
			if (commands.Count > 0) {
				//Pop the next command in line
				InMotion = false;
				current = commands[0];
				commands.RemoveAt(0);

				//If the target start is null then start where the Camera is now
				current.targetStart = ( !current.targetStartDiffer ? this.Camera.Position : current.targetStart);
				this.Camera.Position = current.targetStart;
			}
		}

		/// <summary>
		/// Centers Camera on Sprite
		/// </summary>
		/// <param name="sprite"><see cref="Sprite"/> to be focused on</param>
		public void FocusOn(Graphics.IDrawable target) {

			Vector2 targetpos = new Vector2();
			targetpos.X = (target.Position.X - Camera.Bounds.Width / 2 + target.Area.Width / 2);
			targetpos.Y = (target.Position.Y - Camera.Bounds.Height / 2 + target.Area.Height / 2);
			var cmd = new CameraCommand(INF_TIME, targetpos , true , targetpos);
			//Thanks to the wonders of references this keeps the target up to date
			cmd.spriteTarget = target;
			this.commands.Add(cmd);
		}

		/// <summary>
		/// Centers Camera on Point
		/// </summary>
		/// <param name="Point"><see cref="Vector2"/> to be focused on</param>
		public void Goto(Vector2 Point) {
			Vector2 target = new Vector2();
			target.X = (Point.X - Camera.Bounds.Width / 2);
			target.Y = (Point.Y - Camera.Bounds.Height / 2);
			this.commands.Add(new CameraCommand(INF_TIME, target , true , target));
		}

		/// <summary>
		/// Moves the Camera in a straight line to a point over a number of seconds
		/// </summary>
		/// <param name="point"><see cref="Vector2"/>Vector in terms of the origin</param>
		/// <param name="time">Time in seconds</param>
		public void MoveTo(Graphics.IDrawable target, double time) {
			Vector2 targetpos = new Vector2();
			targetpos.X = (target.Position.X - Camera.Bounds.Width / 2 + target.Area.Width / 2);
			targetpos.Y = (target.Position.Y - Camera.Bounds.Height / 2 + target.Area.Height / 2);
			var cmd = new CameraCommand(time, targetpos, false ,  Vector2.Zero);
			//Thanks to the wonders of references this keeps the target up to date
			cmd.spriteTarget = target;
			this.commands.Add(cmd);
		}

		/// <summary>
		/// Moves the Camera in a straight line to a point over a number of seconds
		/// </summary>
		/// <param name="point"><see cref="Vector2"/>Vector in terms of the origin</param>
		/// <param name="time">Time in seconds</param>
		public void MoveTo(Vector2 point, double time) {
			commands.Add(new CameraCommand( time, point, false ,  Vector2.Zero));
		}
	}
}

