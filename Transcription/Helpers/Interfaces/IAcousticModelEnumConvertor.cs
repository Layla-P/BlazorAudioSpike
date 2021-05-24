using System;
using System.Text.Json;
using Transcription.Common.Enums;

namespace Transcription.Helpers.Interfaces
{
	internal interface IAcousticModelEnumConvertor
	{
		AcousticModelEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
		void Write(Utf8JsonWriter writer, AcousticModelEnum value, JsonSerializerOptions options);
	}
}