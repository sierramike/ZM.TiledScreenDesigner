using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.TiledScreenDesigner
{
    public class ImageJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var strValue = (string)reader.Value;
            if (strValue != null)
                return Image.FromStream(new MemoryStream(Convert.FromBase64String(strValue)));
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var img = (Image)value;

            var ms = new MemoryStream();
            img.Save(ms, img.RawFormat);

            byte[] b = ms.ToArray();

            writer.WriteValue(b);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Image));
        }
    }
}
