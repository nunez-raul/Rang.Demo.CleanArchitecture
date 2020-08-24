using Rang.Demo.CleanArchitecture.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rang.Demo.CleanArchitecture.Domain.Common
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsDuplicates<T>(this IEnumerable<T> enumerable)
        {
            var knownKeys = new HashSet<T>();
            return enumerable.Any(item => !knownKeys.Add(item));
        }
    }
}
