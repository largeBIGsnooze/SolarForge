using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Solar.Rendering;

namespace SolarForge.Skyboxes
{

	public class SkyboxNameJsonConverter : JsonConverter<SkyboxName>
	{

		public override SkyboxName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return new SkyboxName(reader.GetString());
		}


		public override void Write(Utf8JsonWriter writer, SkyboxName skyboxName, JsonSerializerOptions options)
		{
			writer.WriteStringValue(skyboxName.ToString());
		}
	}
}
