using System;
using System.Text.Json;
using AssemblyAi.Common.Enums;

namespace AssemblyAi.Helpers.Interfaces
{
	internal interface IAcousticModelEnumConvertor
	{
		AcousticModelEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
		void Write(Utf8JsonWriter writer, AcousticModelEnum value, JsonSerializerOptions options);
	}
}