using System;
using Rhovlyn.Engine.Input;
using System.Collections.Generic;

namespace Rhovlyn.Engine.Managers
{
	public class InputManager
	{
		Dictionary< string , IInputProvider > inputs;



		public InputManager(string settingsPath )
		{
			inputs = new Dictionary< string , IInputProvider >();
		}

		#region Management
		public bool this[string name]
		{
			get {
				foreach (var pro in inputs.Values )
				{
					if (pro.GetState(name))
					{
						return true;
					}
				}
				return false;
			}
		} 

		public bool Add (string name , IInputProvider state )
		{
			if (!Exists(name))
			{
				inputs.Add(name, state);
				return true;
			}
			return false;
		}

		public bool Exists ( string name)
		{
			return this.inputs.ContainsKey(name);
		}
		#endregion
	}
}

