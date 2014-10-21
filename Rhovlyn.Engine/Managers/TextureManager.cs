using System;
using System.Collections.Generic;
using System.IO;
using Rhovlyn.Engine.Graphics;
using System.Reflection;
using Rhovlyn.Engine.IO.JSON;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpDL.Graphics;

namespace Rhovlyn.Engine.Managers
{
	public class TextureManager
	{
		//private static GraphicsDevice graphics;
		private Dictionary< string , SpriteMap> textures;

		public TextureManager(Renderer renderer)
		{
			//TextureManager.graphics = graphics;
			textures = new Dictionary<string, SpriteMap>();

			// Add to JSON Parser
			var rholib = Assembly.GetExecutingAssembly();
			using (TextReader reader = new StreamReader(rholib.GetManifestResourceStream("SpriteMap.schema"))) {
				JParser.Add<SpriteMap>((i) => { 
					var path = i["path"].ToString();
					var name = i["name"].ToString();
					SpriteMap texture = null;

					var texmap = (JArray)i["map"];
					if (texmap[0] is JArray) {
						var frames = new List<Rectangle>();
						foreach (var item in texmap) {
							frames.Add(JParser.Parse<Rectangle>(item));
						}

						texture = new SpriteMap(new Texture(renderer, new Surface(path, SurfaceType.PNG))
							, name
							, frames);
					} else if (texmap[0] is JValue) {
						int width = (int)(texmap[0]);
						int height = (int)(texmap[1]);

						texture = new SpriteMap(new Texture(renderer, new Surface(path, SurfaceType.PNG))
							, name
							, width, height);
					}

					if (i["animations"] != null) {
						var animations = (JObject)(i["animations"]);
						foreach (var item in animations) {
							texture.AddAnimation(item.Key, JParser.Parse<Animation>(item.Value));
						}
					}

					// HACK : This will break if there are multiple TextureManagers trying to load to different graphics devices
					return texture;
				}, JsonSchema.Read(new JsonTextReader(reader)));
			}

			using (TextReader reader = new StreamReader(rholib.GetManifestResourceStream("Animation.schema"))) {
				JParser.Add<Animation>((i) => { 

					if (i is JArray) {
						var array = (JArray)(i);
						var frames = new List<int>();
						var timings = new List<double>();
						foreach (var item in array) {
							if (item is JArray) {
								var ar = (JArray)(item);
								frames.Add((int)(ar[0]));
								timings.Add((double)(ar[1]));
							}
						}
						return new Animation(frames, timings);
					} else {
						return new Animation(new List<int> { (int)i }, new List<double> { 0.0 });
					}
				}, JsonSchema.Read(new JsonTextReader(reader)));
			}
		}

		#region Management

		public SpriteMap this [string index] {
			get { return textures[index]; }
			set { textures[index] = value; }

		}

		public bool Add(SpriteMap spritemap)
		{
			if (String.IsNullOrEmpty(spritemap.Name))
				throw new Exception("Cannot add a Texture with a empty name");

			if (!Exists(spritemap.Name)) {
				textures.Add(spritemap.Name, spritemap);
				return true;
			}
			return false;
		}

		public bool Add(string path)
		{
			try {
				return Add(JParser.Parse<SpriteMap>(JObject.Parse(File.ReadAllText(path))));
			} catch (JsonSchemaException ex) {
				Console.WriteLine(ex);
				return false;
			}
		}

		public bool Exists(string name)
		{
			return textures.ContainsKey(name);
		}

		/// <summary>
		/// Load a local file.
		/// </summary>
		/// <param name="path">Path</param>
		public bool Load(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open)) {
				return Load(fs);
			}
		}

		/// <summary>
		/// Load the specified stream.
		/// </summary>
		/// <param name="stream">Input Stream</param>
		public bool Load(Stream stream)
		{
			using (var reader = new StreamReader(stream)) {

				JObject json;

				try {
					json = JObject.Parse(reader.ReadToEnd());
				} catch (JsonReaderException ex) {
					Console.WriteLine(ex);
					return false;
				}

				if (json["Textures"] == null) {
					return false;
				}

				var textures = (JArray)(json["Textures"]);
				foreach (JObject texture in textures) {
					if (!Add(JParser.Parse<SpriteMap>(texture))) {
						Console.WriteLine("Something went wrong?!");
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Remove the SpriteMap with the specified name safely.
		/// </summary>
		/// <param name="name">Name of the sprite</param>
		public bool Remove(string name)
		{
			if (Exists(name)) {
				textures[name].Texture.Dispose();
				textures.Remove(name);
			}
			return false;
		}

		#endregion

	}
}

