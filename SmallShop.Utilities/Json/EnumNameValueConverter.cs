using System;
using System.Reflection;
using Newtonsoft.Json;

namespace SmallShop.Utilities
{
    public class EnumNameValueConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string name = value.ToString();
            object typeName = value.GetType().InvokeMember("GetTypeCode", BindingFlags.InvokeMethod, null, value, null);
            object objVal = Convert.ChangeType(value, (TypeCode)typeName);
            if (objVal == null) throw new ArgumentNullException(nameof(objVal));
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(name);
            writer.WritePropertyName("Value");
            writer.WriteValue(objVal.ToString());
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }
}