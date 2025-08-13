using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarForge.Utility
{

	public class ColorJsonConverter : JsonConverter<Color>
	{

		public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return ColorTranslator.FromHtml(reader.GetString());
		}


		public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(string.Concat(new string[]
			{
				"#",
				value.A.ToString("X2"),
				value.R.ToString("X2"),
				value.G.ToString("X2"),
				value.B.ToString("X2").ToLower()
			}));
		}
	}
}
