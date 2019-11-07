namespace Sample.Tris.WebApi.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Sample.Tris.Lib.Exceptions;
    using Sample.Tris.WebApi.Models;

    public class PointDtoTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string valueStr = (string)value;
                string[] strCoords = valueStr.Split(",");

                if (strCoords.Length != 2)
                {
                    throw new TrisLibValidationException("Point value must represented in the format 'x,y'");
                }

                if (!int.TryParse(strCoords[0], out int x))
                {
                    throw new TrisLibValidationException("Invalid value specified for x position");
                }

                if (!int.TryParse(strCoords[1], out int y))
                {
                    throw new TrisLibValidationException("Invalid value specified for y position");
                }

                return new PointDto(x, y);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
