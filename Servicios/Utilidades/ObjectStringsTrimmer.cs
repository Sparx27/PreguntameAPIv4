using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Utilidades
{
    public static class ObjectStringsTrimmer<T>
    {
        public static void Ejecutar(T obj)
        {
            var atributosString = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => property.PropertyType == typeof(string) && property.CanWrite && property.CanRead);

            foreach(var property in atributosString)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    property.SetValue(obj, value.ToString().Trim());
                }
            }
        }
    }
}
