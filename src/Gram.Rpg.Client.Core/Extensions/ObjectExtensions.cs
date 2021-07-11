using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static Task<T> AsCompletedTask<T>(this T obj) => Task.FromResult(obj);

        public static Type GetTypeSafely(this object obj)
        {
            return obj?.GetType();
        }

        public static T AssignFrom<T>(this T dst, object src)
        {
            return src.AssignTo(dst);
        }
        
        public static T AssignTo<T>(this object src, T dst)
        {
            var srcType = src.GetType();
            var dstType = typeof(T);

            var srcMembers = srcType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            var dstMembers = dstType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);

            foreach (var smi in srcMembers)
            {
                var dmi = dstMembers.FirstOrDefault(x => x.Name == smi.Name);

                if (dmi == null)
                    continue;

                object srcVal;

                if (smi is FieldInfo sfi)
                    srcVal = sfi.GetValue(src);
                else if (smi is PropertyInfo spi)
                    srcVal = spi.GetValue(src);
                else
                    continue;

                if (dmi is FieldInfo dfi)
                    dfi.SetValue(dst, srcVal);
                else if (dmi is PropertyInfo dpi)
                    dpi.SetValue(dst, srcVal);
            }

            return dst;
        }

        public static T Inject<T>(this T instance)
        {
            return SL.Inject(instance);
        }
    }
}
