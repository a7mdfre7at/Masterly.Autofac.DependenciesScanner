using System;
using System.Collections.Generic;
using System.Linq;

namespace Masterly.Autofac.DependenciesScanner
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get all interfaces and abstract classes those are assignable from a given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetInterfacesAndAbstractClasses(this Type type)
        {
            if (type.BaseType == null)
                return new Type[0];

            ICollection<Type> baseTypes = new List<Type>(type.GetInterfaces());
            Type currentType = type;

            while ((currentType = currentType.BaseType) != null)
            {
                if (currentType.IsInterface || currentType.IsAbstract)
                    baseTypes.Add(currentType);
            }

            return baseTypes.ToArray();
        }
    }
}