using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Helpers
{ 
    internal class EnumConvertor<TEnum> : JsonConverter<TEnum> where TEnum : struct,  Enum
        {
            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                
                var description = reader.GetString();
                var values = Enum.GetValues<TEnum>();

                var enumType = typeof(TEnum);
                foreach (var value in values)
                {
                    var memberInfos = enumType.GetMember(value.ToString());
                    var memberInfo = memberInfos.FirstOrDefault(item => item.DeclaringType == enumType);
                    var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttribute?.Description == description)
                        return value;
                }

                return default(TEnum);
            }


            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                var enumType = typeof(TEnum);
                var memberInfos = enumType.GetMember(value.ToString());
                var memberInfo = memberInfos.FirstOrDefault(item => item.DeclaringType == enumType);
                var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();

                writer.WriteStringValue(descriptionAttribute?.Description);
            }
        }
    }
