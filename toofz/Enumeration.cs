using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace toofz
{
    // https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
    // https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/#comment-1244455940
    /// <summary>
    /// A base class that provides enum-like semantics. This type can be used to consolidate enum behavior.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public abstract class Enumeration : IComparable
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable> EnumerationCache = new ConcurrentDictionary<Type, IEnumerable>();

        #region Static Members

        /// <summary>
        /// Gets all members of type <typeparamref name="T"/> that are found in the container class.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="EnumerationContainerAttribute"/> was not found on <typeparam name="T"/>.
        /// </exception>
        /// <remarks>
        /// Types deriving from <see cref="Enumeration"/> should specify <see cref="EnumerationContainerAttribute"/> with 
        /// the class that contains instances of the deriving types. The instances should be declared as public static fields.
        /// </remarks>
        public static IEnumerable<T> GetAll<T>()
            where T : Enumeration
        {
            Type container;

            if (Util.TryGetAttributeProperty<Type, EnumerationContainerAttribute>(typeof(T), a => a.ContainerType, out container))
            {
                IEnumerable members;
                if (!EnumerationCache.TryGetValue(container, out members))
                {
                    var fields = container.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
                    members = fields.Select(f => f.GetValue(null));

                    EnumerationCache.TryAdd(container, members);
                }

                return members.OfType<T>();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static T Parse<T>(int id)
            where T : Enumeration
        {
            T result;
            if (TryParse(id, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static T Parse<T>(string name, bool ignoreCase)
            where T : Enumeration
        {
            T result;
            if (TryParse(name, ignoreCase, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"'{name}' is not a member of '{typeof(T)}'.");
            }
        }

        public static T Parse<T>(Func<T, bool> predicate)
            where T : Enumeration
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            T result;
            if (TryParse(predicate, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static bool TryParse<T>(int id, out T result)
            where T : Enumeration
        {
            return TryParse(item => item.Id == id, out result);
        }

        public static bool TryParse<T>(string name, bool ignoreCase, out T result)
            where T : Enumeration
        {
            var comparisonType = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

            return TryParse(item => item.Name.Equals(name, comparisonType), out result);
        }

        public static bool TryParse<T>(Func<T, bool> predicate, out T result)
            where T : Enumeration
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            result = GetAll<T>().FirstOrDefault(predicate);

            return result != null;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class with the specified ID and name.
        /// </summary>
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the instance.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// The name of the instance.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Overrides

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id;

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Enumeration)obj).Id);
        }

        #endregion
    }
}
