using System;
using System.IO;

namespace Rhovlyn.Engine.Input
{
	public interface IInputProvider
	{
		/// <summary>
		/// The end of the Provider's settings to be checked when parsing the settings
		/// </summary>
		string SettingsPostfix {get; }

		/// <summary>
		/// Check to see if a name exists
		/// </summary>
		/// <param name="name">Name to check</param>
		bool Exists( string name );

		/// <summary>
		/// Load configurations from a stream
		/// </summary>
		/// <param name="path">Path to a file that contains settings for the provider</param>
		/// <remarks>It is recommended to use IO.Settings to manage these settings</remarks>
		bool Load ( string path );

		/// <summary>
		/// Unload call to give a chance to clean-up and/or save settings
		/// </summary>
		void Unload ();

		/// <summary>
		/// Get the current state of the input
		/// </summary>
		/// <returns><c>true</c>, if state was was in the specified state, <c>false</c> otherwise.</returns>
		/// <param name="name">Name of condition</param>
		bool GetState ( string name );

		/// <summary>
		/// Set the condition needed for a true state of the input to occur
		/// </summary>
		/// <returns><c>true</c> if condition was met, <c>false</c> otherwise.</returns>
		/// <param name="name">Name of condition</param>
		/// <param name="condition">Condition to be met</param>
		/// <typeparam name="T">The type of the condition</typeparam>
		bool SetCondition<T> (string name , T condition );

		/// <summary>
		/// Adds the condition with given name
		/// </summary>
		/// <param name="name">Name of the newly added condition</param>
		/// <param name="condition">Condition to be added</param>
		/// <typeparam name="T">Type of the Condition</typeparam>
		/// <returns><c>true</c> when successfully added</returns> 
		bool Add<T> (string name , T condition);

		/// <summary>
		/// Parse all key configs that end with the SettingPostfix that are within the loaded Settings
		/// </summary>
		/// <remarks>
		/// Name of the condition is set to header.key with the SettingPostfix removed
		/// </remarks>
		void ParseSettings ();

	}
}

