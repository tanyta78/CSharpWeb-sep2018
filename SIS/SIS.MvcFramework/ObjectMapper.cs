namespace SIS.MvcFramework
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ObjectMapper
    {
        public static T To<T>(this object source)
            where T : new()
        {
            var destination = new T();
            var destinationProps = destination.GetType().GetProperties();

            foreach (var destinationProp in destinationProps)
            {
                if (destinationProp.SetMethod == null)
                {
                    continue;
                }

                var sourceProp = source.GetType()
                                        .GetProperties()
                                        .FirstOrDefault(x =>
                                           x.Name.ToLower() == destinationProp.Name.ToLower());

                if (sourceProp?.GetMethod != null)
                {
                    var sourceValue = sourceProp.GetMethod.Invoke(source, new object[0]);
                    var destinationValue = TryParse(sourceValue.ToString(), destinationProp.PropertyType);
                    destinationProp.SetMethod.Invoke(destination, new [] { destinationValue});
                }
            }

            return destination;
        }

        public static object TryParse(string strValue, Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            object value = null;
            switch (typeCode)
            {
                case TypeCode.Int32:
                    if (int.TryParse(strValue, out var intResult))
                    {
                        value = intResult;
                    }
                    break;
                case TypeCode.Int64:
                    if (long.TryParse(strValue, out var longResult))
                    {
                        value = longResult;
                    }
                    break;
                case TypeCode.Double:
                    if (double.TryParse(strValue, out var doubleResult))
                    {
                        value = doubleResult;
                    }
                    break;
                case TypeCode.Decimal:
                    if (decimal.TryParse(strValue, out var decimalResult))
                    {
                        value = decimalResult;
                    }
                    break;
                case TypeCode.Char:
                    if (char.TryParse(strValue, out var charResult))
                    {
                        value = charResult;
                    }
                    break;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(strValue, out var dateResult))
                    {
                        value = dateResult;
                    }
                    break;
                case TypeCode.String:
                    value = strValue;
                    break;
            }

            return value;
        }
    }
}
