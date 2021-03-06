/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/


/* NFX by ITAdapter
 * Originated: 2006.01
 * Revision: NFX 1.0  2013.11.15
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFX
{
  /// <summary>
  /// Some helpful extensions for standard collections
  /// </summary>
  public static class CollectionUtils
  {
    /// <summary>
    /// Runs some method over each element of src sequence
    /// </summary>
    /// <typeparam name="T">Sequence item type</typeparam>
    /// <param name="src">Source sequence</param>
    /// <param name="action">Method to run over each element</param>
    /// <returns>Source sequence (to have ability to chain similar calls)</returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> src, Action<T> action)
    {
      foreach (T item in src)
        action(item);

      return src;
    }

    /// <summary>
    /// Add all values from range sequence to src IDictionary. Source is actually modified.
    /// </summary>
    /// <typeparam name="TKey">Type of key</typeparam>
    /// <typeparam name="TValue">Type of value</typeparam>
    /// <param name="src">Source IDictionary (where to add range)</param>
    /// <param name="range">Sequence that should be added to source IDictionary</param>
    /// <returns>Source with added elements from range (to have ability to chain operations)</returns>
    public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> src, IEnumerable<KeyValuePair<TKey, TValue>> range)
    {
      foreach (KeyValuePair<TKey, TValue> kvp in range)
        src.Add(kvp.Key, kvp.Value);

      return src;
    }
  }
}
