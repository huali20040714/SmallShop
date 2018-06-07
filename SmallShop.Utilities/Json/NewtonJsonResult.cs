using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmallShop.Utilities
{
    public class NewtonJsonResult : JsonResult
    {
        public NewtonJsonResult()
        {
        }

        public NewtonJsonResult(object data)
        {
            Data = data;
        }

        public Formatting Formatting { get; set; } = Formatting.Indented;

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                using (JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting })
                {
                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings());
                    serializer.Converters.Add(new EnumNameValueConverter());
                    IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter();
                    dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                    serializer.Converters.Add(dateTimeConverter);
                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
        }
    }
}