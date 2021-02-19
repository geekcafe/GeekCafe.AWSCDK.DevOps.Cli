using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekCafe.AWSCDK.DevOps.Cli.Utils
{
    public class FactoryHelpers
    {
        /// <summary>
        /// Helper function to get all classes that implement a specific type or interface.
        /// Most often used in factory calls
        /// </summary>
        /// <param name="type">The type to fitler on</param>
        /// <returns>Returns a list of classes or null</returns>
        public static IEnumerable<Type> GetMatchingTypes(Type type)
        {
            try
            {
                var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(c => type.IsAssignableFrom(c) && c.IsClass);

                return types;
            }
            catch
            {
                return null;
            }
        }
    }
}
