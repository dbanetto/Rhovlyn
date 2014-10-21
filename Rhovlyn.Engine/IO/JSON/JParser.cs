using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using SharpDL.Graphics;

namespace Rhovlyn.Engine.IO.JSON
{
	public delegate object JsonParser(JToken obj);

	public static class JParser
	{
		private static Dictionary<Type, JsonParser > parsers = new Dictionary<Type, JsonParser>();
		private static Dictionary<Type, JsonSchema > schemas = new Dictionary<Type, JsonSchema>();

		public static bool Inited { get; private set; }

		public static void Init()
		{
			if (Inited)
				return;
			Inited = true;

			var rholib = Assembly.GetExecutingAssembly();
			using (TextReader reader = new StreamReader(rholib.GetManifestResourceStream("Rectangle.schema"))) {
				JParser.Add<Rectangle>((i) => {
					var array = (JArray)(i);
					return new Rectangle((int)(array[0]), (int)(array[1]), (int)(array[2]), (int)(array[3]));
				}, JsonSchema.Read(new JsonTextReader(reader)));
			}


		}

		public static bool TryParse<T>(JToken obj, ref T result)
		{
			if (!Inited)
				Init();

			if (parsers.ContainsKey(typeof(T))) {
				if (!obj.IsValid(schemas[typeof(T)]))
					return false;

				object parsed = null;
				try {
					parsed = parsers[typeof(T)](obj);
				} catch (Exception ex) {
					Console.WriteLine(String.Format("Error while parsing {0} : {1}", typeof(T), ex));
				}
				if (parsed != null) {
					result = (T)(parsed);
					return true;
				}
				return false;
			}
			throw new NotImplementedException(String.Format("Cannot parse unknown type: {0}", typeof(T)));
		}

		public static T Parse<T>(JToken obj)
		{
			if (!Inited)
				Init();

			if (parsers.ContainsKey(typeof(T))) {
				if (obj.IsValid(schemas[typeof(T)]))
					return (T)(parsers[typeof(T)](obj));
				throw new JsonSchemaException(String.Format("Invalid Schema for {0}. Json : {1}", typeof(T), obj));
			}
			throw new NotImplementedException(String.Format("Cannot parse unknown type: {0}", typeof(T)));
		}

		public static bool Add<T>(JsonParser parser, JsonSchema schema, bool overrides = false)
		{
			if (!Inited)
				Init();

			if (parsers.ContainsKey(typeof(T))) {
				if (overrides) {
					parsers[typeof(T)] = parser;
					schemas[typeof(T)] = schema;
					Console.WriteLine(String.Format("The Parser for {0} is overriden", typeof(T)));
					return true;
				}
				throw new Exception(String.Format("The Parser for {0} already exists", typeof(T)));
			}
			parsers.Add(typeof(T), parser);
			schemas.Add(typeof(T), schema);
			return true;
		}

		public static bool Exists<T>()
		{
			return parsers.ContainsKey(typeof(T));
		}

		public static bool IsValid<T>(JToken obj)
		{
			if (!Inited)
				Init();

			return Exists<T>() && obj.IsValid(schemas[typeof(T)]);
		}
	}
}
