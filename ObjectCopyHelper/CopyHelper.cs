using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ObjectCopyHelper
{
    public static class CopyHelper
    {
        public static T CopyTo<T>(this T source)
        {
            if (source == null) throw new ArgumentNullException();

            using (var mem = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(mem, source);
                mem.Position = 0;
                return (T)formatter.Deserialize(mem);
            }
        }

        public static T CopyTo<T>(this T source, string propertyName, object value)
        {
            if(source == null || propertyName == null || value == null)
            {
                throw new ArgumentNullException();
            }

            return source.CopyTo(new[] { (propertyName, value) });
        }

        public static T CopyTo<T>(this T source, IEnumerable<(string propertyName, object value)> propertyAndValues)
        {
            if(propertyAndValues == null)
            {
                throw new ArgumentNullException();
            }

            var copied = source.CopyTo();

            foreach (var (propertyName, value) in propertyAndValues) 
            {
                var property = copied.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null)
                {
                    throw new ArgumentException();
                }

                if (property.CanWrite)
                {
                    property.SetValue(copied, value);
                }
                else
                {
                    var field = copied.GetType().GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field == null)
                    {
                        throw new ArgumentException();
                    }
                    field.SetValue(copied, value);
                }
            }
            return copied;
        }
    }
}
